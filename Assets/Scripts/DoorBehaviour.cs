using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [Header("Door Object")]

    [SerializeField] private Animator myDoor = null;

    [Header("Trigger Type")]

    [SerializeField] private bool openTrigger = false;
    [SerializeField] private bool closeTrigger = false;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (openTrigger)
            {
                myDoor.SetBool("DoorOpen", true);
                gameObject.SetActive(false);
            }

            else if (closeTrigger)
            {
                myDoor.SetBool("DoorOpen", false);
                gameObject.SetActive(false);
            }
        }
    }
}

