using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public PanelUI mainPanel;
        
        public bool InSubMenu { get; private set; }


        private void Start()
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            ShowMainPanel();
            FadeToBlack.Instance.FadeIn(FadeToBlack.Instance.defaultFadeInTime);
        }

        public void ShowMainPanel()
        {
            InSubMenu = false;

            mainPanel.Show();
        }

        public void Play()
        {
            float duration = FadeToBlack.Instance.defaultFadeOutTime;
            FadeToBlack.Instance.FadeOut(duration);
            StartCoroutine(StartMainSceneWithDelay(duration));
        }

        public IEnumerator StartMainSceneWithDelay(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            SceneManager.LoadScene(1);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
