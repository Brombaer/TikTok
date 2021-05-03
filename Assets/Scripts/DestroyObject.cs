using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] private float _timeToDestroy = 2;

    private void Start()
    {
        Destroy(gameObject, _timeToDestroy);
    }
}
