using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/Default")]
public class DefaultInfo : ItemInfo
{
    public void Awake()
    {
        Type = ItemType.Default;
    }
}
