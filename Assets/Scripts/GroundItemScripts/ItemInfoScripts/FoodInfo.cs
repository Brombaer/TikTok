using UnityEngine;

[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/Food")]
public class FoodInfo : ItemInfo
{
    public void Awake()
    {
        Type = ItemType.Food;
    }
}
