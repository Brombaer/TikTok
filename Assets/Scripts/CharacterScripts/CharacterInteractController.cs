using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CharacterInteractController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private LayerMask _layerMask;
    [SerializeField]
    private TextMeshProUGUI _itemNameText;
    [SerializeField] private float _maxInteractionDistance = 3;
    private GroundItem _itemBeingPickedUp;

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
    public Transform BackpackTransform;
    public Transform HeadTransform;

    private BoneCombiner _boneCombiner;

    private CharacterInput _characterInput;

    private Outline _prevOutlineObj;
    private Outline _currentOutlineObj;
    RaycastHit hitInfo;

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

    private void Update()
    {
        Raycast();
        OutlineGroundItem();

        if (HasItemTargetted())
        {
            _itemNameText.gameObject.SetActive(true);
        }
        else
        {
            _itemNameText.gameObject.SetActive(false);
        }
    }

    private bool HasItemTargetted()
    {
        return _itemBeingPickedUp != null;
    }

    private void Raycast()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward * _maxInteractionDistance);
    
        if (Physics.Raycast(ray, out hitInfo, _maxInteractionDistance, _layerMask))
        {
            var hitItem = hitInfo.collider.GetComponent<GroundItem>();
            //var outlineItem = hitItem.GetComponent<Outline>();

            if (hitInfo.distance <= _maxInteractionDistance)
            {
                if (hitItem == null)
                {
                    //outlineItem.OutlineWidth = 0;
                    _itemBeingPickedUp = null;
                }
                else if (hitItem != null && hitItem != _itemBeingPickedUp)
                {
                    //outlineItem.OutlineWidth = 5;
                    
                    _itemBeingPickedUp = hitItem;
                    _itemNameText.text = "Pickup " + _itemBeingPickedUp.gameObject.name;
                }
            }
            else
            {
                //outlineItem.OutlineWidth = 0;
            }
        }
        else
        {
            //_prevOutlineObj.enabled = false;
            _itemBeingPickedUp = null;
        }
    }

    private void OutlineGroundItem()
    {
        if (_itemBeingPickedUp != null)
        {
            _currentOutlineObj = hitInfo.collider.GetComponent<Outline>();

            if (_prevOutlineObj != _currentOutlineObj)
            {
                RemoveOutline();
                ShowOutline();

                _prevOutlineObj = _currentOutlineObj;
            }
        }
        else
        {
            RemoveOutline();
        }
    }

    private void RemoveOutline()
    {
        if (_prevOutlineObj != null)
        {
            //_prevOutlineObj.GetComponent<Outline>();
            _prevOutlineObj.enabled = false;
            _prevOutlineObj = null;
        }
    }

    private void ShowOutline()
    {
        if (_currentOutlineObj != null)
        {
            _currentOutlineObj.enabled = true;
        }
    }

    private void PickupItem()
    {
        if (_itemBeingPickedUp != null)
        {
            if (_itemBeingPickedUp.GetComponent<GroundItem>() != null)
            {
                Item item = new Item(_itemBeingPickedUp.Item);

                if (Inventory.AddItem(item, 1))
                {
                    Destroy(_itemBeingPickedUp.gameObject);
                }
            }
        }
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    var item = other.GetComponent<GroundItem>();
    //
    //    if (item)
    //    {
    //        Item _item = new Item(item.Item);
    //
    //        if (Inventory.AddItem(_item, 1))
    //        {
    //            Destroy(other.gameObject);
    //        }
    //    }
    //}

    private void InitializeInput()
    {
        _characterInput = new CharacterInput();

        _characterInput.Player.Interact.performed += context => PickupItem();
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
                            _head = Instantiate(slot.ItemObject.CharacterDisplay, HeadTransform).transform;

                            //_head = _boneCombiner.AddLimb(slot.ItemObject.CharacterDisplay);
                            break;
                        case ItemType.Shoulder:
                            _shoulder = _boneCombiner.AddLimb(slot.ItemObject.CharacterDisplay);
                            break;
                        case ItemType.Back:
                            _back = Instantiate(slot.ItemObject.CharacterDisplay, BackpackTransform).transform;

                            //_back = _boneCombiner.AddLimb(slot.ItemObject.CharacterDisplay);
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
    public CharacterInteractController Parent;
    public Attributes Type;
    public ModifiableInt Value;

    public void SetParent(CharacterInteractController parent)
    {
        Parent = parent;
        Value = new ModifiableInt(AttributeModified);
    }

    public void AttributeModified()
    {
        Parent.AttributeModified(this);
    }
}