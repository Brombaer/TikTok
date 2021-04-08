using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineObject : MonoBehaviour
{
    //[SerializeField] private Material _outlineMaterial;
    [SerializeField] private float _outlineScaleFactor;
    [SerializeField] private Color _outlineColor;

    private Renderer _outlineRenderer;

    private void Start()
    {
        //_outlineRenderer = CreateOutline(_outlineMaterial, _outlineScaleFactor, _outlineColor);
        //_outlineRenderer.enabled = true;

        _outlineRenderer = GetComponentInChildren<MeshRenderer>();
    }

    //private Renderer CreateOutline(Material material, float scaleFactor, Color color)
    //{
    //    GameObject outlineObject = Instantiate(this.gameObject, transform.position, transform.rotation, transform);
    //    Renderer renderer = outlineObject.GetComponent<Renderer>();
    //
    //    renderer.material = material;
    //    renderer.material.SetColor("OutlineColor", color);
    //    renderer.material.SetFloat("Scale", scaleFactor);
    //    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    //
    //    outlineObject.GetComponent<OutlineObject>().enabled = false;
    //    outlineObject.GetComponent<Collider>().enabled = false;
    //
    //    renderer.enabled = false;
    //    return renderer;
    //}

    public void ShowOutline()
    {
        _outlineRenderer.material.SetFloat("Scale", _outlineScaleFactor);
        _outlineRenderer.material.SetColor("OutlineColor", _outlineColor);
    }

    public void HideOutline()
    {
        _outlineRenderer.material.SetFloat("OutlineColor", 0);
    }
}
