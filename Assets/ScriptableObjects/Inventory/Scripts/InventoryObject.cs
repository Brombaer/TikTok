using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string SavePath;
    public ItemDatabaseObject Database;
    public Inventory Container;

    public bool AddItem(Item item, int amount)
    {
        if (EmptySlotCount <= 0)
        {
            return false;
        }

        InventorySlot slot = FindItemOnInventory(item);

        if (!Database.Items[item.Id].Stackable || slot == null)
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

            for (int i = 0; i < Container.Items.Length; i++)
            {
                if (Container.Items[i].Item.Id <= -1)
                {
                    counter++;
                }
            }

            return counter;
        }
    }

    public InventorySlot FindItemOnInventory(Item item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].Item.Id == item.Id)
            {
                return Container.Items[i];
            }
        }

        return null;
    }

    public InventorySlot SetFirstEmptySlot(Item item, int amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].Item.Id <= -1)
            {
                Container.Items[i].UpdateSlot(item, amount);
                return Container.Items[i];
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
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].Item == item)
            {
                Container.Items[i].UpdateSlot(null, 0);
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

            for (int i = 0; i < Container.Items.Length; i++)
            {
                Container.Items[i].UpdateSlot(newContainer.Items[i].Item, newContainer.Items[i].Amount);
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
    public InventorySlot[] Items = new InventorySlot[15];

    public void Clear()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].RemoveItem();
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];
    [System.NonSerialized]
    public UserInterface Parent;
    public Item Item;
    public int Amount;

    public ItemObject ItemObject
    {
        get
        {
            if (Item.Id >= 0)
            {
                return Parent.Inventory.Database.Items[Item.Id];
            }

            return null;
        }
    }

    public InventorySlot()
    {
        Item = new Item();
        Amount = 0;
    }

    public InventorySlot(Item item, int amount)
    {
        Item = item;
        Amount = amount;
    }

    public void UpdateSlot(Item item, int amount)
    {
        Item = item;
        Amount = amount;
    }

    public void RemoveItem()
    {
        Item = new Item();
        Amount = 0;
    }

    public void AddAmount(int value)
    {
        Amount += value;
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
