using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static UserInput Instance { get; private set; }
    
    public bool PauseMenuPressedThisFrame { get; private set; }
    
    public bool DialogueContinuePressedThisFrame {get ; private set; }
    public bool DialogueAutoContinueHeldThisFrame {get ; private set; }
    
    public Vector2 MoveInput { get; private set; }
    
    private PlayerInput _playerInput;
    
    private InputAction _moveAction;


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
    }

    private void Update()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();
    }
}
