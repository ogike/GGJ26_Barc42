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
        
        public SettingsMenu settingsMenu;
        public PanelUI controlsMenu;
        public PanelUI creditsMenu;
        
        
        private void Start()
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            ShowMainPanel();
            FadeToBlack.Instance.FadeIn(FadeToBlack.Instance.defaultFadeInTime);
        }
        
        void Update()
        {
            if (UserInput.Instance.PauseMenuPressedThisFrame)
            {
   
                if (InSubMenu)
                {
                    ShowMainPanel();
                }
            }
        }

        public void ShowSettings()
        {
            InSubMenu = true;
            settingsMenu.ShowSettingsPanel();
            
            mainPanel.Hide();
            creditsMenu.Hide();
            controlsMenu.Hide();
        }

        public void ShowControls()
        {
            InSubMenu = true;
            
            settingsMenu.HideSettingsPanel();
            mainPanel.Hide();
            creditsMenu.Hide();
            
            controlsMenu.Show();
        }
        
        public void ShowCredits()
        {
            InSubMenu = true;
            
            settingsMenu.HideSettingsPanel();
            mainPanel.Hide();
            controlsMenu.Hide();
            
            creditsMenu.Show();
        }

        public void ShowMainPanel()
        {
            InSubMenu = false;
            settingsMenu.HideSettingsPanel();
            controlsMenu.Hide();
            creditsMenu.Hide();
            
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
