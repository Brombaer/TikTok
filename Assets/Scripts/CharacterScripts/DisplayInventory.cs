using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public GameObject InventoryPrefab;
    public InventoryObject Inventory;

    public int XStart;
    public int YStart;

    public int XSpaceBetweenItems;
    public int YSpaceBetweenItems;
    public int NumberOfColumn;

    private Dictionary<InventorySlot, GameObject> _itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    private void Start()
    {
        CreateDisplay();
    }
    
    private void Update()
    {
        UpdateDisplay();
    }
    
    public void CreateDisplay()
    {
        for (int i = 0; i < Inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = Inventory.Container.Items[i];

            var obj = Instantiate(/*Inventory.Container[i].Item.Prefab*/InventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = Inventory.Database.GetItem[slot.Item.Id].UiDisplay;

            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.Amount.ToString("n0");
            _itemsDisplayed.Add(slot, obj);
        }
    }
    
    public void UpdateDisplay()
    {
        for (int i = 0; i < Inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = Inventory.Container.Items[i];

            if (_itemsDisplayed.ContainsKey(slot))
            {
                _itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.Amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(/*Inventory.Container[i].Item.Prefab*/InventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = Inventory.Database.GetItem[slot.Item.Id].UiDisplay;
    
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.Amount.ToString("n0");
                _itemsDisplayed.Add(Inventory.Container.Items[i], obj);
            }
        }
    }
    
    public Vector3 GetPosition(int i)
    {
        return new Vector3(XStart + (XSpaceBetweenItems * (i % NumberOfColumn)), YStart + (-YSpaceBetweenItems * (i / NumberOfColumn)), 0);
    }
}
