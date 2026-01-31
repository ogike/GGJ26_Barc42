using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static UserInput Instance { get; private set; }
    
    //public API
    public bool InteractButtonPressedThisFrame { get; private set; }
    public bool InteractButtonHeldThisFrame { get; private set; }
    
    public bool PauseMenuPressedThisFrame { get; private set; }

    public bool GoToPointPressedThisFrame { get; private set; }
    public Vector2 MoveTarget { get; private set; }
    
    public bool DialogueContinuePressedThisFrame {get ; private set; }
    public bool DialogueAutoContinueHeldThisFrame {get ; private set; }
    public bool DialogueAutoContinuePressedThisFrame { get; private set; }
    public bool DialogueAutoContinueReleasedThisFrame { get; private set; }
    public bool DialogueNextChoicePressedThisFrame { get; private set; }
    public bool DialoguePrevChoicePressedThisFrame { get; private set; }
    
    //actions
    private InputAction _menuAction;
    private InputAction _interactAction;
    private InputAction _dialogueAutoContinueAction;
    private InputAction _dialogueNextChoice;
    private InputAction _dialoguePrevChoice;
    
    private InputAction _moveAction;

    //other priv references
    private PlayerInput _playerInput;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"More than one {this} in scene!");
            return;
        }
        Instance = this;
        
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        
        _interactAction = _playerInput.actions["Interact"];
        _dialogueAutoContinueAction = _playerInput.actions["DialogueAutoContinue"];
        _dialogueNextChoice = _playerInput.actions["DialogueNextChoice"];
        _dialoguePrevChoice = _playerInput.actions["DialoguePrevChoice"];
        _menuAction = _playerInput.actions["PauseMenu"];
    }

    private void Update()
    {
        GoToPointPressedThisFrame = _playerInput.actions["GoToPosition"].WasPressedThisFrame();
        MoveTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        DialogueAutoContinueHeldThisFrame = _dialogueAutoContinueAction.IsPressed();
        DialogueAutoContinuePressedThisFrame = _dialogueAutoContinueAction.WasPressedThisFrame();
        DialogueAutoContinueReleasedThisFrame = _dialogueAutoContinueAction.WasReleasedThisFrame();
        DialogueNextChoicePressedThisFrame = _dialogueNextChoice.WasPressedThisFrame();
        DialoguePrevChoicePressedThisFrame = _dialoguePrevChoice.WasPressedThisFrame();
        
        InteractButtonPressedThisFrame = _interactAction.WasPressedThisFrame();
        InteractButtonHeldThisFrame = _interactAction.IsPressed();
        DialogueContinuePressedThisFrame = InteractButtonPressedThisFrame || DialogueAutoContinuePressedThisFrame;

        PauseMenuPressedThisFrame = _menuAction.WasPressedThisFrame();
    }
}
