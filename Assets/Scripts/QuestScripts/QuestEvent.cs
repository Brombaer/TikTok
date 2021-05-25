using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestEvent
{
  public enum EventStatus
    {
        Waiting,
        Current,
        Done
    };

    public enum ItemToComplete
    {
        None,
        Crowbar,
        Hammer,
        Katana,
        BuzzBlade,
        BrokenAlcoholBottle1,
        BrokenAlcoholBottle2,
        MetalBat,
        WoodBat,
        Pan
    }

    public string name;
    public string description;
    public string id;
    public int order = -1;
    public EventStatus status;
    public QuestButton button;
    public ItemToComplete itemToComplete = ItemToComplete.None;
   



    public List<QuestPath> pathlist = new List<QuestPath>();



    public QuestEvent(string n, string d, ItemToComplete i)
    {
        id = Guid.NewGuid().ToString();
        name = n;
        description = d;
        itemToComplete = i;
        status = EventStatus.Waiting;

    }


    public void UpdateQuestEvent(EventStatus es)
    {
        status = es;
        button.UpdateButton(es);
    }

    public string GetId()
    {
        return id;
    }
}
