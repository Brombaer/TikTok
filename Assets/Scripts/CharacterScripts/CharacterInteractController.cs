using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInteractController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    //[SerializeField] private Transform _target;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private float _maxInteractionDistance = 3;

    private GroundItem _itemBeingPickedUp;
    private Outline _prevOutlineObj;
    private Outline _currentOutlineObj;
    private RaycastHit _hitInfo;

    public InventoryObject Inventory;
    public InventoryObject Equipment;
    public InventoryObject Crafting;

    public Attribute[] Attributes;

    private Transform _head;
    private Transform _back;
    private Transform _weapon;
    private Transform _tool;

    [SerializeField] private Transform _weaponHandTransform;
    [SerializeField] private Transform _weaponHandToolTransform;
    [SerializeField] private Transform _toolHandTransform;
    [SerializeField] private Transform _toolHandWeaponTransform;
    [SerializeField] private Transform _backpackTransform;
    [SerializeField] private Transform _headTransform;

    private BoneCombiner _boneCombiner;

    private CharacterInput _characterInput;
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        InitializeInput();
    }

    private void Start()
    {
        _inventoryUI.SetActive(false);
        _itemNameText.gameObject.SetActive(false);

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

        for (int i = 0; i < Crafting.GetSlots.Length; i++)
        {
            Crafting.GetSlots[i].OnBeforeUpdate += OnRemoveItem;
            Crafting.GetSlots[i].OnAfterUpdate += OnAddItem;
        }
    }

    private void Update()
    {
        if (CharacterMovement.IsEnabled)
        {
            Raycast();
            OutlineGroundItem();

            _itemNameText.gameObject.SetActive(HasItemTargeted());
        }
    }

    private bool HasItemTargeted()
    {
        return _itemBeingPickedUp != null;
    }

    private void Raycast()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward * _maxInteractionDistance);
    
        if (Physics.Raycast(ray, out _hitInfo, _maxInteractionDistance, _layerMask))
        {
            var hitItem = _hitInfo.collider.GetComponent<GroundItem>();

            if (_hitInfo.distance <= _maxInteractionDistance)
            {
                if (hitItem == null)
                {
                    _itemBeingPickedUp = null;
                }
                else if (hitItem != _itemBeingPickedUp)
                {
                    _itemBeingPickedUp = hitItem;
                    _itemNameText.text = $"Pickup {_itemBeingPickedUp.gameObject.name}";
                }
            }
        }
        else
        {
            _itemBeingPickedUp = null;
        }
    }

    private void OutlineGroundItem()
    {
        if (_itemBeingPickedUp)
        {
            _currentOutlineObj = _hitInfo.collider.GetComponent<Outline>();

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

    private void ShowOutline()
    {
        if (_currentOutlineObj)
        {
            _currentOutlineObj.enabled = true;
        }
    }

    private void RemoveOutline()
    {
        if (_prevOutlineObj)
        {
            _prevOutlineObj.enabled = false;
            _prevOutlineObj = null;
        }
    }

    private void PickupItem(InputAction.CallbackContext context)
    {
        if (_itemBeingPickedUp)
        {
            if (Inventory.AddItem(_itemBeingPickedUp.itemInfo, 1))
            {
                Destroy(_itemBeingPickedUp.gameObject);
            }
        }
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        if (_inventoryUI.activeSelf)
        {
            _inventoryUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;

            //GetComponent<CharacterController>().enabled = true;
            //CharacterMovement.IsEnabled = true;
            //_camera.GetComponent<CinemachineBrain>().enabled = true;
        }
        else if (_inventoryUI.activeSelf == false)
        {
            _inventoryUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;

            //GetComponent<CharacterController>().enabled = false;
            //CharacterMovement.IsEnabled = false;
            //_camera.GetComponent<CinemachineBrain>().enabled = false;
        }
    }

    //private void ToggleCharacterInput()
    //{
    //    if (GetComponent<CharacterController>().enabled)
    //    {
    //        GetComponent<CharacterController>().enabled = false;
    //    }
    //    else
    //    {
    //        GetComponent<CharacterController>().enabled = true;
    //    }
    //}

    private void InitializeInput()
    {
        _characterInput = new CharacterInput();

        _characterInput.Player.Interact.performed += PickupItem;
        _characterInput.Player.Inventory.performed += ToggleInventory;
        _characterInput.Player.Save.performed += SaveInventory;
        _characterInput.Player.Load.performed += LoadInventory;
    }

    private void OnRemoveItem(InventorySlot slot)
    {
        if (slot.ItemInfo == null)
        {
            return;
        }

        switch (slot.Parent.Inventory.Type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", slot.ItemInfo, " on ", slot.Parent.Inventory.Type, ", Allowed Items: ", string.Join(", ", slot.AllowedItems)));

                for (int i = 0; i < slot.Content.Item.Buffs.Length; i++)
                {
                    for (int j = 0; j < Attributes.Length; j++)
                    {
                        if (Attributes[j].Type == slot.Content.Item.Buffs[i].Attribute)
                        {
                            Attributes[j].Value.RemoveModifier(slot.Content.Item.Buffs[i]);
                        }
                    }
                }

                if (slot.ItemInfo.visualisedGameObject != null)
                {
                    switch (slot.AllowedItems[0])
                    {
                        case ItemType.Head:
                            Destroy(_head.gameObject);
                            break;
                        case ItemType.Back:
                            Destroy(_back.gameObject);
                            break;
                        case ItemType.Weapon:
                            Destroy(_weapon.gameObject);
                            _animator.SetBool("isHoldingWeapon", false);
                            break;
                        case ItemType.Tool:
                            Destroy(_tool.gameObject);
                            break;
                    }
                }
                break;
            case InterfaceType.Crafting:
                print(string.Concat("Removed ", slot.ItemInfo, " on ", slot.Parent.Inventory.Type, ", Allowed Items: ", string.Join(", ", slot.AllowedItems)));
                break;
        }
    }

    private void OnAddItem(InventorySlot slot)
    {
        if (slot.ItemInfo == null)
        {
            return;
        }

        switch (slot.Parent.Inventory.Type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", slot.ItemInfo, " on ", slot.Parent.Inventory.Type, ", Allowed Items: ", string.Join(", ", slot.AllowedItems)));

                for (int i = 0; i < slot.Content.Item.Buffs.Length; i++)
                {
                    for (int j = 0; j < Attributes.Length; j++)
                    {
                        if (Attributes[j].Type == slot.Content.Item.Buffs[i].Attribute)
                        {
                            Attributes[j].Value.AddModifier(slot.Content.Item.Buffs[i]);
                        }
                    }
                }

                if (slot.ItemInfo.visualisedGameObject != null)
                {
                    switch (slot.AllowedItems[0])
                    {
                        case ItemType.Head:
                            _head = Instantiate(slot.ItemInfo.visualisedGameObject, _headTransform).transform;

                            //_head = _boneCombiner.AddLimb(slot.ItemObject.CharacterDisplay);
                            break;
                        case ItemType.Back:
                            _back = Instantiate(slot.ItemInfo.visualisedGameObject, _backpackTransform).transform;

                            //_back = _boneCombiner.AddLimb(slot.ItemObject.CharacterDisplay);
                            break;
                        case ItemType.Weapon:
                            switch (slot.ItemInfo.Type)
                            {
                                case ItemType.Weapon:
                                    _weapon = Instantiate(slot.ItemInfo.visualisedGameObject, _weaponHandTransform).transform;
                                    _animator.SetBool("isHoldingWeapon", true);
                                    break;
                                case ItemType.Tool:
                                    _weapon = Instantiate(slot.ItemInfo.visualisedGameObject, _weaponHandToolTransform).transform;
                                    break;
                            }
                            break;
                        case ItemType.Tool:
                            switch (slot.ItemInfo.Type)
                            {
                                case ItemType.Tool:
                                    _tool = Instantiate(slot.ItemInfo.visualisedGameObject, _toolHandTransform).transform;
                                    break;
                                case ItemType.Weapon:
                                    _tool = Instantiate(slot.ItemInfo.visualisedGameObject, _toolHandWeaponTransform).transform;
                                    break;
                            }
                            break;
                    }
                } 
                break;
            case InterfaceType.Crafting:
                print(string.Concat("Placed ", slot.ItemInfo, " on ", slot.Parent.Inventory.Type, ", Allowed Items: ", string.Join(", ", slot.AllowedItems)));
                break;
        }
    }

    private void SaveInventory(InputAction.CallbackContext context)
    {
        Inventory.Save();
        Equipment.Save();
        Crafting.Save();
    }

    private void LoadInventory(InputAction.CallbackContext context)
    {
        Inventory.Load();
        Equipment.Load();
        Crafting.Load();
    }

    private void OnEnable()
    {
        _characterInput.Enable();
    }

    private void OnDisable()
    {
        _characterInput.Disable();
    }

    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.Type, " was updated! Value is now ", attribute.Value.ModifiedValue));
    }

    private void OnApplicationQuit()
    {
        Inventory.Clear();
        Equipment.Clear();
        Crafting.Clear();
    }
}

[System.Serializable]
public class Attribute
{
    [System.NonSerialized] public CharacterInteractController Parent;
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