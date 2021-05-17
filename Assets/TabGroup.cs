using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    private List<TabButton> TabButtons;
    [SerializeField] private Sprite TabIdle;
    [SerializeField] private Sprite TabHover;
    [SerializeField] private Sprite TabActive;
    [SerializeField] private TabButton SelectedTab;
    [SerializeField] private List<GameObject> ObjectsToSwap;

    public void Subscribe(TabButton button)
    {
        if (TabButtons == null)
        {
            TabButtons = new List<TabButton>();
        }
        
        TabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();

        if (SelectedTab != null && button != SelectedTab)
        {
            button.Background.sprite = TabHover;
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        SelectedTab = button;
        ResetTabs();
        button.Background.sprite = TabActive;
        
        int index = button.transform.GetSiblingIndex();

        for (int i = 0; i < ObjectsToSwap.Count; i++)
        {
            if (i == index)
            {
                ObjectsToSwap[i].SetActive(true);
            }
            else
            {
                if (ObjectsToSwap[i].name == "InventoryPanel")
                {
                    ObjectsToSwap[i].SetActive(true);
                }
                else
                {
                    ObjectsToSwap[i].SetActive(false);
                }
            }
        }
    }

    private void ResetTabs()
    {
        foreach (TabButton button in TabButtons)
        {
            if (SelectedTab != null && button == SelectedTab)
            {
                continue;
            }
            
            button.Background.sprite = TabIdle;
        }
    }
}
