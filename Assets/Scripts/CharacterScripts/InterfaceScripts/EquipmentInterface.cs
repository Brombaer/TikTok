using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentInterface : UserInterface
{
    public GameObject[] Slots;

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
