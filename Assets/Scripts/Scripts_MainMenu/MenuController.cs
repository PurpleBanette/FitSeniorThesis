using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;




public class MenuController : MonoBehaviour

{ //Command out when pushing
    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null; // Controlls Audio %
    [SerializeField] private Slider volumeSlider = null; // Voulume Slider
    [SerializeField] private float defaultVolume = 1.0f; // Voulume Slider

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text controllerSenTextValue = null; // Controlls Audio %
    [SerializeField] private Slider controllerSenSlider = null; // Sensivity slider 
    [SerializeField] private int defaultControllerSen = 4; // default sensitvity
    public int mainControllerSen = 4;

    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertYToggle = null; // Toggle Invert Y

    [Header("Graphics Settings")]
    [SerializeField] private TMP_Text brightnessTextValue = null; // Controlls Brigtness %
    [SerializeField] private Slider brightnessSlider = null; // Graphic Brigtness Slider
    [SerializeField] private float defaultBrightness = 1; // Defulte Brigness

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightessLevel;
    [Space(10)] //space of the array
    [SerializeField] private TMP_Dropdown qualityDropDown;
    [SerializeField] private Toggle fullScreenToggle;

    [Header("Confirmation Settings")]
    [SerializeField] private GameObject comfirmationPrompt = null; //Accptence prompt


    [Header("Levels To Load")]
    public string _newGameLevel; // new load from build settings
    private string levelToLoad;   //load from build settings
    private string yourLevelis;
    private string _credits;
    [SerializeField] private GameObject credits = null;
    [SerializeField] private GameObject noSavedGameDialog = null;

    [Header("Resolution Dropdowns")]
    public TMP_Dropdown resolutionDropdown; // ref drop down indvisual settings
    private Resolution[] resolutions;   //An arry to change the resolution through build settings
   
    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        //Try to add list to an update method when testing

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            //Will check if resoultion is the same as our defualt resolution
            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

      //  resolutionDropdown.AddOptions(options); // controlsss dropdown addons / options
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue(); // is whats being called
      

    }
    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


    //New Game
    public void NewGameDialogYes()
    {

      
        SceneManager.LoadScene(_newGameLevel);

        /* if (PlayerPrefs.HasKey("YB_Intro_Scene001"))
             {

             }
        */
        
    }
    //Load Game level
    /*public void LoadGameDialogYes()
    {
       
            levelToLoad = PlayerPrefs.GetString("SaveLevel");
            SceneManager.LoadScene(levelToLoad);
            PlayerPrefs.SetString("SavedLevel", yourLevelis);
        
           
        
    }*/


    //Command out when pushing
    public void RoboButton()
    {

        SceneManager.LoadScene("IntroVideo");
        //add code to returne to main
    }

    public void ObsidianButton()
    {

        SceneManager.LoadScene("BossObsidian");
        //add code to returne to main
    }
    /*
    public void GunSlingerButton()
    {

        SceneManager.LoadScene("BossGunslinger");
        //add code to returne to main
    }
    */
    //Will Quit Application
    public void CreditsButton()

    {

        SceneManager.LoadScene("Credits");
        //add cde to returne to main
    }
    public void ExitButton()

    {
        Application.Quit();
    }
    //Controls Voulme Slider [UiElmements]
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; //Conect Audio Listener
        volumeTextValue.text = volume.ToString("0.0");

    }
    //Voulme apply [UiElmements]
    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        //Will Show promp
        StartCoroutine(ConfirmationBox());
        //show prompt

    }

    //Will Control Controller Senesitivity

    public void SetControllerSen(float sensitivity)
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);
        controllerSenTextValue.text = sensitivity.ToString("0");

    }

    //Will apply Gamplay Settings
    public void GameplayApply()
    {
        if(invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertY", 1);
            //Invert Y

        }
        else
        {
            PlayerPrefs.SetInt("masterInvertY", 0);
            //No Invert Y
        }

        PlayerPrefs.SetFloat("masterSan", mainControllerSen); //Will store Sensitivity Data

        //Will pop up dialog box
        StartCoroutine(ConfirmationBox());
    }


    public void SetBrigtness(float brightness)
    {
        _brightessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
        
    }

    public void SetFullScreen(bool isFullscreen )
    {
        _isFullScreen = isFullscreen;
    }

    public void SetQuality (int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightessLevel);
        //Update when finish to access just post processing

        PlayerPrefs.SetInt("masterQuality", _qualityLevel);

        //Will change quility  though master quility setting located in project pref setting

        QualitySettings.SetQualityLevel(_qualityLevel);

        //Controls toggle for full screen

        PlayerPrefs.SetInt("masterFullscreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;

        //Will Promte the confirmation box / icon
        StartCoroutine(ConfirmationBox());
    }

    //Will Reset Volume etc....
   public void ResetButton(string MenuType)
    {
        if (MenuType == "Graphics")
        {
            //When testing have this reset to defult post prosscess value && max Res
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");

            qualityDropDown.value = 1;//default Quality value (med)
            QualitySettings.SetQualityLevel(1);
            fullScreenToggle.isOn = false;//hat obj display in our ui
            Screen.fullScreen = false; //will toggle boole to reset screen size

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length; 
            GraphicsApply();



        }
        if(MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply(); 

        }
        if (MenuType == "Gameplay" )
        {
            controllerSenTextValue.text = defaultControllerSen.ToString("0");
            controllerSenSlider.value = defaultControllerSen;
            mainControllerSen = defaultControllerSen;
            invertYToggle.isOn = false;
            GameplayApply();
        }
    }
    //Store Comfirmation promt and setting / data

    public IEnumerator ConfirmationBox()
    {
        comfirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(4f);
        comfirmationPrompt.SetActive(false);
    }
    public void LoadConfimBox()
    {
        StartCoroutine(ConfirmationBox());
    }
    
}
