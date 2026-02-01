using System;
using System.Collections;
using System.Collections.Generic;
using Dialogue;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public Transform mainCameraTransform { get; private set; }
    public Camera mainCamera;

    public Sprite defaultPlayerMaskSprite;
    public Sprite lionMaskSprite;
    public Sprite foxMaskSprite;
    public Sprite bearMaskSprite;
    public Sprite deathSprite;

    private List<GameObject> _npcs;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"More than one {this} in scene!");
            return;
        }

        Instance = this;
        mainCameraTransform = mainCamera.transform;

        _npcs = new List<GameObject>();
        DialogueTrigger[] dialogueTriggers = FindObjectsByType<DialogueTrigger>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (DialogueTrigger trigger in dialogueTriggers)
        {
            _npcs.Add(trigger.transform.parent.gameObject);
        }
    }

    private void Start()
    {
        DialogueUI.Instance.SetPlayerPortrait(defaultPlayerMaskSprite);
    }

    public void ChangeMask(string maskName)
    {
        Sprite newMaskSprite = defaultPlayerMaskSprite;
        switch (maskName)
        {
            case "fox":
                newMaskSprite = foxMaskSprite;
                break;
            case "lion":
                newMaskSprite = lionMaskSprite;
                break;
            case "bear":
                newMaskSprite = bearMaskSprite;
                break;
            default:
                newMaskSprite = defaultPlayerMaskSprite;
                break;
        }
        DialogueUI.Instance.SetPlayerPortrait(newMaskSprite);
    }

    public void KillNpc(string npcName)
    {
        GameObject npc = _npcs.Find(elem => elem.transform.name == npcName);
        if (!npc)
        {
            Debug.LogWarning($"NPC with name {npcName} not found in scene!");
            return;
        }
        
        Debug.Log($"Killed: {npcName}");
        DialogueUI.Instance.SetNpcPortrait(deathSprite);
        npc.SetActive(false);
    }

    public void TeleportPlayer(string placeName)
    {
        GameObject place = GameObject.Find(placeName);
        if (!place)
        {
            Debug.LogWarning($"Teleport place with name  {placeName} not found in scene!");
            return;
        }
        
        PlayerController.Instance.Teleport(place.transform.position);
    }

    public void OpenCodeDoor()
    {
        CodeDoor codeDoor = FindObjectOfType<CodeDoor>();
        if (!codeDoor)
        {
            Debug.LogWarning("No CodeDoor found in scene!");
            return;
        }

        Debug.Log("Opening Code Door via GameManager");
        codeDoor.Open();
    }

    public void GoToMainMenu()
    {
        float duration = FadeToBlack.Instance.defaultFadeOutTime;
        FadeToBlack.Instance.FadeOut(duration);
        StartCoroutine(StartMenuSceneWithDelay(duration));
    }

    public IEnumerator StartMenuSceneWithDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(0);
    }
}
