using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PanelUI : UIElement
    {
        private CanvasGroup _canvasGroup;
        
        protected override void Init()
        {
            base.Init();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void Show()
        {
            _go.SetActive(true);
            base.Show();
        }

        public override bool Tween(TweeningSetting setting)
        {
            if (!base.Tween(setting)) return false;

            LeanTween.value(_go, _canvasGroup.alpha, setting.color.a, setting.switchTime)
                .setDelay(setting.startDelay)
                .setEase(setting.easeType)
                .setOnUpdate(alpha => _canvasGroup.alpha = alpha)
                .setIgnoreTimeScale(true)
                .setOnComplete(CompletePanelTween);

            return true;
        }

        private void CompletePanelTween()
        {
            if (_canvasGroup.alpha == 0)
            {
                _canvasGroup.interactable = false;
                _go.SetActive(false);
            }
            else
            {
                _canvasGroup.interactable = true;
                // Show() handles the gameobject.SetActive
            }
        }
    }
}
