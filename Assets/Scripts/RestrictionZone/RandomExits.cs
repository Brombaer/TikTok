using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomExits : MonoBehaviour
{
    //Imsleepy

    //Variables
    [SerializeField]
    private GameObject[] Exits;

    //Functions
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
        int randomExits = Random.Range(0, Exits.Length);
        Exits[randomExits].SetActive(true);
        }
    }
}
