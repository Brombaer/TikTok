using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class GroundItem : MonoBehaviour
{
    [FormerlySerializedAs("ItemObject")] [FormerlySerializedAs("Item")] 
    public ItemInfo itemInfo;
}
