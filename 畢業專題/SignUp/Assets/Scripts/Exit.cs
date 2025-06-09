using UnityEngine;

public class ExitAppController : MonoBehaviour
{
    public GameObject popupPanel;

    public void OnExitButtonClicked()
    {
        popupPanel.SetActive(true);
    }

    public void OnConfirmExit()
    {
        Debug.Log("結束應用程式");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnCancelExit()
    {
        popupPanel.SetActive(false);
    }
}

