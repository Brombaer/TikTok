using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restriction : MonoBehaviour
{
    //Variable
    [SerializeField]
    private ItemInfo[] _itemsToEscape;


    //Function
    private void OnTriggerEnter(Collider other)
    {
        IKillable killable = other.GetComponent<IKillable>();
        if (killable != null)
        {

            KillZoneEnteredEffect effect = killable.Effect();
            if (effect == KillZoneEnteredEffect.Kill)
            {
                gameObject.SetActive(false);
                SceneManager.LoadScene(1);

            }
            if (effect == KillZoneEnteredEffect.Escape)
            {
                //gameObject.SetActive(true);
                //SceneManager.LoadScene(2);
                CheckSyringe();
            }
        }
    }

    private void CheckSyringe()
    {
        InventoryObject inventoryObject = GetComponent<InventoryObject>();
        for (int i = 0; i < _itemsToEscape.Length; i++)
        {

            if (inventoryObject.FindItemOnInventory(_itemsToEscape[i]) != null)
            {
                gameObject.SetActive(true);
                SceneManager.LoadScene(2);
                
            }
            else
            {
                Debug.Log("You need a syringe");
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
