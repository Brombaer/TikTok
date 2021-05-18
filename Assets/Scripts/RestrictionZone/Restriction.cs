using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restriction : MonoBehaviour
{
    //Variable
    //private bool[] _isFull;
    //private GameObject[] _slots;




    //Function
    //Check if have item
    private void checkItem()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        IKillable killable = other.GetComponent<IKillable>();
        if (killable != null)
        {

            KillZoneEnteredEffect effect = killable.Kill();
            if (effect == KillZoneEnteredEffect.Kill)
            {
                gameObject.SetActive(false);
                SceneManager.LoadScene(1);

            }
            if (effect == KillZoneEnteredEffect.Escape )
            {
                gameObject.SetActive(true);
                SceneManager.LoadScene(2);
            }
        }
    }


}
public enum KillZoneEnteredEffect
{
    None,
    Kill,
    Warn,
    Block,
    Escape
}
