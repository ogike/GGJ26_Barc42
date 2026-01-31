using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class FadeToBlack : MonoBehaviour
    {
        public static FadeToBlack Instance;
        
        public Image fadeToBlackScreen;

        public float defaultFadeInTime;
        public float defaultFadeOutTime;
        public float fullBlackTime;

        public AnimationCurve fadeInCurve;
        public AnimationCurve fadeOutCurve;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"Multiple {this} in the scene!");
                return;
            }

            Instance = this;
        }

        public void Start()
        {
            FadeIn(defaultFadeInTime);
        }

        public void SetFadeToBlackColor(float opacity)
        {
            Color color = fadeToBlackScreen.color;
            color.a = opacity;
            fadeToBlackScreen.color = color;
        }
    
        public void FadeIn(float duration)
        {
            SetFadeToBlackColor(1);
            StartCoroutine(FadeInCoroutine(duration));
        }
    
        public IEnumerator FadeInCoroutine(float duration)
        {
            float time = 0.0f;
            SetFadeToBlackColor(1);
            while (time <= duration)
            {
                float opacity = fadeInCurve.Evaluate(time / duration);
                SetFadeToBlackColor(opacity);
                time += Time.unscaledDeltaTime;
                yield return new WaitForSeconds(0);
            }
            SetFadeToBlackColor(0);
        }
        
        public void FadeOut(float duration)
        {
            StartCoroutine(FadeOutCoroutine(duration));
        }
        
        public IEnumerator FadeOutCoroutine(float duration)
        {
            float time = 0;
            SetFadeToBlackColor(0);
            while (time <= duration)
            {
                float opacity = fadeOutCurve.Evaluate(time / duration);
                SetFadeToBlackColor(opacity);
                time += Time.unscaledDeltaTime;
                yield return new WaitForSeconds(0);
            }
            SetFadeToBlackColor(1);
        }
    
        public void FadeOutSequenceStart(float fadeOut, float wait, float fadeIn)
        {
            StartCoroutine(FadeOutSequence(fadeOut, wait, fadeIn));
        }
        
        public IEnumerator FadeOutSequence(float fadeOut, float wait, float fadeIn)
        {
            FadeOut(fadeOut);
            yield return new WaitForSeconds(fadeOut + wait);
            FadeIn(fadeIn);
        }
    }
}
