using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLocation : MonoBehaviour
{
    public QuestManager qManager;
    public QuestEvent qEvent;
    public QuestButton qButton;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag != "Player")
            return;

        if (qEvent.status != QuestEvent.EventStatus.Current)
            return;

        qEvent.UpdateQuestEvent(QuestEvent.EventStatus.Done);
        qButton.UpdateButton(QuestEvent.EventStatus.Done);
        qManager.UpdateQuestsOnCompletion(qEvent);


    }

    public void Setup(QuestManager qm, QuestEvent qe, QuestButton qb)
    {
        qManager = qm;
        qEvent = qe;
        qButton = qb;
        qe.button = qButton;
    }
}
