using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public InventoryObject Inventory;
    public InventoryObject Equipment;

    public Attribute[] Attributes;

    private Transform _head;
    private Transform _shoulder;
    private Transform _back;
    private Transform _weapon;
    private Transform _tool;

    public Transform WeaponHandTransform;
    public Transform WeaponHandToolTransform;
    public Transform ToolHandTransform;
    public Transform ToolHandWeaponTransform;

    private BoneCombiner _boneCombiner;

    private CharacterInput _characterInput;

    private void Awake()
    {
        InitializeInput();
    }

    private void Start()
    {
        _boneCombiner = new BoneCombiner(gameObject);

        for (int i = 0; i < Attributes.Length; i++)
        {
            Attributes[i].SetParent(this);
        }

        for (int i = 0; i < Equipment.GetSlots.Length; i++)
        {
            Equipment.GetSlots[i].OnBeforeUpdate += OnRemoveItem;
            Equipment.GetSlots[i].OnAfterUpdate += OnAddItem;
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

    public void OnRemoveItem(InventorySlot slot)
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

                if (slot.ItemObject.CharacterDisplay != null)
                {
                    switch (slot.AllowedItems[0])
                    {
                        case ItemType.Head:
                            Destroy(_head.gameObject);
                            break;
                        case ItemType.Shoulder:
                            Destroy(_shoulder.gameObject);
                            break;
                        case ItemType.Back:
                            Destroy(_back.gameObject);
                            break;
                        case ItemType.Weapon:
                            Destroy(_weapon.gameObject);
                            break;
                        case ItemType.Tool:
                            Destroy(_tool.gameObject);
                            break;
                    }
                }
                break;
        }
    }

    public void OnAddItem(InventorySlot slot)
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

                if (slot.ItemObject.CharacterDisplay != null)
                {
                    switch (slot.AllowedItems[0])
                    {
                        case ItemType.Head:
                            _head = _boneCombiner.AddLimb(slot.ItemObject.CharacterDisplay);
                            break;
                        case ItemType.Shoulder:
                            _shoulder = _boneCombiner.AddLimb(slot.ItemObject.CharacterDisplay);
                            break;
                        case ItemType.Back:
                            _back = _boneCombiner.AddLimb(slot.ItemObject.CharacterDisplay);
                            break;
                        case ItemType.Weapon:
                            switch (slot.ItemObject.Type)
                            {
                                case ItemType.Weapon:
                                    _weapon = Instantiate(slot.ItemObject.CharacterDisplay, WeaponHandTransform).transform;
                                    break;
                                case ItemType.Tool:
                                    _weapon = Instantiate(slot.ItemObject.CharacterDisplay, WeaponHandToolTransform).transform;
                                    break;
                            }
                            break;
                        case ItemType.Tool:
                            switch (slot.ItemObject.Type)
                            {
                                case ItemType.Tool:
                                    _tool = Instantiate(slot.ItemObject.CharacterDisplay, ToolHandTransform).transform;
                                    break;
                                case ItemType.Weapon:
                                    _tool = Instantiate(slot.ItemObject.CharacterDisplay, ToolHandWeaponTransform).transform;
                                    break;
                            }
                            break;
                    }
                } 
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