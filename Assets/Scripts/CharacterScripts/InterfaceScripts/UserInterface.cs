using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class UserInterface : MonoBehaviour
{
    public InventoryObject Inventory;

    public Dictionary<GameObject, InventorySlot> SlotsOnInterface = new Dictionary<GameObject, InventorySlot>();

    private void Start()
    {
        for (int i = 0; i < Inventory.GetSlots.Length; i++)
        {
            Inventory.GetSlots[i].Parent = this;
            Inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }

        CreateSlots();

        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    private void OnSlotUpdate(InventorySlot slot)
    {
        if (slot.IsNotEmpty())
        {
            slot.SlotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.ItemInfo.UiDisplay;
            slot.SlotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            slot.SlotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = slot.Content.Amount == 1 ? "" : slot.Content.Amount.ToString("n0");
        }
        else
        {
            slot.SlotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            slot.SlotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            slot.SlotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

    protected abstract void CreateSlots();

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnterInterface(GameObject obj)
    {
        MouseData.InterfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface(GameObject obj)
    {
        MouseData.InterfaceMouseIsOver = null;
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.SlotHoveredOver = obj;
    }

    public void OnExit(GameObject obj)
    {
        MouseData.SlotHoveredOver = null;
    }

    public void OnDragStart(GameObject obj)
    {
        MouseData.TempItemBeingDragged = CreateTempItem(obj);
    }

    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;

        if (!SlotsOnInterface[obj].IsEmpty())
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(150, 150);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = SlotsOnInterface[obj].ItemInfo.UiDisplay;
            img.raycastTarget = false;
        }

        return tempItem;
    }

    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.TempItemBeingDragged);

        if (MouseData.InterfaceMouseIsOver == null)
        {
            SlotsOnInterface[obj].RemoveItem();
            return;
        }

        if (MouseData.SlotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.InterfaceMouseIsOver.SlotsOnInterface[MouseData.SlotHoveredOver];
            Inventory.SwapItems(SlotsOnInterface[obj], mouseHoverSlotData);
        }
    }

    public void OnDrag(GameObject obj)
    {
        if (MouseData.TempItemBeingDragged != null)
        {
            MouseData.TempItemBeingDragged.GetComponent<RectTransform>().position = Mouse.current.position.ReadValue();
        }
    }
}

public static class MouseData
{
    public static UserInterface InterfaceMouseIsOver;
    public static GameObject TempItemBeingDragged;
    public static GameObject SlotHoveredOver;
}
