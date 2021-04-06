using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Food,
    Head,
    Back,
    Shoulder,
    Weapon,
    Tool,
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
    public Sprite UiDisplay;
    public GameObject CharacterDisplay;
    public bool Stackable;
    public ItemType Type;
    [TextArea(15,20)]
    public string Description;
    public Item Data = new Item();

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
    public int Id = -1;
    public ItemBuff[] Buffs;

    public Item()
    {
        Name = "";
        Id = -1;
    }

    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.Data.Id;

        Buffs = new ItemBuff[item.Data.Buffs.Length];

        for (int i = 0; i < Buffs.Length; i++)
        {
            Buffs[i] = new ItemBuff(item.Data.Buffs[i].Min, item.Data.Buffs[i].Max)
            {
                Attribute = item.Data.Buffs[i].Attribute
            };
        }
    }
}

[System.Serializable]
public class ItemBuff : IModifiers
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

    public void AddValue(ref int baseValue)
    {
        baseValue += Value;
    }

    public void GenerateValue()
    {
        Value = Random.Range(Min, Max);
    }
}
