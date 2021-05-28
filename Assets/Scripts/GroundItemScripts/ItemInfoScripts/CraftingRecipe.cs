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
    
    public bool IsNotContainedIn(ItemAmountPair[] objects)
    {
        foreach (var obj in objects)
        {
            if (Item != null && Item.IsSameAs(obj.Item))
            {
                if (Amount <= obj.Amount)
                {
                    return false;
                }
            }
        }

        return true;
    }
}

[CreateAssetMenu]
public class CraftingRecipe : ScriptableObject
{
    public ItemAmountPair Output;
    public ItemAmountPair[] Inputs;

    public bool HasAllIngredients(ItemAmountPair[] objects)
    {
        for (int i = 0; i < Inputs.Length; i++)
        {
            if (Inputs[i].IsNotContainedIn(objects))
            {
                return false;
            }
        }
    
        return true;
    }
}
