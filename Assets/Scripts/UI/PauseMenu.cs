using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        public static PauseMenu Instance { get; private set; }
        
        public bool IsPaused { get; private set; }
        public PanelUI pausePanel;
        public bool InSubMenu { get; private set; }
        public SettingsMenu settingsMenu;
        public PanelUI controlsMenu;
        public PanelUI creditsMenu;
        public PanelUI inventoryPanel;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple Pause menus in scene!");
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            Resume();
            InSubMenu = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (UserInput.Instance.PauseMenuPressedThisFrame)
            {
                if (IsPaused)
                {
                    if (InSubMenu)
                    {
                        ShowPausePanel();
                    }
                    else
                    {
                        Resume();
                    }
                }
                else          
                    Pause();
            }
        }

        public void Pause()
        {
            IsPaused = true;
            Time.timeScale = 0;
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            ShowPausePanel();
        }
        
        
        public void HidePausePanel()
        {
            pausePanel.Hide();
            inventoryPanel.Hide();
        }

        public void ShowPausePanel()
        {
            pausePanel.Show();
            inventoryPanel.Show();

            InSubMenu = false;
            settingsMenu.HideSettingsPanel();
            creditsMenu.Hide();
            controlsMenu.Hide();
        }
        
        public void ShowControls()
        {
            InSubMenu = true;
            
            settingsMenu.HideSettingsPanel();
            HidePausePanel();
            creditsMenu.Hide();
            
            controlsMenu.Show();
        }
        
        public void ShowCredits()
        {
            InSubMenu = true;
            
            settingsMenu.HideSettingsPanel();
            HidePausePanel();
            controlsMenu.Hide();
            
            creditsMenu.Show();
        }

        public void Resume()
        {
            IsPaused = false;
            InSubMenu = false;
            HidePausePanel();
            settingsMenu.HideSettingsPanel();
            creditsMenu.Hide();
            controlsMenu.Hide();
            
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
        
        public void GoToSettings()
        {
            IsPaused = true;
            HidePausePanel();
            
            InSubMenu = true;
            settingsMenu.ShowSettingsPanel();
            
            Time.timeScale = 0;
        }
        
        
        public void GoToMainMenu()
        {
            float duration = FadeToBlack.Instance.defaultFadeOutTime;
            FadeToBlack.Instance.FadeOut(duration);
            StartCoroutine(StartMenuSceneWithDelay(duration));
        }

        public IEnumerator StartMenuSceneWithDelay(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            SceneManager.LoadScene(0);
        }
    }
}
