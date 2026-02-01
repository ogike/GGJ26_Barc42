// All of the code in this folder is copied/derived from Github user shapedbyrainstudios
// Repo: https://github.com/shapedbyrainstudios/ink-dialogue-system

using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UI;
using UnityEngine;
using Utils;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }

        public TextAsset inkJson;
        [SerializeField] private TextAsset loadGlobalsJSON;

        [Header("Params")]
        public float typingSpeed = 0.04f;
        public float autoContinueTypingSpeed = 0.005f;
        public float lineBreakTime = 0.1f;
        public float characterSpecialWaitTime = 0.3f;
        private bool _npcDialogueActive;
        
        private DialogueClass _dialogueTriggerer;


        public Story currentStory { get; private set; }
        public bool dialogueIsPlaying { get; private set; }
        public static event Action<Story> OnCreateStory; // for Editor script
        public static event Action OnDestroyStory;

        private bool _canContinueToNextLine = false;
        private bool _continueInputBuffered = false;
        private bool _autoContinue = false;
        private bool _isPausedFromInk;
        private int _currentChoiceIndex = 0;
        private bool _hasShownChoice;
        private bool _npcTalking;
        private bool _isThought;
        private bool _switchedChoiceAlready;

        private bool _movingPlayerPos;

        private Coroutine displayLineCoroutine;

        private const string PLAYER_STRING_TAG = "Player";
        private const string THOUGHT_STRING_TAG = "Thought";

        private const string PLAYER_TITLE = "You";
        private const string THOUGHT_TITLE = "Thought";

        private string _npcTalkingTag;

        private DialogueVariables dialogueVariables;

        private const float _floatingPointTolerance = 0.01f;

        public bool debugMode;

        private DialogueUI _ui;

        private void Awake() 
        {
            if (Instance != null)
            {
                Debug.LogWarning("Found more than one Dialogue Manager in the scene");
            }
            Instance = this;

            dialogueVariables = new DialogueVariables(loadGlobalsJSON);
        }

        private void Start() 
        {
            _ui = DialogueUI.Instance;
            dialogueIsPlaying = false;
            
            currentStory = new Story(inkJson.text);
            BindExternalFunctions();
            StartListeningToStoryVariable(currentStory);
            if (OnCreateStory != null) OnCreateStory(currentStory);
        }

        private void Update()
        {
            // return right away if dialogue isn't playing
            if (!dialogueIsPlaying) 
            {
                return;
            }

            _autoContinue = UserInput.Instance.DialogueAutoContinueHeldThisFrame;
            

            // handle continuing to the next line in the dialogue when submit is pressed
            if (UserInput.Instance.DialogueContinuePressedThisFrame)
            {
                if (_canContinueToNextLine)
                {
                    if (currentStory.currentChoices.Count == 0)
                        ContinueStory();
                    else
                    {
                        if (_hasShownChoice)
                            MakeChoice();
                        else
                            ShowChoicePanel();
                    }

                    //RumbleManager.Instance.AddNewImpulse(RumbleManager.Instance.dialogueProgressRumble);
                }
                else
                {
                    _continueInputBuffered = true;
                    //RumbleManager.Instance.AddNewImpulse(RumbleManager.Instance.dialogueSkipRumble);
                }
            }

            if (currentStory.currentChoices.Count > 0)
            {
                if (UserInput.Instance.DialoguePrevChoicePressedThisFrame)
                {
                    PreviousChoice();
                }
                else if (UserInput.Instance.DialogueNextChoicePressedThisFrame)
                {
                    NextChoice();
                }
            }
        }

        //TODO: extract this to its own class
        private void BindExternalFunctions()
        {
            currentStory.BindExternalFunction ("fadeOutSequence", (float fadeOut, float wait, float fadeIn) => {
                if(debugMode) Debug.Log("InkDebug: Fade out sequence for " + (fadeOut + wait + fadeIn) + " seconds...");
                
                // Call this here too incase it would fall on the next frame because of StartCoroutines
                _isPausedFromInk = true;
                StartCoroutine(PauseLines(fadeOut + wait + fadeIn));
                FadeToBlack.Instance.FadeOutSequenceStart(fadeOut, wait, fadeIn);
            });
            currentStory.BindExternalFunction ("fadeOut", (float fadeOut) => {
                if(debugMode) Debug.Log("InkDebug: Fading out for " + fadeOut + " seconds...");
                
                _isPausedFromInk = true;
                StartCoroutine(PauseLines(fadeOut));
                FadeToBlack.Instance.FadeOut(fadeOut);
            });
            currentStory.BindExternalFunction ("fadeIn", (float fadeIn) => {
                if(debugMode) Debug.Log("InkDebug: Fading in for " + fadeIn + " seconds...");
                
                _isPausedFromInk = true;
                StartCoroutine(PauseLines(fadeIn));
                FadeToBlack.Instance.FadeIn(fadeIn);
            });
            
            currentStory.BindExternalFunction ("wait", (float time) => {
                if(debugMode) Debug.Log("InkDebug: Waiting for " + time + " seconds...");
                StartCoroutine(PauseLines(time));
            });
            
            
            currentStory.BindExternalFunction ("killNpc", (string npcName) => {
                if(debugMode) Debug.Log($"InkDebug: Trying to kill {npcName}...");
                GameManager.Instance.KillNpc(npcName);
            });
            
            currentStory.BindExternalFunction ("teleportPlayer", (string placeName) => {
                if(debugMode) Debug.Log($"InkDebug: Teleporting player to {placeName}...");
                GameManager.Instance.TeleportPlayer(placeName);
            });

            currentStory.BindExternalFunction ("openDoor", () => {
                if(debugMode) Debug.Log($"InkDebug: Opening door...");
                GameManager.Instance.OpenCodeDoor();
            });
        }
        
        public void EnterDialogueMode(DialogueClass triggerer) 
        {
            _dialogueTriggerer = triggerer;
            
            _npcDialogueActive = !triggerer.monologue;
            if(_npcDialogueActive)
                _npcTalkingTag = triggerer.talkingTag;
            
            dialogueIsPlaying = true;
            _hasShownChoice = false;
            _canContinueToNextLine = false;
            _isPausedFromInk = false;
            
            currentStory.ChoosePathString(triggerer.inkPath);
            
            StartCoroutine(StartStory());
        }
        
        private IEnumerator StartStory()
        {
            yield return new WaitForSeconds(0);

            ContinueStory();
        }

        private IEnumerator ExitDialogueMode() 
        {
            yield return new WaitForSeconds(0.2f);

            StopListeningToStoryVariable(currentStory);

            dialogueIsPlaying = false;
            _ui.HideDialogueBoxes();

            //TODO: camera
            // CameraFollow.Instance.SetZoomNormal();

            if (_dialogueTriggerer != null)
            {
                _dialogueTriggerer.Stop();
                _dialogueTriggerer = null;
            }
        }

        private void ContinueStory() 
        {
            if (_isPausedFromInk)
            {
                // Unpause should call ReContinueStory when we are ready, dont do anything until then
                if(debugMode) Debug.Log("Ink: Tried to continued story while Ink is paused, skipping...");
                return;
            }
            
            if (currentStory.canContinue) 
            {
                // set text for the current dialogue line
                if (displayLineCoroutine != null) 
                {
                    StopCoroutine(displayLineCoroutine);
                }

                string currentLine = currentStory.Continue();
                
                if (_isPausedFromInk)
                {
                    // Unpause should call ReContinueStory when we are ready, dont do anything until then
                    if(debugMode) Debug.Log("Ink: Tried to continued story while Ink is paused, skipping...");
                    return;
                }
                
                string lineToDisplay = ParseLine(currentLine);
                HandleTags(currentStory.currentTags);
                
                if(debugMode) Debug.Log("InkDebug: Continued story, ispaused state: " + _isPausedFromInk + ", next line: \n" + lineToDisplay);
                
                displayLineCoroutine = StartCoroutine(DisplayLine(lineToDisplay));
            }
            else if (currentStory.currentChoices.Count > 0)
            {
                ShowChoicePanel();
            }
            else 
            {
                if(debugMode) Debug.Log("InkDebug: Cant continue story, exiting");
                StartCoroutine(ExitDialogueMode());
            }
        }

        private void ReContinueStory()
        {
            string lineToDisplay = ParseLine(currentStory.currentText);
            HandleTags(currentStory.currentTags);
                
            if(debugMode) Debug.Log("InkDebug: Recontinued story, ispaused state: " + _isPausedFromInk + ", next line: \n" + lineToDisplay);
                
            displayLineCoroutine = StartCoroutine(DisplayLine(lineToDisplay));
        }
        
        public void StartListeningToStoryVariable(Story story)
        {
            dialogueVariables.StartListening(story);
        }
        
        public void StopListeningToStoryVariable(Story story)
        {
            dialogueVariables.StopListening(story);
        }

        //TODO: seperate this into the two bubbles
        private IEnumerator DisplayLine(string line)
        {
            if (line.Trim().Length == 0)
            {
                ContinueStory();
                yield break;
            }
            
            _canContinueToNextLine = false;
            _continueInputBuffered = false;
            bool isAddingRichTextTag = false;
            
            if(_npcTalking) _ui.LoadLineNpc(line);
            else           _ui.LoadLinePlayer(line, _isThought);

            // wait to reset frame input
            yield return new WaitForSeconds(0);

            int visibleLetters = 0;
            string currentRichText = "";
            
            // display each letter one at a time
            foreach (char letter in line.ToCharArray())
            {
                
                // if the submit button is pressed, finish up displaying the line right away
                if (_continueInputBuffered) 
                {
                    if(debugMode) Debug.Log("InkDebug: Skipping this line.");
                    
                    _ui.SetTextVisibleCharacters(line.Length);

                    break;
                }

                // check for rich text tag, if found, add it without waiting
                if (letter == '<')
                {
                    currentRichText = "";
                    isAddingRichTextTag = true;
                }
                else if(isAddingRichTextTag)
                {
                    if (letter == '>')
                    {
                        isAddingRichTextTag = false;
                        if (currentRichText == "br")
                        {
                            float waitTime = lineBreakTime;
                            if (_autoContinue) waitTime *= (autoContinueTypingSpeed / typingSpeed);
                            yield return new WaitForSeconds(waitTime);
                        }
                    }
                    else
                    {
                        currentRichText += letter;
                    }
                }
                
                // if not (or not anymore) add the next letter and wait a small time
                if(!isAddingRichTextTag)
                {
                    visibleLetters++;
                    _ui.SetTextVisibleCharacters(visibleLetters);

                    float curTypingSpeed = typingSpeed;
                    if (letter == '.')
                    {
                        curTypingSpeed = characterSpecialWaitTime * (autoContinueTypingSpeed / typingSpeed);
                    }
                    if (_autoContinue) curTypingSpeed *= (autoContinueTypingSpeed / typingSpeed);

                    yield return new WaitForSeconds(curTypingSpeed);
                }
            }

            _canContinueToNextLine = true;
            _continueInputBuffered = false;
            if (!_autoContinue)
            {
                // actions to take after the entire line has finished displaying
                _ui.ShowContinueIcon();

            }
            else
            {
                ContinueStory();
            }
        }

        //Currently only handles the speaking tag splitting
        private string ParseLine(string line)
        {
            string ret = line.Trim();
            string[] parts = ret.Split(':');
            if (parts.Length == 1)
            {
                //Leave the speaking tag as is, holds the same value as last line
            }
            else if (parts.Length >= 2)
            {
                string characterName = parts[0].Trim();
                string character_title = characterName;
                if (characterName is PLAYER_STRING_TAG)
                {
                    _npcTalking = false;
                    _isThought = false;
                    character_title = PLAYER_TITLE;
                }
                else if (characterName is THOUGHT_STRING_TAG)
                {
                    _npcTalking = false;
                    _isThought = true;
                    character_title = THOUGHT_TITLE;
                }
                else if (_npcDialogueActive && characterName == _npcTalkingTag)
                {
                    _npcTalking = true;
                }
                else
                {
                    Debug.Log($"Unable to handle character name: {characterName}");
                    _npcTalking = true; // default to NPC, probably speakingTag is not updated with a new NPC name
                }
                _ui.SetTalkingTitle(character_title);

                return parts[1].Trim();
            }
            // else
                // return "Too many ':' characters in line: " + ret;
            
            return ret;
        }


        private void HandleTags(List<string> currentTags)
        {
            // loop through each tag and handle it accordingly
            foreach (string tag in currentTags) 
            {
                switch (tag.Trim())
                {
                    default:
                        Debug.LogWarning("Tag could not be appropriately parsed: " + tag);
                        break;
                }
            }
        }
        
        public void MakeChoice()
        {
            if (_canContinueToNextLine) 
            {
                if(debugMode) Debug.Log("InkDebug: Chosen choice index: " + _currentChoiceIndex 
                                                + "\ntext: " + currentStory.currentChoices[_currentChoiceIndex].text);
                currentStory.ChooseChoiceIndex(_currentChoiceIndex);
                ContinueStory();
                _hasShownChoice = false;
            }
        }

        private void ShowChoicePanel()
        {
            _hasShownChoice = true;
            _canContinueToNextLine = true;
            _npcTalking = false;
            
            _currentChoiceIndex = 0;
            List<string> choiceTexts = new List<string>();
            currentStory.currentChoices.ForEach(choice => choiceTexts.Add(choice.text));
            _ui.ShowChoicesPanel(choiceTexts, _currentChoiceIndex);
            _ui.SetTalkingTitle(PLAYER_TITLE);
        }

        public void NextChoice()
        {
            if(currentStory.currentChoices.Count == 0) return;
            
            int maxChoiceIndex = Math.Min(currentStory.currentChoices.Count-1, 4);
            
            _currentChoiceIndex++;
            if (_currentChoiceIndex > maxChoiceIndex)
                _currentChoiceIndex = 0;
            
            if (debugMode) Debug.Log("InkDebug: Switching to choice " + _currentChoiceIndex 
                                     + ": " + currentStory.currentChoices[_currentChoiceIndex].text);

            
            _ui.ChangeChoice(_currentChoiceIndex);
        }

        public void PreviousChoice()
        {
            if(currentStory.currentChoices.Count == 0) return;
            
            int maxChoiceIndex = Math.Min(currentStory.currentChoices.Count-1, 4);

            _currentChoiceIndex--;
            if (_currentChoiceIndex < 0)
                _currentChoiceIndex = maxChoiceIndex;
            
            if (debugMode) Debug.Log("InkDebug: Switching to choice " + _currentChoiceIndex 
                                     + ": " + currentStory.currentChoices[_currentChoiceIndex].text);


            _ui.ChangeChoice(_currentChoiceIndex);
        }

        public Ink.Runtime.Object GetVariableState(string variableName) 
        {
            Ink.Runtime.Object variableValue = null;
            dialogueVariables.variables.TryGetValue(variableName, out variableValue);
            if (variableValue == null) 
            {
                Debug.LogWarning("Ink Variable was found to be null: " + variableName);
            }
            return variableValue;
        }

        private IEnumerator PauseLines(float seconds)
        {
            if(debugMode) Debug.Log("Ink: pausing lines");
            _ui.HideDialogueBoxes();
            _isPausedFromInk = true;
            _canContinueToNextLine = false;
            
            yield return new WaitForSeconds(seconds);
            
            if(debugMode) Debug.Log("Ink: continuing lines");
            _isPausedFromInk = false;
            _canContinueToNextLine = true;
            ReContinueStory();
        }

        public void SetVariableBool(string var, bool val)
        {
            if(debugMode) Debug.Log($"Value {var} before: " + currentStory.variablesState[var]);
            currentStory.variablesState[var] = val;
            // currentStory.variablesState.SetGlobal(var, (Ink.Runtime.BoolValue);
            if(debugMode) Debug.Log($"Value {var} after: " + currentStory.variablesState[var]);
        }

        public void SetVariableInt(string var, int val)
        {
            if(debugMode) Debug.Log($"Value {var} before: " + currentStory.variablesState[var]);
            currentStory.variablesState[var] = val;
            // currentStory.variablesState.SetGlobal(var, (Ink.Runtime.BoolValue);
            if(debugMode) Debug.Log($"Value {var} after: " + currentStory.variablesState[var]);
        }

        // This method will get called anytime the application exits.
        // Depending on your game, you may want to save variable state in other places.
        public void OnApplicationQuit() 
        {
            StopListeningToStoryVariable(currentStory);
            dialogueVariables.SaveVariables();
            if (OnDestroyStory != null) OnDestroyStory();
        }

    }
}
