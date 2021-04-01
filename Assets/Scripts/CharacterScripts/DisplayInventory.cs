using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DisplayInventory : MonoBehaviour
{
    public MouseItem MouseItem = new MouseItem();

    public GameObject InventoryPrefab;
    public InventoryObject Inventory;

    public int XStart;
    public int YStart;

    public int XSpaceBetweenItems;
    public int YSpaceBetweenItems;
    public int NumberOfColumn;

    private Dictionary<GameObject, InventorySlot> _itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    private void Start()
    {
        CreateSlots();
    }
    
    private void Update()
    {
        UpdateSlots();
    }

    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in _itemsDisplayed)
        {
            if (slot.Value.ID >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = Inventory.Database.GetItem[slot.Value.Item.Id].UiDisplay;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.Amount == 1 ? "" : slot.Value.Amount.ToString("n0");
            }
            else
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
    
    public void CreateSlots()
    {
        _itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < Inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(InventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            _itemsDisplayed.Add(obj, Inventory.Container.Items[i]);
        }
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseItem.HoverObj = obj;

        if (_itemsDisplayed.ContainsKey(obj))
        {
            MouseItem.HoverItem = _itemsDisplayed[obj];
        }
    }

    public void OnExit(GameObject obj)
    {
        MouseItem.HoverObj = null;
        MouseItem.HoverItem = null;
    }

    public void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(150, 150);
        mouseObject.transform.SetParent(transform.parent);

        if (_itemsDisplayed[obj].ID >= 0)
        {
            var image = mouseObject.AddComponent<Image>();
            image.sprite = Inventory.Database.GetItem[_itemsDisplayed[obj].ID].UiDisplay;
            image.raycastTarget = false;
        }

        MouseItem.Obj = mouseObject;
        MouseItem.Item = _itemsDisplayed[obj];
    }

    public void OnDragEnd(GameObject obj)
    {
        if (MouseItem.HoverObj)
        {
            Inventory.MoveItem(_itemsDisplayed[obj], _itemsDisplayed[MouseItem.HoverObj]);
        }
        else
        {
            Inventory.RemoveItem(_itemsDisplayed[obj].Item);
        }

        Destroy(MouseItem.Obj);
        MouseItem.Item = null;
    }

    public void OnDrag(GameObject obj)
    {
        if (MouseItem.Obj != null)
        {
            MouseItem.Obj.GetComponent<RectTransform>().position = Mouse.current.position.ReadValue();
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(XStart + (XSpaceBetweenItems * (i % NumberOfColumn)), YStart + (-YSpaceBetweenItems * (i / NumberOfColumn)), 0);
    }
}