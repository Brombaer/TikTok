using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class UserInterface : MonoBehaviour
{
    public CharacterInventory Player;
    public InventoryObject Inventory;

    public Dictionary<GameObject, InventorySlot> ItemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    private void Start()
    {
        for (int i = 0; i < Inventory.Container.Items.Length; i++)
        {
            Inventory.Container.Items[i].Parent = this;
        }

        CreateSlots();

        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    private void Update()
    {
        UpdateSlots();
    }

    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in ItemsDisplayed)
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

    public abstract void CreateSlots();

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
        Player.MouseItem.Ui = obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface(GameObject obj)
    {
        Player.MouseItem.Ui = null;
    }

    public void OnEnter(GameObject obj)
    {
        Player.MouseItem.HoverObj = obj;

        if (ItemsDisplayed.ContainsKey(obj))
        {
            Player.MouseItem.HoverItem = ItemsDisplayed[obj];
        }
    }

    public void OnExit(GameObject obj)
    {
        Player.MouseItem.HoverObj = null;
        Player.MouseItem.HoverItem = null;
    }

    public void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(150, 150);
        mouseObject.transform.SetParent(transform.parent);

        if (ItemsDisplayed[obj].ID >= 0)
        {
            var image = mouseObject.AddComponent<Image>();
            image.sprite = Inventory.Database.GetItem[ItemsDisplayed[obj].ID].UiDisplay;
            image.raycastTarget = false;
        }

        Player.MouseItem.Obj = mouseObject;
        Player.MouseItem.Item = ItemsDisplayed[obj];
    }

    public void OnDragEnd(GameObject obj)
    {
        var itemOnMouse = Player.MouseItem;
        var mouseHoverItem = itemOnMouse.HoverItem;
        var mouseHoverObj = itemOnMouse.HoverObj;
        var GetItemObj = Inventory.Database.GetItem;

        if (itemOnMouse.Ui != null)
        {
            if (mouseHoverObj)
            {
                if (mouseHoverItem.CanPlaceInSlot(GetItemObj[ItemsDisplayed[obj].ID]) && (mouseHoverItem.Item.Id <= -1 || (mouseHoverItem.Item.Id >= 0 && ItemsDisplayed[obj].CanPlaceInSlot(GetItemObj[mouseHoverItem.Item.Id]))))
                {
                    Inventory.MoveItem(ItemsDisplayed[obj], mouseHoverItem.Parent.ItemsDisplayed[mouseHoverObj]);
                }
            }
        }
        else
        {
            Inventory.RemoveItem(ItemsDisplayed[obj].Item);
        }

        Destroy(itemOnMouse.Obj);
        itemOnMouse.Item = null;
    }

    public void OnDrag(GameObject obj)
    {
        if (Player.MouseItem.Obj != null)
        {
            Player.MouseItem.Obj.GetComponent<RectTransform>().position = Mouse.current.position.ReadValue();
        }
    }
}

public class MouseItem
{
    public UserInterface Ui;
    public GameObject Obj;
    public InventorySlot Item;
    public InventorySlot HoverItem;
    public GameObject HoverObj;
}
