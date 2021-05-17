using UnityEngine;
using UnityEngine.Serialization;

public enum ItemType
{
    Food,
    Head,
    Back,
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
    [FormerlySerializedAs("CharacterDisplay")] public GameObject visualisedGameObject;
    public int SpawnRate;
    public bool Stackable;
    public ItemType Type;
    [TextArea(15,20)]
    public string Description;

    public void InitializeModifiers()
    {
        foreach (var buff in Buffs)
        {
            buff.GenerateValue();
        }
    }

    public bool IsSameAs(ItemInfo info)
    {
        if (info == null)
        {
            return false;
        }
        
        return ItemName == info.ItemName;
    }
}

[System.Serializable]
public class ItemBuff : IModifiers
{
    public Attributes Attribute;
    public int Value;
    public int Min;
    public int Max;

    public void AddValue(ref int baseValue)
    {
        baseValue += Value;
    }

    public void GenerateValue()
    {
        Value = Random.Range(Min, Max);
    }
}
