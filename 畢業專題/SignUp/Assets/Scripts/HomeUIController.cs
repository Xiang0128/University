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
                Debug.Log("��� PanelManager�A�������O�G" + GameData.targetPanelName);
                panelManager.ShowPanel(GameData.targetPanelName);
            }
            else
            {
                Debug.LogError("�䤣�� PanelManager�I");
            }

            GameData.targetPanelName = "";
        }
    }
}
