using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restriction : MonoBehaviour
{
    //Variable


    //Function
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        IKillable killable = collision.collider.GetComponent<IKillable>();
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
