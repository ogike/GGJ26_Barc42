using System.Collections;
using TMPro;
using UI;
//using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dialogue
{
    [System.Serializable]
    public class DialogueClass
    {
        public string inkPath;

        public bool monologue = false;
        //public CinemachineCamera talkingCam;

        [Header("NPC specific, fill when monologue is false")]
        public Transform speechBubblePosition;
        
        public string talkingTag;
        public Transform playerTalkingPosition;
        [FormerlySerializedAs("npcPosition")] public Transform npcTransform;

        // private DialogueTrigger _npcTrigger;
        //
        // public void InitializeFromNpc(DialogueTrigger trigger)
        // {
        //     if (!monologue)
        //     {
        //         _npcTrigger = trigger;
        //     }
        // }

        public void Start()
        {
            DialogueManager.Instance.EnterDialogueMode(this);
        }
        
        public void Stop()
        {
        }
    }
}
