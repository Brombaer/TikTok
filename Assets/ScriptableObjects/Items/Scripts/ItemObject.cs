using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Food,
    Equipment,
    Default
}

public enum Attributes
{
    Strength,
    Durability,
    Agility
}

public abstract class ItemObject : ScriptableObject
{
    public int Id;
    public Sprite UiDisplay;
    public ItemType Type;
    [TextArea(15,20)]
    public string Description;

    public ItemBuff[] Buffs;

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id;
    public ItemBuff[] Buffs;

    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.Id;

        Buffs = new ItemBuff[item.Buffs.Length];

        for (int i = 0; i < Buffs.Length; i++)
        {
            Buffs[i] = new ItemBuff(item.Buffs[i].Min, item.Buffs[i].Max)
            {
                Attribute = item.Buffs[i].Attribute
            };
        }
    }
}

[System.Serializable]
public class ItemBuff
{
    public Attributes Attribute;
    public int Value;
    public int Min;
    public int Max;

    public ItemBuff(int min, int max)
    {
        Min = min;
        Max = max;

        GenerateValue();
    }

    public void GenerateValue()
    {
        Value = Random.Range(Min, Max);
    }
}
