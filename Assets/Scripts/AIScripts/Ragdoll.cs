using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private GameObject[] _skins;

    public void SetSkin(int index)
    {
        if (_skins != null && index < _skins.Length)
        {
            _skins[0].SetActive(false);
            _skins[index].SetActive(true);
        }
    }
}
