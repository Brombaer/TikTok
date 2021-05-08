using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerUICraftingRecipe : UserInterface
{
    private CraftingRecipe _craftingRecipe;
    [SerializeField] private Button _craftItemButton; 
    [SerializeField] private Image[] _inputItemImages;
    [SerializeField] private TMP_Text[] _inputItemAmounts;
    [SerializeField] private Image _outputItemImage;
    [SerializeField] private GameObject[] _slots;
    [SerializeField] private InventoryObject _playerInventory;

    public void AssignCraftingRecipe(CraftingRecipe recipe)
    {
        _craftingRecipe = recipe;

        if (_craftingRecipe != null)
        {
            _outputItemImage.sprite = recipe.Output.Item.UiDisplay;
        }

        for (int i = 0; i < _inputItemImages.Length; i++)
        {
            for (int j = 0; j < _inputItemAmounts.Length; j++)
            {
                if (_craftingRecipe != null && !_craftingRecipe.Inputs[i].IsEmpty())
                {
                    _inputItemImages[i].sprite = _craftingRecipe.Inputs[i].Item.UiDisplay;
                    _inputItemAmounts[j].text = _craftingRecipe.Inputs[j].Amount.ToString();
                }
            }
        }
        
        _craftItemButton.onClick.AddListener(OnCraftButtonClick);
    }

    private void OnCraftButtonClick()
    {
        bool hasAllIngredients = _craftingRecipe.HasAllIngredients(GetItemsFromSlots());
        Debug.Log(hasAllIngredients.ToString());

        if (hasAllIngredients)
        {
            if (!_craftingRecipe.Output.IsEmpty())
            {
                if (_playerInventory.AddItem(_craftingRecipe.Output.Item, _craftingRecipe.Output.Amount))
                {
                    for (int i = 0; i < _craftingRecipe.Inputs.Length; i++)
                    {
                        SlotsOnInterface[_slots[i]].RemoveItem();
                    }
                }
            }
        }
    }

    private ItemAmountPair[] GetItemsFromSlots()
    {
        var items = new ItemAmountPair[_slots.Length];

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = (SlotsOnInterface[_slots[i]]).Content;
        }

        return items;
    }

    protected override void CreateSlots()
    {
        SlotsOnInterface = new Dictionary<GameObject, InventorySlot>();
    
        for (int i = 0; i < Inventory.GetSlots.Length; i++)
        {
            var obj = _slots[i];
    
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
    
            Inventory.GetSlots[i].SlotDisplay = obj;
    
            SlotsOnInterface.Add(obj, Inventory.GetSlots[i]);
        }
    }
}
