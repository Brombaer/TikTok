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

    public string name;
    public string description;
    public string id;
    public int order = -1;
    public EventStatus status;
    public QuestButton button;
   

    public List<QuestPath> pathlist = new List<QuestPath>();

    public QuestEvent(string n, string d)
    {
        id = Guid.NewGuid().ToString();
        name = n;
        description = d;
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
