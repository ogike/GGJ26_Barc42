using System;
using UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public Transform mainCameraTransform { get; private set; }
    public Camera mainCamera;

    public Sprite defaultPlayerMaskSprite;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"More than one {this} in scene!");
            return;
        }

        Instance = this;
        mainCameraTransform = mainCamera.transform;
    }

    private void Start()
    {
        DialogueUI.Instance.SetPlayerPortrait(defaultPlayerMaskSprite);
    }
}
