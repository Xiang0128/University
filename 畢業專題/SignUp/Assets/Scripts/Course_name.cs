using UnityEngine;
using UnityEngine.UI;

public class CourseTitleLoader : MonoBehaviour
{
    public Text courseTitleText;

    void OnEnable()
    {
        LoadTitle();
    }

    public void LoadTitle()
    {
        if (courseTitleText != null)
        {
            // �q GameData Ū���襤���ҵ{��T
            int courseId = GameData.selectedCourseId;
            string courseName = GameData.selectedCourseName;

            courseTitleText.text = $"��{courseId}�� {courseName}";
        }
    }
}
