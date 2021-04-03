using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] Items;

    [ContextMenu("Update ID'S")]
    public void UpdateID()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i].Data.Id != i)
            {
                Items[i].Data.Id = i;
            }
            
        }
    }

    public void OnAfterDeserialize()
    {
        UpdateID();
    }

    public void OnBeforeSerialize()
    {

    }
}
