using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UIElement : MonoBehaviour

    {
        public TweeningSetting onHideTween = TweeningSetting.DefaultHideSettings();
        public TweeningSetting onShowTween = TweeningSetting.DefaultShowSettings();
        
        protected RectTransform _trans;
        protected GameObject _go;
        protected Image _image;
        protected Vector3 _curUntweenedScale;

        protected bool initialized;

        protected void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            if(initialized) return;
            
            _go = gameObject;
            _image = GetComponent<Image>();
            _trans = GetComponent<RectTransform>();
            _curUntweenedScale = _trans.localScale;

            initialized = true;
        }

        public virtual void Show()
        {
            Init();
            Tween(onShowTween);
        }

        public virtual void Hide()
        {
            Init();
            Tween(onHideTween);
        }

        public virtual bool Tween(TweeningSetting setting)
        {
            if (!setting.enabled) return false;

            if (_go.LeanIsTweening())
                _go.LeanCancel();

            _curUntweenedScale = new Vector3(setting.scale, setting.scale, setting.scale);
            LeanTween.scale(_go, _curUntweenedScale, setting.switchTime)
                .setDelay(setting.startDelay)
                .setEase(setting.easeType)
                .setIgnoreTimeScale(true);

            if (_image)
                LeanTween.value(_go, _image.color, setting.color, setting.switchTime)
                    .setDelay(setting.startDelay)
                    .setEase(setting.easeType)
                    .setOnUpdate(color => _image.color = color)
                    .setIgnoreTimeScale(true);

            return true;
        }
    }
}
