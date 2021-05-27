using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class QuestManager : MonoBehaviour
{

    public Quest quest = new Quest();
    public GameObject questPrintBox;
    public GameObject buttonPrefab;
    public GameObject questUI;


    List<QuestButton> buttons = new List<QuestButton>();

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
       

        QuestEvent a = quest.AddQuestEvent("Pick Up Any Weapon", "You Can Kill Zombies With Weapon", new QuestEvent.ItemToComplete[]
        { QuestEvent.ItemToComplete.Crowbar,
        QuestEvent.ItemToComplete.Hammer,
        QuestEvent.ItemToComplete.Katana,
        QuestEvent.ItemToComplete.BuzzBlade,
        QuestEvent.ItemToComplete.BrokenAlcoholBottle1,
        QuestEvent.ItemToComplete.BrokenAlcoholBottle2,
        QuestEvent.ItemToComplete.MetalBat,
        QuestEvent.ItemToComplete.WoodBat,
            QuestEvent.ItemToComplete.Pan
         });
        QuestEvent b = quest.AddQuestEvent("Quest Tutorial", "Go to the Food Market");
        QuestEvent c = quest.AddQuestEvent("Find The Location", "Go to the Repairs");
        QuestEvent d = quest.AddQuestEvent("Getting Ready for Crafting", "Find The Crafting Materials");
        QuestEvent e = quest.AddQuestEvent("Craft your Syringe", "You May Survive");

        quest.Addpath(a.GetId(), b.GetId());
        quest.Addpath(b.GetId(), c.GetId());
        quest.Addpath(c.GetId(), d.GetId());
        quest.Addpath(d.GetId(), e.GetId());
        //quest.Addpath(d.GetId(), e.GetId());

        quest.BFS(a.GetId());



        QuestButton button = CreateButton(a).GetComponent<QuestButton>();
        A.GetComponent<QuestLocation>().Setup(this, a, button);
        buttons.Add(button);
        button = CreateButton(b).GetComponent<QuestButton>();
        B.GetComponent<QuestLocation>().Setup(this, b, button);
        buttons.Add(button);

        button = CreateButton(c).GetComponent<QuestButton>();
      
        buttons.Add(button);

        button = CreateButton(d).GetComponent<QuestButton>();
       
        buttons.Add(button);

        button = CreateButton(e).GetComponent<QuestButton>();
        
        buttons.Add(button);

        quest.PrintPath();

    }
    private void ToggleQuestSystem(InputAction.CallbackContext context)
    {
        if (questUI.activeSelf)
        {
            questUI.SetActive(false);
            FMODUnity.RuntimeManager.PlayOneShot("event:/MenuButtons/Questlog/Activate");
        }
        else if(questUI.activeSelf ==false)
        {
            questUI.SetActive(true);
            FMODUnity.RuntimeManager.PlayOneShot("event:/MenuButtons/Questlog/Deactivate");
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


    public void UpdateQuestsOnCompletion(QuestEvent.ItemToComplete e)
    {
        if (e == QuestEvent.ItemToComplete.None)
        {
            quest.questEvents[0].UpdateQuestEvent(QuestEvent.EventStatus.Done);
            buttons[0].UpdateButton(QuestEvent.EventStatus.Done);
            buttons.RemoveAt(0);
            buttons[0].UpdateButton(QuestEvent.EventStatus.Current);
            quest.questEvents.RemoveAt(0);
            quest.questEvents[0].UpdateQuestEvent(QuestEvent.EventStatus.Current);

            /*foreach (QuestEvent n in quest.questEvents)
            {
                if (n.order == (e.order + 1))
                {
                    n.UpdateQuestEvent(QuestEvent.EventStatus.Current);
                    FMODUnity.RuntimeManager.PlayOneShot("event:/MenuButtons/Questlog/New");
                }
            }*/
        }
        else
        {
           
                    if (quest.questEvents[0].itemsToComplete.Contains(e))
                    {
                        quest.questEvents[0].UpdateQuestEvent(QuestEvent.EventStatus.Done);
                        buttons[0].UpdateButton(QuestEvent.EventStatus.Done);
                        buttons.RemoveAt(0);
                        buttons[0].UpdateButton(QuestEvent.EventStatus.Current);
                        quest.questEvents.RemoveAt(0);
                        quest.questEvents[0].UpdateQuestEvent(QuestEvent.EventStatus.Current);
                        

                    }
                
        }
    }
}
