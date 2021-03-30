using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Food,
    Equipment,
    Default
}

public abstract class ItemObject : ScriptableObject
{
    public GameObject Prefab;
    public ItemType Type;
    [TextArea(15,20)]
    public string Description;
}
