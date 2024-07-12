using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static UserInput instance;

    public Vector2 MoveInput { get; private set; }
    public bool JumpJustPressed { get; private set; }
    public bool JumpBeingHeld { get; private set; }
    public bool JumpJustReleased { get; private set; }
    public bool DashInput { get; private set; }
    public bool CrouchInput { get; private set; }
    public bool OpenPauseMenuInput { get; private set; }
    public bool Att1Input { get; private set; }
    public bool Att2Input { get; private set; }
    public bool SpeAtt1Input { get; private set; }
    public bool SpeAtt2Input { get; private set ; }


    private PlayerInput _PlayerInput;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _dashAction;
    private InputAction _crouchAction;
    private InputAction _menuPauseAction;
    private InputAction _att1Action;
    private InputAction _att2Action;
    private InputAction _SpeAtt1Action;
    private InputAction _SpeAtt2Action;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _PlayerInput = GetComponent<PlayerInput>();

        SetupInputActions();
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void SetupInputActions()
    {
        _moveAction = _PlayerInput.actions["Move"];
        _jumpAction = _PlayerInput.actions["Jump"];
        _crouchAction = _PlayerInput.actions["Crouch"];
        _dashAction = _PlayerInput.actions["Dash"];
        _menuPauseAction = _PlayerInput.actions["Pause"];
        _att1Action = _PlayerInput.actions["Attack 1"];
        _att2Action = _PlayerInput.actions["Attack 2"];
        _SpeAtt1Action = _PlayerInput.actions["Special Attack 1"];
        _SpeAtt2Action = _PlayerInput.actions["Special Attack 2"];
    }

    private void  UpdateInputs()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();
        JumpJustPressed = _jumpAction.WasPressedThisFrame();
        JumpBeingHeld = _jumpAction.IsPressed();
        JumpJustReleased = _jumpAction.WasReleasedThisFrame(); 
        CrouchInput = _crouchAction.IsPressed();
        DashInput = _dashAction.WasPressedThisFrame();
        OpenPauseMenuInput = _menuPauseAction.WasPressedThisFrame();
        Att1Input = _att1Action.WasPressedThisFrame();
        Att2Input = _att2Action.WasPressedThisFrame();
        SpeAtt1Input = _SpeAtt1Action.WasPressedThisFrame();
        SpeAtt2Input = _SpeAtt2Action.WasPressedThisFrame();
    }
}