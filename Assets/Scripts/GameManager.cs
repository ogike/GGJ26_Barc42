using System;
using System.Collections.Generic;
using Dialogue;
using UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public Transform mainCameraTransform { get; private set; }
    public Camera mainCamera;

    public Sprite defaultPlayerMaskSprite;

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

    public void KillNpc(string npcName)
    {
        GameObject npc = _npcs.Find(elem => elem.transform.name == npcName);
        if (!npc)
        {
            Debug.LogWarning($"NPC with name {npcName} not found in scene!");
            return;
        }
        
        Debug.Log($"Killed: {npcName}");
        npc.SetActive(false);
    }
}
