using UnityEngine;
using System.Collections;

public class QuestPickupable : MonoBehaviour
{
    public QuestEvent.ItemToComplete item = QuestEvent.ItemToComplete.None;
    public QuestManager qManager;
    public QuestEvent qEvent;
    public QuestButton qButton;


    void Start()
    {

    }

    public void PickupableItems()
    {

        if (qManager != null)
        {
            qManager.UpdateQuestsOnCompletion(item);
        }
    }

}

