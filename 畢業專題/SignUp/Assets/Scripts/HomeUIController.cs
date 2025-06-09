using UnityEngine;

public class HomeUIController : MonoBehaviour
{
    void Start()
    {
        if (!string.IsNullOrEmpty(GameData.targetPanelName))
        {
            PanelManager panelManager = FindObjectOfType<PanelManager>();
            if (panelManager != null)
            {
                Debug.Log("找到 PanelManager，切換面板：" + GameData.targetPanelName);
                panelManager.ShowPanel(GameData.targetPanelName);
            }
            else
            {
                Debug.LogError("找不到 PanelManager！");
            }

            GameData.targetPanelName = "";
        }
    }
}
