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
    [SerializeField] private GameObject zombies;
    [SerializeField] private GameObject groundItems;
    [SerializeField] private GameObject pausemenu;
    [SerializeField] private GameObject player;
   
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
        zombies.gameObject.SetActive(false);
        groundItems.gameObject.SetActive(false);
        pausemenu.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
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
        zombies.gameObject.SetActive(true);
        groundItems.gameObject.SetActive(true);
        pausemenu.gameObject.SetActive(true);
        player.gameObject.SetActive(true);

    }


    public void Quitgame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

}
