using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public Dropdown ResolutionDropdown;
   
    private CharacterInput _characterInput;
    
    private FMOD.Studio.Bus _master;
    private float _masterVolume = 1;
    private Slider _slider;
    private Resolution[] _resolutions;

    private void Awake()
    {
        InitializeInput();
    }

    private void Start()
    {
        pauseMenuUI.SetActive(false);
        
        _master = FMODUnity.RuntimeManager.GetBus("bus:/");
        _slider = GetComponent<Slider>();

        _resolutions = Screen.resolutions;
        ResolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    //private void Update()
    //{
    //    _master.setVolume(_masterVolume);
    //}

    private void TogglePauseMenu()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    private void InitializeInput()
    {
        _characterInput = new CharacterInput();
        _characterInput.Enable();

        _characterInput.Player.PauseMenu.performed += context => TogglePauseMenu();
    }

    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
      
    }


    //important 
    private void OnEnable()
    {
        _characterInput.Enable();
    }

    private void OnDisable()
    {
        _characterInput.Disable();
    }

    public void RestartMenu()
    {
        SceneManager.LoadScene("WorldScene");
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void SetMasterVolume(float newMasterVolume)
    {
        //_masterVolume = newMasterVolume;
        _master.setVolume(newMasterVolume);
    }

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetGameResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void QuitMenu()
    {
        Application.Quit();
    }
}
