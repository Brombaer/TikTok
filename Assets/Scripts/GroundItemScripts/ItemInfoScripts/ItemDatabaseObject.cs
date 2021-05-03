using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemInfo[] ItemObjects;

    public void OnAfterDeserialize()
    {
        
    }

    public void OnBeforeSerialize()
    {

    }
}
