using UnityEngine;
using UnityEngine.SceneManagement;

public class Restriction : MonoBehaviour
{
    //Variable
    [SerializeField]
    private ItemInfo[] _itemsToEscape;


    //Function
    private void OnTriggerEnter(Collider other)
    {
        IKillable killable = other.GetComponent<IKillable>();
        if (killable != null && !other.isTrigger)
        {

            KillZoneEnteredEffect effect = killable.Effect();
            if (effect == KillZoneEnteredEffect.Kill)
            {
                gameObject.SetActive(false);
                SceneManager.LoadScene(1);

            }
            if (effect == KillZoneEnteredEffect.Escape)
            {            
                TryToEscape();
            }
        }
    }

    private void TryToEscape()
    {
        CharacterInteractController interactController = GetComponent<CharacterInteractController>();
        
        for (int i = 0; i < _itemsToEscape.Length; i++)
        {
            if (interactController.Inventory.FindItemOnInventory(_itemsToEscape[i]) != null)
            {
                /*gameObject.SetActive(true);*/
                SceneManager.LoadScene(2);
            }
            else
            {
                Debug.Log("You need a syringe");
            }
        }
    }
}
public enum KillZoneEnteredEffect
{
    None,
    Kill,
    Warn,
    Block,
    Escape
}
