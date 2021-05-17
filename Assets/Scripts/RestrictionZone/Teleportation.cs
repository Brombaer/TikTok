using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    //Variables
    [SerializeField]
    private Transform _teleportPosition; //Player will teleport to this location

    //Functions
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.position = _teleportPosition.position;
            other.transform.rotation = _teleportPosition.rotation;
        }
    }
}
