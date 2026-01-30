using System;
using UnityEngine;

namespace UI
{
    [Serializable]
    public class TweeningSetting
    {
        public bool enabled = true;
        public float scale;
        public Color color;
        public float switchTime;
        public float startDelay;
        public LeanTweenType easeType;

        public static TweeningSetting DefaultShowSettings()
        {
            TweeningSetting setting = new TweeningSetting();
            setting.enabled = true;
            setting.scale = 1;
            setting.color = Color.white;
            setting.switchTime = 0.15f;
            setting.startDelay = 0.05f;
            setting.easeType = LeanTweenType.easeInOutQuad;
            return setting;
        }
        
        public static TweeningSetting DefaultHideSettings()
        {
            TweeningSetting setting = new TweeningSetting();
            setting.enabled = true;
            setting.scale = 0;
            setting.color = Color.white;
            setting.color.a = 0;
            setting.switchTime = 0.15f;
            setting.startDelay = 0.1f;
            setting.easeType = LeanTweenType.easeInOutQuad;
            return setting;
        }
    }
}
