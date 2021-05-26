using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseScene : MonoBehaviour
{


    public void Awake()
    {

        UnlockMouse();
        

    }
  
    public void Quitgame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void Restart()
    {
        SceneManager.LoadScene("WorldScene");
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


    }
    void UnlockMouse()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}

