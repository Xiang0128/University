using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CourseButtonGenerator : MonoBehaviour
{
    public GameObject buttonTemplate;
    public Transform contentParent;
    public PanelManager panelManager;
    public Text courseTitleText;

    private string serverURL = GameData.baseUrl;

    [System.Serializable]
    public class Course
    {
        public int course_id;
        public string course_name;
        public string course_content;
    }

    [System.Serializable]
    public class CourseList
    {
        public List<Course> courses;
    }

    public Course selectedCourse;

    public enum CallerSource
    {
        LearnMainPanel,
        TestMainPanel
  
    }
    public CallerSource callerSource = CallerSource.LearnMainPanel;

    void Start()
    {
        StartCoroutine(FetchCourses());
        Debug.Log("Start 呼叫了 LoadCoursesFromServer");
    }

    IEnumerator FetchCourses()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(serverURL + "/get_courses"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                CourseList courseList = JsonUtility.FromJson<CourseList>(request.downloadHandler.text);
                GenerateCourseButtons(courseList.courses);
            }
            else
            {
                Debug.LogError("獲取課程失敗：" + request.error);
            }
        }
    }

    void GenerateCourseButtons(List<Course> courses)
    {
        // 根據 course_id 升序排列
        courses.Sort((a, b) => a.course_id.CompareTo(b.course_id));

        foreach (Course course in courses)
        {
            if (course.course_id == 0)
                continue;

            GameObject newButton = Instantiate(buttonTemplate, contentParent);
            newButton.SetActive(true);

            Text btnText = newButton.GetComponentInChildren<Text>();
            if (btnText != null)
                btnText.text = $"第 {course.course_id} 課\n{course.course_name}";

            newButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                selectedCourse = course;
                GameData.selectedCourseId = course.course_id;
                GameData.selectedCourseName = course.course_name;

                Debug.Log($"你選擇了課程：第{course.course_id}課 {course.course_name} id={GameData.selectedCourseId}");
                Debug.Log($"內容：{course.course_content}");

                switch (callerSource)
                {
                    case CallerSource.LearnMainPanel:
                        if (panelManager != null)
                            panelManager.ShowPanel("LearnChoosePanel");
                        break;

                    case CallerSource.TestMainPanel:
                        Debug.Log("切換到 Test Panel 場景");
                        SceneManager.LoadScene("TestPanel");
                        GameData.targetPanelName = "TestPanel";
                        break;
                }
            });
        }
    }



}
