using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Playgame()
    {
  
        SceneManager.LoadScene(1);
    }

    public void Quitgame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
