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
                Debug.Log("成功找到要顯示的面板：" + panelName);
            }
            else
            {
                Debug.LogError("找不到對應名稱的面板：" + panelName);
            }
        }
    }
}
