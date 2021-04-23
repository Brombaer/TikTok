using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CraftingInterface : UserInterface
{
    public GameObject[] Slots;
    private Button _button;

    private void Start()
    {
        Button button = _button.GetComponent<Button>();
        button.onClick.AddListener(CraftItem);
    }
    protected override void CreateSlots()
    {
        SlotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        
        for (int i = 0; i < Inventory.GetSlots.Length; i++)
        {
            var obj = Slots[i];

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
