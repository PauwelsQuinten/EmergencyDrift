using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private float _ambulancePrice = 290f;
    [SerializeField] private float _speedPrice = 120f;
    [SerializeField] private float _radarPrice = 540;
    [SerializeField] private float _upgradeMultiplier = 1.2f;

    [SerializeField] private FloatReference _money;
    [SerializeField] private FloatReference _ambulanceCount;
    [SerializeField] private FloatReference _speed;
    [SerializeField] private FloatReference _maxSpeed;
    [SerializeField] private GameObject _radar;
    [SerializeField] private GameObject _shop;
    [SerializeField] private GameObject _debtScreen;

    [SerializeField] private Button _ambulanceButton;
    [SerializeField] private Button _speedButton;
    [SerializeField] private Button _radarButton;

    [SerializeField] private GameEvent _boughtAmbulance;
    [SerializeField] private GameEvent _updateMoney;


    private float _currentAmbulancePrice;
    private float _currentSpeedPrice;

    private TextMeshProUGUI _ambulanceText;
    private TextMeshProUGUI _speedText;
    private TextMeshProUGUI _radarText;

    private Gamepad _gamepad;
    private EventSystem _eventsystem;

    private void Start()
    {
        _currentAmbulancePrice = _ambulancePrice;
        _currentSpeedPrice = _speedPrice;

        _ambulanceText = _ambulanceButton.GetComponentInChildren<TextMeshProUGUI>();
        _speedText = _speedButton.GetComponentInChildren <TextMeshProUGUI>();
        _radarText = _radarButton.GetComponentInChildren<TextMeshProUGUI>();

        _ambulanceText.text = $"+1 Ambulance {_currentAmbulancePrice}";
        _speedText.text = $"+ Speed {_currentSpeedPrice}";
        _radarText.text = $"Buy Radar {_radarPrice}";

        _gamepad = Gamepad.current;
        _eventsystem = GameObject.FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        if (_gamepad.buttonNorth.wasPressedThisFrame || _shop.active == true && _gamepad.buttonEast.wasPressedThisFrame)
        {
            if (_shop.active == true)
            {
                _shop.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                _eventsystem.SetSelectedGameObject(_speedButton.gameObject);
                if (_debtScreen.active == true) _debtScreen.SetActive(false);
                _shop.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    public void BuyAmbulance()
    {
        if (_currentAmbulancePrice > _money.value) return;
        _money.variable.value -= _currentAmbulancePrice;

        _currentAmbulancePrice = (int)((_currentAmbulancePrice * _upgradeMultiplier) + _ambulancePrice);

        _ambulanceText.text = $"+1 Ambulance {_currentAmbulancePrice}";
        _boughtAmbulance.Raise();
        _updateMoney.Raise();
    }
    public void BuySpeed()
    {
        if (_currentSpeedPrice > _money.value) return;
        _money.variable.value -= _currentSpeedPrice;

        _currentSpeedPrice = (int)((_currentSpeedPrice * _upgradeMultiplier) + _speedPrice);

        _maxSpeed.variable.value += 1f;

        _speedText.text = $"+ Speed {_currentSpeedPrice}";
        _updateMoney.Raise();
    }

    public void BuyRadar()
    {
        if (_radarPrice > _money.value) return;
        _money.variable.value -= _radarPrice;

        _radarButton.interactable = false;
        _radar.SetActive(true);
        _radarButton.image.color = Color.green;
        _radarText.text = "Upgrade purchased";
        _updateMoney.Raise();
        _eventsystem.SetSelectedGameObject(_speedButton.gameObject);
    }
}
