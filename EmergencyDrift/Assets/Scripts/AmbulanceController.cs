using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class AmbulanceController : MonoBehaviour
{
    [Header("Model")]
    [SerializeField] private GameObject _model;

    [Header("Moving")]
    [SerializeField] private FloatReference _moveSpeed;
    [SerializeField] private FloatReference _maxSpeed;
    [SerializeField] private float _drag = 0.98f;
    [SerializeField] private AudioSource _enginesound;

    [Header("Steering")]
    [SerializeField] private float _steerAngle = 30f;
    [SerializeField] private float _traction = 2f;
    [SerializeField] private float _driftSpeed = 2f;
    [SerializeField] private AudioSource _driftSound;

    [Header("Collision")]
    [SerializeField] private float _bouncyness = 5f;
    [SerializeField] private float _collisionMultiplier = 10f;
    [SerializeField] private AudioSource _hitSound;

    [Header("Health")]
    [SerializeField] private FloatReference _health;
    [SerializeField] private GameObject _HealthbarObject;
    [SerializeField] private GameEvent _died;

    private Vector3 _moveForce;
    private Slider _healthBar;
    private Gamepad _gamepad;
    private Rigidbody _rb;

    private void Start()
    {
        _healthBar = _HealthbarObject.GetComponent<Slider>();
        _healthBar.value = _health.value;
        _enginesound.pitch = 0.5f;
        _gamepad = Gamepad.current;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_gamepad == null)
        {
            _gamepad = Gamepad.current;
            return;
        }
        GasInput();
        DragAndTraction();
    }

    private void FixedUpdate()
    {
        if (_gamepad == null) return;
        Steering();
        _rb.Move(transform.position + _moveForce * Time.deltaTime, transform.rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 7) return;
        // Calculate the current speed magnitude (this is the speed of the ambulance)
        float speed = _moveForce.magnitude;

        // Calculate damage based on speed and the collision multiplier
        float damage = speed * _collisionMultiplier;

        // push player away from collision
        Vector3 direction = transform.position - collision.transform.position;
        direction.y = 0;
        _rb.AddForce(direction * _bouncyness, ForceMode.Impulse);


        //Debug.Log(damage);
        // Apply the damage to health
        _health.variable.value -= damage;
        _healthBar.value = _health.value;
        _hitSound.Play();

        //Debug.Log(_health);

        // Check if health falls below zero and handle accordingly
        if (_health.value <= 0)
        {
            Debug.Log("Ambulance Destroyed!");
            _died.Raise();
        }
        else
        {
           Debug.Log("Damage Taken: " + damage + ", Remaining Health: " + _health);
        }
    }

    private void GasInput()
    {
        _moveForce += transform.forward * _moveSpeed.value * (_gamepad.rightTrigger.ReadValue() - _gamepad.leftTrigger.ReadValue()) * Time.deltaTime;

        _moveForce = Vector3.ClampMagnitude(_moveForce, _maxSpeed.value);

        float enginePitch;
        if (_gamepad.rightTrigger.ReadValue() == 0 && _gamepad.leftTrigger.ReadValue() == 0) enginePitch = 0.5f;
        else enginePitch = 0.5f + (_gamepad.rightTrigger.ReadValue() / 2) + (_gamepad.leftTrigger.ReadValue() / 2);
        enginePitch = Mathf.Clamp(enginePitch, 0.5f, 1f);
        _enginesound.pitch = enginePitch;
    }

    private void Steering()
    {
        float steerInput = _gamepad.leftStick.ReadValue().x;
        if(_gamepad.leftTrigger.ReadValue() != 0) transform.Rotate(Vector3.up * -steerInput * _moveForce.magnitude * _steerAngle * Time.deltaTime);
        else transform.Rotate(Vector3.up * steerInput * _moveForce.magnitude * _steerAngle * Time.deltaTime);

        // Drifting
        if (steerInput != 0 && _gamepad.rightTrigger.ReadValue() != 0)
        {
            float driftAngle = Mathf.Clamp(steerInput * 45, -45, 45) * Mathf.Clamp01(_gamepad.rightTrigger.ReadValue());
            Quaternion targetRotation = Quaternion.Euler(0, driftAngle, 0);

            _model.transform.localRotation = Quaternion.Slerp(_model.transform.localRotation, targetRotation, Time.deltaTime * _driftSpeed);
        }
        else
        {
            // If there's no input, gradually reset the car's tilt
            Quaternion straightRotation = Quaternion.Euler(0, 0, 0);
            _model.transform.localRotation = Quaternion.Slerp(_model.transform.localRotation, straightRotation, Time.deltaTime * _driftSpeed);
        }

        if (steerInput >= 0.25f && _gamepad.rightTrigger.ReadValue() != 0 || steerInput <= -0.25f && _gamepad.rightTrigger.ReadValue() != 0)
        {
            if (!_driftSound.isPlaying) _driftSound.Play();
        }
        else
        {
            if (_driftSound.isPlaying) _driftSound.Stop();
        }
    }

    private void DragAndTraction()
    {
        // Drag and max speed limit
        _moveForce *= _drag;

        // Traction
        Debug.DrawRay(transform.position, _moveForce.normalized * 3);
        Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);
        _moveForce = Vector3.Lerp(_moveForce.normalized, transform.forward, _traction * Time.deltaTime) * _moveForce.magnitude;
    }
}
