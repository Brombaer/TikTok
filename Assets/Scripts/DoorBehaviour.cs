using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
   

    [SerializeField] private Animator myDoor = null;
    [SerializeField] private bool openTrigger = false;

    [SerializeField] private string doorOpen = "DoorAnmiation";



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (openTrigger)
            {
                myDoor.SetBool("DoorAnimation", true);
                gameObject.SetActive(false);
            }

            
        }
    }
}

