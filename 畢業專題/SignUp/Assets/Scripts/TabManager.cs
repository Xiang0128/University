using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tabs : MonoBehaviour
{
    public List<TabButton> tabButtons;  // List of tab buttons
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public TabButton selectedTab;
    public List<GameObject> panels;  // Panels to switch

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.sprite = tabHover;
            button.background.type = Image.Type.Sliced;
        }
    }

    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.sprite = tabActive;
        button.background.type = Image.Type.Sliced;

        int index = tabButtons.IndexOf(button);
        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetActive(i == index);
        }
    }

    public void ResetTabs()
    {
        foreach (TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) continue;
            button.background.sprite = tabIdle;
            button.background.type = Image.Type.Sliced;
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

}
