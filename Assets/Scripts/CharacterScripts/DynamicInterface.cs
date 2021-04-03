using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    public GameObject InventoryPrefab;

    public int XStart;
    public int YStart;

    public int XSpaceBetweenItems;
    public int YSpaceBetweenItems;
    public int NumberOfColumn;

    public override void CreateSlots()
    {
        SlotsOnInterface = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < Inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(InventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            SlotsOnInterface.Add(obj, Inventory.Container.Items[i]);
        }
    }

    private Vector3 GetPosition(int i)
    {
        return new Vector3(XStart + (XSpaceBetweenItems * (i % NumberOfColumn)), YStart + (-YSpaceBetweenItems * (i / NumberOfColumn)), 0);
    }
}
