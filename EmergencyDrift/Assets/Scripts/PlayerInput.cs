using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private FloatReference _steeringInput;
    [SerializeField] private FloatReference _gasInput;
    [SerializeField] private FloatReference _breakInput;
    [SerializeField] private GameEvent _driftEngaged;
    [SerializeField] private GameEvent _driftDesignated;

    public void SteeringInput(InputAction.CallbackContext ctx)
    {
        _steeringInput.variable.value = ctx.ReadValue<Vector2>().x;
    }

    public void GasInput(InputAction.CallbackContext ctx)
    {
        _gasInput.variable.value = ctx.ReadValue<float>();
    }

    public void BreakInput(InputAction.CallbackContext ctx) 
    {
        _breakInput.variable.value = ctx.ReadValue<float>();
    }

    public void DriftInput(InputAction.CallbackContext ctx)
    {
        if (ctx.action.WasPressedThisFrame()) _driftEngaged.Raise(this, EventArgs.Empty);

        if(ctx.action.WasReleasedThisFrame()) _driftDesignated.Raise(this,EventArgs.Empty);
    }
}