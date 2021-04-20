using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    private CharacterInput _characterInput;

    private void Awake()
    {
        InitializeInput();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        if (GameIsPaused)
    //        {
    //            Resume();
    //        }
    //
    //        else
    //        {
    //            Pause();
    //        }
    //    }
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

        _characterInput.Player.Escape.performed += context => TogglePauseMenu();
    }

    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

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
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitMenu()
    {
        Application.Quit();
    }
}
