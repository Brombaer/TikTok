using System.Collections.Generic;
using UnityEngine;

public class BoneCombiner 
{
    public readonly Dictionary<int, Transform> RootBoneDictionary = new Dictionary<int, Transform>();

    private readonly Transform[] _boneTransforms = new Transform[50];
    private readonly Transform _transform;

    public BoneCombiner(GameObject rootObj)
    {
        _transform = rootObj.transform;
        TraverseHierarchy(_transform);
    }

    public Transform AddLimb(GameObject bonedObj)
    {
        Transform limb = ProcessBonedObject(bonedObj.GetComponentInChildren<SkinnedMeshRenderer>());
        limb.SetParent(_transform);
        return limb;
    }

    private Transform ProcessBonedObject(SkinnedMeshRenderer renderer)
    {
        var bonedObj = new GameObject().transform;

        var meshRenderer = bonedObj.gameObject.AddComponent<SkinnedMeshRenderer>();

        var bones = renderer.bones;

        for (int i = 0; i < bones.Length; i++)
        {
            _boneTransforms[i] = RootBoneDictionary[bones[i].name.GetHashCode()];
        }

        meshRenderer.bones = _boneTransforms;
        meshRenderer.sharedMesh = renderer.sharedMesh;
        meshRenderer.materials = renderer.sharedMaterials;

        return bonedObj;
    }

    private void TraverseHierarchy(Transform transform)
    {
        foreach (Transform child in transform)
        {
            RootBoneDictionary.Add(child.name.GetHashCode(), child);
            TraverseHierarchy(child);
        }
    }
}
