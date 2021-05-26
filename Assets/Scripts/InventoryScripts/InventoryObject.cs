using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
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
    public InventorySlot[] GetSlots => Container.Slots;

    public bool AddItem(ItemInfo item, int amount)
    {
        if (EmptySlotCount <= 0)
        {
            return false;
        }

        InventorySlot slot = FindItemOnInventory(item);

        if (!item.Stackable || slot == null)
        {
            SetFirstEmptySlot(new ItemAmountPair(item, amount));
            return true;
        }

        slot.AddAmount(amount);
        return true;
    }

    private int EmptySlotCount
    {
        get
        {
            int counter = 0;

            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].IsEmpty())
                {
                    counter++;
                }
            }

            return counter;
        }
    }

    public InventorySlot FindItemOnInventory(ItemInfo item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].ContainsItem(item))
            {
                return GetSlots[i];
            }
        }

        return null;
    }

    private InventorySlot SetFirstEmptySlot(ItemAmountPair content)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].Content.IsEmpty())
            {
                GetSlots[i].UpdateSlot(content);
                return GetSlots[i];
            }
        }

        // Set up functionality for full inventory
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item2.CanPlaceInSlot(item1.ItemInfo) && item1.CanPlaceInSlot(item2.ItemInfo))
        {
            ItemAmountPair temp = item2.Content;
            item2.UpdateSlot(item1.Content);
            item1.UpdateSlot(temp);
        }
    }

    //public void RemoveItem(Item item)
    //{
    //    for (int i = 0; i < GetSlots.Length; i++)
    //    {
    //        if (GetSlots[i].Item == item)
    //        {
    //            GetSlots[i].UpdateSlot(null, 0);
    //        }
    //    }
    //}

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
                GetSlots[i].UpdateSlot(newContainer.Slots[i].Content);
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

    public Inventory()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i] = new InventorySlot();
        }
    }
    
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

    [System.NonSerialized] public UserInterface Parent;
    [System.NonSerialized] public GameObject SlotDisplay;

    [System.NonSerialized] public SlotUpdated OnBeforeUpdate;
    [System.NonSerialized] public SlotUpdated OnAfterUpdate;

    public ItemAmountPair Content;

    public ItemInfo ItemInfo
    {
        get
        {
            return Content.Item;
        }
    }

    public InventorySlot()
    {
        UpdateSlot(ItemAmountPair.Empty);
    }

    public InventorySlot(ItemAmountPair content)
    {
        UpdateSlot(content);
    }

    public void UpdateSlot(ItemAmountPair content)
    {
        OnBeforeUpdate?.Invoke(this);

        Content = content;

        OnAfterUpdate?.Invoke(this);
    }

    public void RemoveItem()
    {
        UpdateSlot(ItemAmountPair.Empty);
    }

    public void AddAmount(int value)
    {
        UpdateSlot(Content.Add(value));
    }

    public bool CanPlaceInSlot(ItemInfo itemObj)
    {
        if (AllowedItems.Length <= 0 || itemObj == null)
        {
            return true;
        }

        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (itemObj.Type == AllowedItems[i])
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/MenuButtons/Inventory/Equip");
                return true;
            }
        }

        return false;
    }

    public bool ContainsItem(ItemInfo info)
    {
        if (Content.Item == null)
        {
            return info == null;
        }
        
        return Content.Item.IsSameAs(info);
    }

    public bool IsEmpty()
    {
        return Content.IsEmpty();
    }

    public bool IsNotEmpty()
    {
        return !Content.IsEmpty();
    }
}
