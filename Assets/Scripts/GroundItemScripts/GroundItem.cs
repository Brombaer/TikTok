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

    [SerializeField] private float _timer = 3;

    private void Start()
    {
        if (itemInfo != null)
        {
            itemInfo = Instantiate(itemInfo);
            itemInfo.InitializeModifiers();
            
            StartCoroutine(DisableGravity());
        }
    }
    
    private IEnumerator DisableGravity()
    {
        yield return new WaitForSeconds(_timer);

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
    
    private void OnDisable()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
}
