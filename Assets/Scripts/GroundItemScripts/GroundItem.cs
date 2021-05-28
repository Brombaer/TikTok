using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class GroundItem : MonoBehaviour
{
    [FormerlySerializedAs("ItemObject")] [FormerlySerializedAs("Item")] 
    public ItemInfo itemInfo;

    private const int INTERACTABLE_LAYER = 6;

    [SerializeField] private float _timer = 3;

    private void Start()
    {
        gameObject.layer = INTERACTABLE_LAYER;
        
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
