using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public InventoryObject Inventory;

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
            Inventory.AddItem(new Item(item.Item), 1);
            Destroy(other.gameObject);
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
    }

    private void LoadInventory()
    {
        Inventory.Load();
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
        Inventory.Container.Items.Clear();
    }
}
