using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBarricades : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "InteractableObject")
        {
            Destroy(collision.gameObject);
        }
    }
}
