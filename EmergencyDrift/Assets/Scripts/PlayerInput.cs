using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private FloatReference _steeringInput;
    [SerializeField] private FloatReference _gasInput;
    [SerializeField] private FloatReference _breakInput;

    public void SteeringInput(InputAction.CallbackContext ctx)
    {
        _steeringInput.variable.value = ctx.ReadValue<Vector2>().x;
    }

    public void GasInput(InputAction.CallbackContext ctx)
    {
        _gasInput.variable.value = ctx.ReadValue<float>();
    }

    public void breakInput(InputAction.CallbackContext ctx) 
    {
        _breakInput.variable.value = ctx.ReadValue<float>();
    }
}