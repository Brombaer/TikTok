using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour
{
    public Button buttonComponent;
    public RawImage icon;
    public Text eventName;
    public Sprite currentImage;
    public Sprite waitingImage;
    public Sprite doneImage;
    public QuestEvent thisEvent;

    QuestEvent.EventStatus status;


  

    public void Setup (QuestEvent e, GameObject scrollList)
    {
        thisEvent = e;
        buttonComponent.transform.SetParent(scrollList.transform, false);
        eventName.text = "<b>" + thisEvent.name + "</b>\n" + thisEvent.description;
        status = thisEvent.status;
        icon.texture = waitingImage.texture;
        buttonComponent.interactable = false;
    }

    public void UpdateButton(QuestEvent.EventStatus s)
    {
        status = s;
        if(status == QuestEvent.EventStatus.Done)
        {
            icon.texture = doneImage.texture;
            buttonComponent.interactable = false;
            FMODUnity.RuntimeManager.PlayOneShot("event:/MenuButtons/Questlog/Finished");
        }

        else if (status == QuestEvent.EventStatus.Waiting)
        {
            icon.texture = waitingImage.texture;
            buttonComponent.interactable = false;
        }

        else if (status == QuestEvent.EventStatus.Current)
        {
            icon.texture = currentImage.texture;
            buttonComponent.interactable = true;
        }
    }

    
}
