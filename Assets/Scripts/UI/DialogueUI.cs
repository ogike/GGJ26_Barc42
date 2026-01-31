using System;
using System.Collections.Generic;
using System.Linq;
using Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class DialogueUI : MonoBehaviour
    {
        
        [Serializable]
        public class DialogueBoxVariant
        {
            public RectTransform panelImg;
            public TextMeshProUGUI textField;
            
            [HideInInspector] public Image image;
            [HideInInspector] public GameObject go;

            private TweeningSetting _tweeningSetting;
            private Vector3 _curUntweenedScale;

            public void SetCachedValues(TweeningSetting tweeningSetting)
            {
                image = panelImg.GetComponent<Image>();
                go = panelImg.gameObject;

                _tweeningSetting = tweeningSetting;
                _curUntweenedScale = panelImg.localScale;
            }

            public void ShowText(string text)
            {
                panelImg.gameObject.SetActive(true);
            
                textField.text = text;
                textField.maxVisibleCharacters = 0;

                if(go.LeanIsTweening())
                    go.LeanCancel();
                
                panelImg.localScale = new Vector3(_curUntweenedScale.x, 0, _curUntweenedScale.z);
                
                LeanTween.scale(panelImg, _curUntweenedScale, _tweeningSetting.boxAppearTime).setEase(_tweeningSetting.easeType);
            }

            public void ShowAllText()
            {
                this.SetVisibleCharacterCount(99);
            }

            public void SetVisibleCharacterCount(int count)
            {
                textField.maxVisibleCharacters = count;
            }

            public void Hide()
            {
                panelImg.gameObject.SetActive(false);
                textField.text = "";
            }

            public void ApplyStyle(DialogueBoxStyle style, TweeningSetting tweening)
            {
                if(go.LeanIsTweening())
                    go.LeanCancel();
                
                _curUntweenedScale = new Vector3(style.scale, style.scale, style.scale);
                LeanTween.scale(panelImg, _curUntweenedScale, tweening.choiceSwitchTime).setEase(tweening.easeType);
                
                LeanTween.value(go, image.color, style.color, tweening.choiceSwitchTime).setEaseInOutQuad().setOnUpdate(color => image.color = color);
            }
        }

        [Serializable]
        public class DialogueBoxStyle
        {
            public float scale;
            public Color color;
        }

        [Serializable]
        public class TweeningSetting
        {
            [FormerlySerializedAs("time")] public float choiceSwitchTime;
            public float boxAppearTime;
            public LeanTweenType easeType;
        }
        
        public static DialogueUI Instance { get; private set; }

        [Header("Normal Dialogue UI")] 
        [SerializeField] private GameObject dialoguePanel;
        
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private TextMeshProUGUI talkingTitleText;
        [SerializeField] private Image npcPortrait; //TODO: replace with custom PanelUI
        [SerializeField] private Image playerPortrait;
        [SerializeField] private PanelUI continueIcon;


        [Header("Player Choice Dialogue UI")]
        [SerializeField] private GameObject nonChoicePanel;
        [SerializeField] private List<DialogueBoxVariant> playerChoiceBoxes;
        private int _curChoice;

        public DialogueBoxStyle choiceStyleSelected;
        public DialogueBoxStyle choiceStyleNotSelected;

        [Header("Tweening")]
        public TweeningSetting tweening;

        [Header("Others")]
        private string _curInteractBlurb;
        private DialogueManager _manager;

        private int _maxChoicesThatFit = 3;
        public ScrollRect scrollRect;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Found more than one DialogueUI in the scene");
            }
            Instance = this;
            playerChoiceBoxes.ForEach((box) =>
            {
                box.SetCachedValues(tweening);
                box.ApplyStyle(choiceStyleNotSelected, tweening);
            });
        }

        private void Start()
        {
            _manager = DialogueManager.Instance;

            HideDialogueBoxes();
        }


        public void HideDialogueBoxes()
        {
            playerChoiceBoxes.ForEach(box => box.Hide());
            dialoguePanel.SetActive(false);
        }
        
        public void ShowContinueIcon()
        {
            continueIcon.Show();            
        }

        public void HideContinueIcon()
        {
            continueIcon.Hide();
        }

        public void SetTextVisibleCharacters(int num)
        {
            dialogueText.maxVisibleCharacters = num;
        }

        public void SetTalkingTitle(string title)
        {
            talkingTitleText.text = title;
        }

        public void LoadLinePlayer(string text, bool thought)
        {
            //TODO: select player portrait
            
            HideContinueIcon();
            
            if (thought)
            {
                //TODO: special case
            }
            
            //else
            {
                //hide other items while text is displaying
                playerChoiceBoxes.ForEach(box => box.Hide());
            }

            dialoguePanel.SetActive(true);
            nonChoicePanel.SetActive(true);
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.text = text;
        }
        
        public void LoadLineNpc(string text)
        {
            //TODO: select npc portrait
            
            HideContinueIcon();
            playerChoiceBoxes.ForEach(box => box.Hide());
            dialoguePanel.SetActive(true);
            nonChoicePanel.SetActive(true);

            
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.text = text;
        }

        public void ShowChoicesPanel(List<string> choiceTexts, int selectedChoice)
        {
            nonChoicePanel.SetActive(false);

            int maxCount = playerChoiceBoxes.Count;
            if (choiceTexts.Count > maxCount)
            {
                Debug.LogWarning($"More than {maxCount} choices received by UI, only 3 will be displayed.");
                choiceTexts.RemoveRange(maxCount, choiceTexts.Count - maxCount);
            }

            _curChoice = selectedChoice;
            for (int i = 0; i < choiceTexts.Count; i++)
            {
                playerChoiceBoxes[i].ShowText(choiceTexts[i]);
                playerChoiceBoxes[i].ShowAllText();
                DialogueBoxStyle style = (i == _curChoice) ? choiceStyleSelected : choiceStyleNotSelected;
                playerChoiceBoxes[i].ApplyStyle(style, tweening);
            }

            for (int j = choiceTexts.Count; j < playerChoiceBoxes.Count; j++)
            {
                playerChoiceBoxes[j].Hide();
            }
        }

        public void ChangeChoice(int selectedChoice)
        {
            playerChoiceBoxes[_curChoice].ApplyStyle(choiceStyleNotSelected, tweening);
            _curChoice = selectedChoice;
            playerChoiceBoxes[selectedChoice].ApplyStyle(choiceStyleSelected, tweening);
            
            if (playerChoiceBoxes.Count > _maxChoicesThatFit)
            {
                scrollRect.verticalNormalizedPosition = 1 - (selectedChoice)/((float)(playerChoiceBoxes.Count) - 1.0f);
            }
        }

        public void SetPlayerPortrait(Sprite img)
        {
            playerPortrait.sprite = img;
        }

        public void SetNpcPortrait(Sprite img)
        {
            npcPortrait.sprite = img;
        }

        public void ShowInteractionBlurb(string line)
        {
            if(DialogueManager.Instance.dialogueIsPlaying) return;

            _curInteractBlurb = line;
            LoadLinePlayer(line, true);
            dialogueText.maxVisibleCharacters = line.Length;
        }

        public void HideInteractionBlurb(string line)
        {
            if(DialogueManager.Instance.dialogueIsPlaying) return;
            
            if(line != _curInteractBlurb) return;
            _curInteractBlurb = null;
            HideDialogueBoxes();
        }

    }
}
