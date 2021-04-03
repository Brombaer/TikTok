using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public InventoryObject Inventory;
    public InventoryObject Equipment;

    public Attribute[] Attributes;

    private CharacterInput _characterInput;

    private void Awake()
    {
        InitializeInput();
    }

    private void Start()
    {
        for (int i = 0; i < Attributes.Length; i++)
        {
            Attributes[i].SetParent(this);
        }

        for (int i = 0; i < Equipment.GetSlots.Length; i++)
        {
            Equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            Equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }
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

    public void OnBeforeSlotUpdate(InventorySlot slot)
    {
        if (slot.ItemObject == null)
        {
            return;
        }

        switch (slot.Parent.Inventory.Type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", slot.ItemObject, " on ", slot.Parent.Inventory.Type, ", Allowed Items: ", string.Join(", ", slot.AllowedItems)));

                for (int i = 0; i < slot.Item.Buffs.Length; i++)
                {
                    for (int j = 0; j < Attributes.Length; j++)
                    {
                        if (Attributes[j].Type == slot.Item.Buffs[i].Attribute)
                        {
                            Attributes[j].Value.RemoveModifier(slot.Item.Buffs[i]);
                        }
                    }
                }

                break;
            default:
                break;
        }
    }

    public void OnAfterSlotUpdate(InventorySlot slot)
    {
        if (slot.ItemObject == null)
        {
            return;
        }

        switch (slot.Parent.Inventory.Type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", slot.ItemObject, " on ", slot.Parent.Inventory.Type, ", Allowed Items: ", string.Join(", ", slot.AllowedItems)));

                for (int i = 0; i < slot.Item.Buffs.Length; i++)
                {
                    for (int j = 0; j < Attributes.Length; j++)
                    {
                        if (Attributes[j].Type == slot.Item.Buffs[i].Attribute)
                        {
                            Attributes[j].Value.AddModifier(slot.Item.Buffs[i]);
                        }
                    }
                }

                break;
            default:
                break;
        }
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

    public  void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.Type, " was updated! Value is now ", attribute.Value.ModifiedValue));
    }

    private void OnApplicationQuit()
    {
        Inventory.Clear();
        Equipment.Clear();
    }
}

[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public CharacterInventory Parent;
    public Attributes Type;
    public ModifiableInt Value;

    public void SetParent(CharacterInventory parent)
    {
        Parent = parent;
        Value = new ModifiableInt(AttributeModified);
    }

    public void AttributeModified()
    {
        Parent.AttributeModified(this);
    }
}