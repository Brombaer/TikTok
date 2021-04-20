using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

public enum InterfaceType
{
    Inventory,
    Equipment,
    Crafting
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string SavePath;
    public ItemDatabaseObject Database;
    public InterfaceType Type;
    public Inventory Container;
    public InventorySlot[] GetSlots { get { return Container.Slots; } }

    public bool AddItem(Item item, int amount)
    {
        if (EmptySlotCount <= 0)
        {
            return false;
        }

        InventorySlot slot = FindItemOnInventory(item);

        if (!Database.ItemObjects[item.Id].Stackable || slot == null)
        {
            SetFirstEmptySlot(item, amount);
            return true;
        }

        slot.AddAmount(amount);
        return true;
    }

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;

            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].Item.Id <= -1)
                {
                    counter++;
                }
            }

            return counter;
        }
    }

    public InventorySlot FindItemOnInventory(Item item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].Item.Id == item.Id)
            {
                return GetSlots[i];
            }
        }

        return null;
    }

    public InventorySlot SetFirstEmptySlot(Item item, int amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].Item.Id <= -1)
            {
                GetSlots[i].UpdateSlot(item, amount);
                return GetSlots[i];
            }
        }

        // Set up funcionality for full inventory
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
        {
            InventorySlot temp = new InventorySlot(item2.Item, item2.Amount);
            item2.UpdateSlot(item1.Item, item1.Amount);
            item1.UpdateSlot(temp.Item, temp.Amount);
        }
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].Item == item)
            {
                GetSlots[i].UpdateSlot(null, 0);
            }
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
        //string saveData = JsonUtility.ToJson(this, true);
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(string.Concat(Application.persistentDataPath, SavePath));
        //bf.Serialize(file, saveData);
        //file.Close();

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, SavePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, SavePath)))
        {
            //BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(string.Concat(Application.persistentDataPath, SavePath), FileMode.Open);
            //JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            //file.Close();

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, SavePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);

            for (int i = 0; i < GetSlots.Length; i++)
            {
                GetSlots[i].UpdateSlot(newContainer.Slots[i].Item, newContainer.Slots[i].Amount);
            }

            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] Slots = new InventorySlot[15];

    public void Clear()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].RemoveItem();
        }
    }
}

public delegate void SlotUpdated(InventorySlot slot);

[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];

    [System.NonSerialized]
    public UserInterface Parent;
    [System.NonSerialized]
    public GameObject SlotDisplay;

    [System.NonSerialized]
    public SlotUpdated OnBeforeUpdate;
    [System.NonSerialized]
    public SlotUpdated OnAfterUpdate;

    public Item Item;
    public int Amount;

    public ItemObject ItemObject
    {
        get
        {
            if (Item.Id >= 0)
            {
                return Parent.Inventory.Database.ItemObjects[Item.Id];
            }

            return null;
        }
    }

    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
    }

    public InventorySlot(Item item, int amount)
    {
        UpdateSlot(item, amount);
    }

    public void UpdateSlot(Item item, int amount)
    {
        if (OnBeforeUpdate != null)
        {
            OnBeforeUpdate.Invoke(this);
        }

        Item = item;
        Amount = amount;

        if (OnAfterUpdate != null)
        {
            OnAfterUpdate.Invoke(this);
        }
    }

    public void RemoveItem()
    {
        UpdateSlot(new Item(), 0);
    }

    public void AddAmount(int value)
    {
        UpdateSlot(Item, Amount += value);
    }

    public bool CanPlaceInSlot(ItemObject itemObj)
    {
        if (AllowedItems.Length <= 0 || itemObj == null || itemObj.Data.Id < 0)
        {
            return true;
        }

        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (itemObj.Type == AllowedItems[i])
            {
                return true;
            }
        }

        return false;
    }
}
