using UnityEngine;
using UnityEngine.UI;

public class CloseHandler : MonoBehaviour
{
    [Header("Assign the UI Button that closes this GameObject")]
    public Button closeButton;

    private void Start()
    {
        if (closeButton == null)
        {
            Debug.LogWarning("Close button not assigned in CloseHandler on " + gameObject.name);
            return;
        }

        // Attach the listener
        closeButton.onClick.AddListener(Close);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        // Clean up the listener to avoid memory leaks
        if (closeButton != null)
            closeButton.onClick.RemoveListener(Close);
    }
}
