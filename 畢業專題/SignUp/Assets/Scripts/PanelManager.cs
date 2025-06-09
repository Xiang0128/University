using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [System.Serializable]
    public class NamedPanel
    {
        public string name;
        public GameObject panel;
    }

    public List<NamedPanel> panels;
    public void ShowPanel(string panelName)
    {
        foreach (var p in panels)
        {
            p.panel.SetActive(p.name == panelName);
            if (p.panel != null)
            {
                Debug.Log("���\���n��ܪ����O�G" + panelName);
            }
            else
            {
                Debug.LogError("�䤣������W�٪����O�G" + panelName);
            }
        }
    }
}
