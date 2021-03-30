using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public InventoryObject Inventory;

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();

        if (item)
        {
            Inventory.AddItem(item.ItemObj, 1);
            Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        Inventory.Container.Clear();
    }
}
