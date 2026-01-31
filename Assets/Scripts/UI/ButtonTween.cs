using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{

    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class ButtonTween : UIElement, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private Button _button;

        public TweeningSetting onEnterTween;
        public TweeningSetting onExitTween;
        public TweeningSetting onClickTween;

        protected void Start()
        {
            Tween(onExitTween);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Tween(onEnterTween);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tween(onExitTween);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Tween(onClickTween);
        }
    }
}
