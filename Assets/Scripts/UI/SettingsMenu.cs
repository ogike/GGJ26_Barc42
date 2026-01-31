using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    // should be type PanelUI
    public class SettingsMenu : MonoBehaviour
    {
        public static SettingsMenu Instance { get; private set; }
            
        public UIElement settingsPanel;

        public TMP_Dropdown resolutionDropdown;
        private Resolution[] _resolutions;
        public TMP_Dropdown qualityDropdown;

        public Toggle fullscreenToggle;
    
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple Settings menus in scene!");
                return;
            }
    
            Instance = this;
        }

        private void Start()
        {
            fullscreenToggle.isOn = Screen.fullScreen;
            qualityDropdown.value = QualitySettings.GetQualityLevel();
            SetupResolutionsDropdown();
        }
        
        private void SetupResolutionsDropdown()
        {
            _resolutions = Screen.resolutions;
            
            resolutionDropdown.ClearOptions();
            List<string> resolutionOptions = new List<string>();

            int index = 0; //bleh
            int curResolutionIndex = 0;
            foreach (Resolution resolution in _resolutions)
            {
                resolutionOptions.Add($"{resolution.width} X {resolution.height}");
                if (resolution.width == Screen.width && resolution.height == Screen.height)
                {
                    curResolutionIndex = index;
                }

                index++;
            }
            resolutionDropdown.AddOptions(resolutionOptions);
            resolutionDropdown.value = curResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void SetResolution(int index)
        {
            Resolution resolution = _resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetQualityLevel(int level)
        {
            QualitySettings.SetQualityLevel(level);
        }

        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }

        public void HideSettingsPanel()
        {
            settingsPanel.Hide();
        }

        public void ShowSettingsPanel()
        {
            settingsPanel.Show();
        }
    }
}
