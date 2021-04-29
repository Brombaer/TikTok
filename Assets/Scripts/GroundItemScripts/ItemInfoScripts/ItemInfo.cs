using UnityEngine;

public enum ItemType
{
    Food,
    Head,
    Back,
    Shoulder,
    Weapon,
    Tool,
    Crafting,
    Default
}

public enum Attributes
{
    Strength,
    Durability,
    Agility,
}

public abstract class ItemInfo : ScriptableObject
{
    public string ItemName;
    public ItemBuff[] Buffs;
    
    public Sprite UiDisplay;
    public GameObject CharacterDisplay;
    public bool Stackable;
    public ItemType Type;
    [TextArea(15,20)]
    public string Description;
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