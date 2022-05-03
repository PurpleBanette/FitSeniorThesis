using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomLoadPref : MonoBehaviour
{
   // Command out when pushing
    [Header("GeneralSettings")]
    [SerializeField] private bool canUse = true;
    [SerializeField] private MenuController menuController;

    // will store Volume data

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null; // Voulume Slider

    // will store Brightness Data
    [Header("Brightness")]
    [SerializeField] private TMP_Text brightnessTextValue = null; // Store brigtness value for player
    [SerializeField] private Slider brightnessSlider = null; // Sore Brightness slider value

    // will store Quality Data
    [Header("Quality level Setting")]
    [SerializeField] private TMP_Dropdown qualityDropDown;

    // will store Fullscreen Data

    [Header("Fullscreen Settings")]

    [SerializeField] private Toggle fullScreenToggle;

    // will store Sensitiviy Data

    [Header("Sensitivity Settings")]
    [SerializeField]private TMP_Text controllerSenTexValue = null;
    [SerializeField] private Slider controllerSenSlider = null; // Sensivity slider

    // will store Invert - U

    [Header("Invert-Y Settings")]
    [SerializeField] private Toggle invertYToggle = null;
    
    private void Awake()
    {
        if (canUse)
        { //Change when setting up input system
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                volumeTextValue.text = localVolume.ToString("0.0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;

            }
            else
            {
                menuController.ResetButton("Audio");
            }

            if (PlayerPrefs.HasKey("masterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");
                qualityDropDown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }
            if(PlayerPrefs.HasKey ("masterFullScreen"))
            {
                int localFullscreen = PlayerPrefs.GetInt("masterFullScreen");

                if(localFullscreen == 1)
                {
                    Screen.fullScreen = true;
                    fullScreenToggle.isOn = true;
                }
                // will reset full to false as a precausion
                else
                {
                    Screen.fullScreen = false;
                    fullScreenToggle.isOn = false;
                }
            }
            
        }

        if (PlayerPrefs.HasKey("masterBrigtness"))
        {
            float localBrightness = PlayerPrefs.GetFloat("masterBrightness");
                

                brightnessTextValue.text = localBrightness.ToString("0.0");
                brightnessSlider.value = localBrightness;

                //When setting up change to post process set up

                

        }

            if (PlayerPrefs.HasKey("masterSan"))
            {
                float localSensitivity = PlayerPrefs.GetFloat("masterSan");

                controllerSenTexValue.text = localSensitivity.ToString("0");
                controllerSenSlider.value = localSensitivity;
                menuController.mainControllerSen = Mathf.RoundToInt(localSensitivity);
            }

            if (PlayerPrefs.HasKey("masterInvert-Y"))
            {
                if (PlayerPrefs.GetInt("masterInvertY") == 1)
                {
                    invertYToggle.isOn = true;
                }
                else
                {
                    invertYToggle.isOn = false;
                }
            }
    }
   
}


