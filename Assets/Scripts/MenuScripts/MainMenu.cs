using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuObject;
    [SerializeField] private CinemachineVirtualCamera cinemachine;
    [SerializeField] private Camera camera1;
    [SerializeField] private Camera camera2;
    [SerializeField] private GameObject minimap;
    [SerializeField] private GameObject healthbar;

    private void OnEnable()
    {
        camera1.enabled = true;
        camera2.enabled = false;

        CharacterMovement.IsEnabled = false;
        cinemachine.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        minimap.gameObject.SetActive(false);
        healthbar.gameObject.SetActive(false);
    }

    public void Playgame()
    {
        cinemachine.enabled = true;
        CharacterMovement.IsEnabled = true;
        menuObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        camera1.enabled = false;
        camera2.enabled = true;
        minimap.gameObject.SetActive(true);
        healthbar.gameObject.SetActive(true);

    }

    public void Quitgame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

}
