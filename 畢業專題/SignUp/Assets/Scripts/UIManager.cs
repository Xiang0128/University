using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Tooltip("List all root panel GameObjects in the scene here (lecturePanel, quizPanel, editLecturePanel, wordEditPanel, grammarEditPanel, sentenceEditPanel)")]
    public GameObject[] allPanels;

    /// <summary>
    /// Show only the target panel, hide all others.
    /// </summary>
    public void ShowOnly(GameObject target)
    {
        foreach (var panel in allPanels)
        {
            if (panel != null)
                panel.SetActive(panel == target);
        }
    }
}