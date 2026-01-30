using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils.Tweening;

namespace Dialogue
{
    /// <summary>
    /// Holds everything needed for a dialogue from NPC side
    /// </summary>
    public class DialogueTrigger : MonoBehaviour
    {
        public DialogueClass dialogue;
        
        [Header("UI")]
        public TransformTween visualCue;

        private bool _visualCueShown;
        private bool _playerInRange;

        private void Awake() 
        {
            _playerInRange = false;
            _visualCueShown = false;
            if(visualCue)
                visualCue.HideWithoutTween();
            dialogue.InitializeFromNpc(this);

            // animator.SetBool("isTalking", false);
        }

        private void Update() 
        {
            if (_playerInRange && !DialogueManager.Instance.dialogueIsPlaying) 
            {
                if (!_visualCueShown &&visualCue)
                {
                    _visualCueShown = true;
                    visualCue.Show();
                }
                if (UserInput.Instance.InteractButtonPressedThisFrame)
                {
                    EnterDialogue();
                }
            }
            else if(_visualCueShown && visualCue) 
            {
                visualCue.Hide();
                _visualCueShown = false;
            }
        }

        public void EnterDialogue()
        {
            dialogue.Start();
            // animator.SetBool("isTalking", true);
            //TODO: audio
            // SfxManager.Instance.PlayAudio(SfxManager.Instance.interactSound);
            // RumbleManager.Instance.AddNewImpulse(RumbleManager.Instance.dialogueProgressRumble);
        }

        public void ExitDialogue()
        {
            //None
        }
        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                _playerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collider) 
        {
            if (collider.gameObject.tag == "Player")
            {
                _playerInRange = false;
            }
        }
    }
}