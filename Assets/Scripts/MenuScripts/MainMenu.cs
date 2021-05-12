using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuObject;
    [SerializeField] private CinemachineVirtualCamera cinemachine;

    private void OnEnable()
    {

        CharacterMovement.IsEnabled = false;
        cinemachine.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Playgame()
    {
        cinemachine.enabled = true;
        CharacterMovement.IsEnabled = true;
        menuObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
