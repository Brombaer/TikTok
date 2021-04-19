using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restriction : MonoBehaviour
{
    //Variable


    //Function    
    private void OnTriggerEnter(Collider other)
    {
        IKillable killable = other.GetComponent<IKillable>();
        if (killable != null)
        {
            KillZoneEnteredEffect effect = killable.Kill();

            if (effect == KillZoneEnteredEffect.Kill)
            {
                gameObject.SetActive(false);
                SceneManager.LoadScene(2);

            }
        }
    }

}
public enum KillZoneEnteredEffect
{
    None,
    Kill
}
