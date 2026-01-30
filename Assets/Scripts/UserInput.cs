using UnityEngine;

public class UserInput : MonoBehaviour
{
    public static UserInput Instance { get; private set; }
    
    public bool PauseMenuPressedThisFrame { get; private set; }
    
    public bool DialogueContinuePressedThisFrame {get ; private set; }
    public bool DialogueAutoContinueHeldThisFrame {get ; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"More than one {this} in scene!");
            return;
        }

        Instance = this;
    }
}
