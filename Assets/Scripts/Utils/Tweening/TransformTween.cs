using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils.Tweening
{
    [Serializable]
    public class TransformTweenSetting
    {
        public bool enabled = true;
        public float scale;
        public float duration;
        public float startDelay;
        public LeanTweenType easeType;
        
        public static TransformTweenSetting DefaultShowSettings()
        {
            TransformTweenSetting setting = new TransformTweenSetting();
            setting.enabled = true;
            setting.scale = 1;
            setting.duration = 0.15f;
            setting.startDelay = 0.00f;
            setting.easeType = LeanTweenType.easeInOutQuad;
            return setting;
        }
        
        public static TransformTweenSetting DefaultHideSettings()
        {
            TransformTweenSetting setting = new TransformTweenSetting();
            setting.enabled = true;
            setting.scale = 0;
            setting.duration = 0.15f;
            setting.startDelay = 0.0f;
            setting.easeType = LeanTweenType.easeInOutQuad;
            return setting;
        }
    }
    
    public class TransformTween : MonoBehaviour
    {
        [Header("Popup")]
        public TransformTweenSetting onHideTween = TransformTweenSetting.DefaultHideSettings();
        public TransformTweenSetting onShowTween = TransformTweenSetting.DefaultShowSettings();
        public bool disableAfterHide = false;

        [Header("Move loop")] 
        public Vector3 moveAmount;
        public float moveSpeed;
        public LeanTweenType moveCurve;

        [Header("Rotate loop")]
        public Vector3 rotateAmount;
        public float rotateSpeed;
        public LeanTweenType rotateCurve;

        protected Transform _trans;
        protected GameObject _go;
        protected Vector3 _curUntweenedScale;
        protected Vector3 _curUntweenedPos;
        protected Vector3 _origUntweenedScale;

        protected bool initialized;
        protected float _minStartDelay = 0.05f;
        protected float _maxStartDelay = 0.5f;

        protected void Start()
        {
            Init();
            StartCoroutine(StartTweenCoroutine());
        }

        private IEnumerator StartTweenCoroutine()
        {
            // FIXME: this is needed,
            // because for some fucking reason LeanTween won't actually start the first tween during Start/Awake/OnEnable.
            // But if it was to be called twice, it would work
            // huh???
            float startDelay = Random.Range(_minStartDelay, _maxStartDelay);
            yield return new WaitForSeconds(startDelay);
            StartLoopingTweens();
        }

        protected virtual void Init()
        {
            if(initialized) return;
            
            _go = gameObject;
            _trans = GetComponent<Transform>();
            _curUntweenedScale = _trans.localScale;
            _origUntweenedScale = _curUntweenedScale;

            initialized = true;
        }

        public void StartLoopingTweens()
        {
            Init();
            
            if (moveAmount != Vector3.zero)
            {
                _curUntweenedPos = _trans.localPosition;
                LeanTween.value(_go, 0, moveAmount.y, moveSpeed)
                    .setEase(moveCurve)
                    .setLoopPingPong()
                    .setOnUpdate((value) =>
                    {
                        Vector3 newLocalPosition = _curUntweenedPos;
                        newLocalPosition.y += value;
                        _trans.localPosition = newLocalPosition;
                    });
            }

            if (rotateAmount != Vector3.zero)
            {
                LeanTween.rotate(_go, _trans.rotation.eulerAngles + rotateAmount, rotateSpeed)
                    .setEase(rotateCurve)
                    .setLoopPingPong();
            }
        }

        public virtual void Show()
        {
            _go.SetActive(true);
            Init();
            LTDescr showTween = Tween(onShowTween);
            if (showTween != null) 
                showTween.setOnComplete(StartLoopingTweens);
        }

        public virtual void Hide()
        {
            Init();
            LTDescr hideTween = Tween(onHideTween);
            if (hideTween != null && disableAfterHide)
                hideTween.setOnComplete(() => _go.SetActive(false));
        }

        public virtual void HideWithoutTween()
        {
            Init();
            _go.SetActive(false);
        }

        public virtual LTDescr Tween(TransformTweenSetting setting)
        {
            if (!setting.enabled) return null;

            if (_go.LeanIsTweening())
            {
                _go.LeanCancel();
            }
            
            if (setting == onShowTween)
                _curUntweenedScale = _origUntweenedScale;
            else
                _curUntweenedScale = new Vector3(setting.scale, setting.scale, setting.scale);
            
            return LeanTween.scale(_go, _curUntweenedScale, setting.duration)
                .setDelay(setting.startDelay)
                .setEase(setting.easeType)
                .setIgnoreTimeScale(false);
        }

        public void CancelTweens()
        {
            if(_go.LeanIsTweening())
                _go.LeanCancel();
        }
    }
}
