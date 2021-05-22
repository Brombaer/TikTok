using UnityEngine;
using System.Collections;

public class QuestPickupable : MonoBehaviour
{
    public QuestEvent.ItemToComplete item;
    public QuestManager qManager;
    public QuestEvent qEvent;
    public QuestButton qButton;


    void Start()
    {
        
    }

    public void PickupableItems()
    {
        qEvent.UpdateQuestEvent(QuestEvent.EventStatus.Done);
        qButton.UpdateButton(QuestEvent.EventStatus.Done);
        qManager.UpdateQuestsOnCompletion(qEvent);
    }

}
