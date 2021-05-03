using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemAmountPair
{
    public ItemInfo Item;
    public int Amount;

    public static ItemAmountPair Empty
    {
        get
        {
            return new ItemAmountPair();
        }
    }

    public bool IsEmpty()
    {
        return Item == null || Amount <= 0;
    }
    
    public ItemAmountPair Add(int amount)
    {
        var itemPair =  new ItemAmountPair();
        
        itemPair.Item = this.Item;
        itemPair.Amount = this.Amount + amount;

        return itemPair;
    }

    public ItemAmountPair(ItemInfo item, int amount)
    {
        Item = item;
        Amount = amount;
    }
}

[CreateAssetMenu]
public class CraftingRecipe : ScriptableObject
{
    public ItemAmountPair Output;
    public ItemAmountPair[] Inputs;

    public bool HasAllIngredients(ItemAmountPair[] objects)
    {
        return false;
    }
    
    
}
