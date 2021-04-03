using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    //public MouseItem MouseItem = new MouseItem();

    public InventoryObject Inventory;
    public InventoryObject Equipment;

    private CharacterInput _characterInput;

    private void Awake()
    {
        InitializeInput();
    }

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<GroundItem>();

        if (item)
        {
            Item _item = new Item(item.Item);

            if (Inventory.AddItem(_item, 1))
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void InitializeInput()
    {
        _characterInput = new CharacterInput();

        _characterInput.Player.Save.performed += context => SaveInventory();
        _characterInput.Player.Load.performed += context => LoadInventory();
    }

    private void SaveInventory()
    {
        Inventory.Save();
        Equipment.Save();
    }

    private void LoadInventory()
    {
        Inventory.Load();
        Equipment.Load();
    }

    private void OnEnable()
    {
        _characterInput.Enable();
    }

    private void OnDisable()
    {
        _characterInput.Disable();
    }

    private void OnApplicationQuit()
    {
        Inventory.Container.Clear();
        Equipment.Container.Clear();
    }
}
