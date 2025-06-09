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
            // 從 GameData 讀取選中的課程資訊
            int courseId = GameData.selectedCourseId;
            string courseName = GameData.selectedCourseName;

            courseTitleText.text = $"第{courseId}課 {courseName}";
        }
    }
}
