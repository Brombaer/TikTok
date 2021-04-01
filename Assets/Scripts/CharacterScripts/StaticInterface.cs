using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInterface : UserInterface
{
    public GameObject[] Slots;

    public override void CreateSlots()
    {
        ItemsDisplayed = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < Inventory.Container.Items.Length; i++)
        {
            var obj = Slots[i];

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            ItemsDisplayed.Add(obj, Inventory.Container.Items[i]);
        }
    }
}
