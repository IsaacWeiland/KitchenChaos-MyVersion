using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameControls : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "inputBindings";
    
    public static GameControls Instance;
    
    
    public event EventHandler OnInteractAction;
    public event EventHandler OnInterAltAct;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;
    
    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlt,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlt,
        Gamepad_Pause,
    }
    private PlayerInputActions _playerInputActions;

    

    private void Awake()
    {
        Instance = this;
        _playerInputActions = new PlayerInputActions();
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            _playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Interact.performed += InteractOnperformed;
        _playerInputActions.Player.InteractAlt.performed += InteractAltOnperformed;
        _playerInputActions.Player.Pause.performed += PausePerformed;
    }

    private void OnDestroy()
    {
        _playerInputActions.Player.Interact.performed -= InteractOnperformed;
        _playerInputActions.Player.InteractAlt.performed -= InteractAltOnperformed;
        _playerInputActions.Player.Pause.performed -= PausePerformed;
        
        _playerInputActions.Dispose();
    }

    private void InteractAltOnperformed(InputAction.CallbackContext obj)
    {
    OnInterAltAct?.Invoke(this, EventArgs.Empty);
    }

    private void InteractOnperformed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void PausePerformed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this,EventArgs.Empty);
    }
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Interact:
                return _playerInputActions.Player.Interact.bindings[0].ToDisplayString();
                break;
            case Binding.InteractAlt:
                return _playerInputActions.Player.InteractAlt.bindings[0].ToDisplayString();
                break;
            case Binding.Move_Up:
                return _playerInputActions.Player.Move.bindings[1].ToDisplayString();
                break;
            case Binding.Move_Down:
                return _playerInputActions.Player.Move.bindings[2].ToDisplayString();
                break;
            case Binding.Move_Left:
                return _playerInputActions.Player.Move.bindings[3].ToDisplayString();
                break;
            case Binding.Move_Right:
                return _playerInputActions.Player.Move.bindings[4].ToDisplayString();
                break;
            case Binding.Pause:
                return _playerInputActions.Player.Pause.bindings[0].ToDisplayString();
                break;
            case Binding.Gamepad_Interact:
                return _playerInputActions.Player.Interact.bindings[1].ToDisplayString();
                break;
            case Binding.Gamepad_InteractAlt:
                return _playerInputActions.Player.InteractAlt.bindings[1].ToDisplayString();
                break;
            case Binding.Gamepad_Pause:
                return _playerInputActions.Player.Pause.bindings[1].ToDisplayString();
                break;
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        InputAction inputAction;
        int bindingIndex;
        _playerInputActions.Disable();
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = _playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlt:
                inputAction = _playerInputActions.Player.InteractAlt;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = _playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.Gamepad_Interact:
                inputAction = _playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_InteractAlt:
                inputAction = _playerInputActions.Player.InteractAlt;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Pause:
                inputAction = _playerInputActions.Player.Pause;
                bindingIndex = 1;
                break;
        }
        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                _playerInputActions.Player.Enable();
                onActionRebound();
                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, _playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
                OnBindingRebind?.Invoke(this,EventArgs.Empty);
            }).Start();
    }
}
