using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private GameObject[] _skins;
    [SerializeField] private Transform _rigRoot;

    public void SetSkin(int index)
    {
        if (_skins != null && index < _skins.Length)
        {
            _skins[0].SetActive(false);
            _skins[index].SetActive(true);
        }
    }

    public void MatchRig(Transform reference)
    {
        ConfigureRagdollPosition(reference, _rigRoot);
    }

    public void ConfigureRagdollPosition(Transform reference, Transform ragdollPart)
    {
        ragdollPart.localPosition = reference.localPosition;
        ragdollPart.localRotation = reference.localRotation;

        for (int i = 0; i < reference.childCount; i++)
        {
            Transform referenceTransfrom = reference.GetChild(i);
            Transform ragdollTransform = ragdollPart.GetChild(i);

            if (referenceTransfrom != null && ragdollTransform != null)
            {
                ConfigureRagdollPosition(referenceTransfrom, ragdollTransform);
            }
        }
    }
}
