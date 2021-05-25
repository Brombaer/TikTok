using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public Quest quest = new Quest();
    public GameObject questPrintBox;
    public GameObject buttonPrefab;
    public GameObject questUI;


    public GameObject A;
    public GameObject B;
    public GameObject C;
    public GameObject D;
    public GameObject E;


    private CharacterInput _characterInput;

    private void Awake()
    {
        InitializeInput();
    }
    private void Start()
    {


        questUI.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;


        QuestEvent a = quest.AddQuestEvent("Quest Tutorial", "Go to the Food Market",QuestEvent.ItemToComplete.BrokenAlcoholBottle1);
        QuestEvent b = quest.AddQuestEvent("FInd The Location", "Go to the Repairs",QuestEvent.ItemToComplete.BrokenAlcoholBottle2);
        QuestEvent c = quest.AddQuestEvent("Find The Light Source", "Find the Flash light", QuestEvent.ItemToComplete.WoodBat);
        QuestEvent d = quest.AddQuestEvent("FInd The Light Source", "Find the Flip Lighter", QuestEvent.ItemToComplete.MetalBat);
        QuestEvent e = quest.AddQuestEvent("test5", "description 5",QuestEvent.ItemToComplete.Hammer);

        quest.Addpath(a.GetId(), b.GetId());
        quest.Addpath(b.GetId(), c.GetId());
        quest.Addpath(b.GetId(), d.GetId());
        quest.Addpath(c.GetId(), e.GetId());
        quest.Addpath(d.GetId(), e.GetId());

        quest.BFS(a.GetId());


        QuestButton button = CreateButton(a).GetComponent<QuestButton>();
        A.GetComponent<QuestLocation>().Setup(this, a, button);
        button = CreateButton(b).GetComponent<QuestButton>();
        B.GetComponent<QuestLocation>().Setup(this, b, button);
        button = CreateButton(c).GetComponent<QuestButton>();
         button = CreateButton(c).GetComponent<QuestButton>();
        C.GetComponent<QuestLocation>().Setup(this, c, button);
        button = CreateButton(d).GetComponent<QuestButton>();
        D.GetComponent<QuestLocation>().Setup(this, d, button);
        button = CreateButton(e).GetComponent<QuestButton>();
        E.GetComponent<QuestLocation>().Setup(this, e, button);

        quest.PrintPath();

    }
    private void ToggleQuestSystem(InputAction.CallbackContext context)
    {
        if (questUI.activeSelf)
        {
            questUI.SetActive(false);
        }
        else if(questUI.activeSelf ==false)
        {
            questUI.SetActive(true);
        }

    }

    private void InitializeInput()
    {
        _characterInput = new CharacterInput();

        _characterInput.Player.Quest.performed += ToggleQuestSystem;
    }

    GameObject CreateButton(QuestEvent e)
    {
        GameObject b = Instantiate(buttonPrefab);
        b.GetComponent<QuestButton>().Setup(e, questPrintBox);
        if(e.order == 1)
        {
            b.GetComponent<QuestButton>().UpdateButton(QuestEvent.EventStatus.Current);
            e.status = QuestEvent.EventStatus.Current;
        }

        return b;
    }

    private void OnEnable()
    {
        _characterInput.Enable();
    }

    private void OnDisable()
    {
        _characterInput.Disable();
    }


    public void UpdateQuestsOnCompletion(QuestEvent e)
    {
        if (e.itemToComplete == QuestEvent.ItemToComplete.None)

            foreach (QuestEvent n in quest.questEvents)
        {
            if(n.order == (e.order + 1))
            {
                n.UpdateQuestEvent(QuestEvent.EventStatus.Current);
            }
        }
    }
}
