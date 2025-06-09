using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.XR;

public class RecordManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject recordItemPrefab;       // ���� prefab (�Ω�������)
    public Transform content;                 // ScrollView �� Content
    public GameObject answerButtonPrefab;     // ���~��r���s prefab
    public GameObject resultPanel;            // ��ܴ��絲�G�����O
    public Text wrongWordsText;               // ��ܿ��~��r���D
    public Transform wrongWordsContainer;     // ���~��r���s��m�ϰ�

    [System.Serializable]
    public class QuizRecord
    {
        public int test_id;
        public string quizName;
        public int course_id;
        public int correctCount;
        public string dateTime;
    }

    [System.Serializable]
    public class QuizRecordList
    {
        public List<QuizRecord> records;
    }

    [System.Serializable]
    public class WrongWord
    {
        public string word_text;
        public string model_name;
    }

    [System.Serializable]
    public class WrongWordList
    {
        public List<WrongWord> wrong_words;
    }
    void OnEnable()
    {
        StartCoroutine(GetUserQuizRecords());
    }

    // ���o�ϥΪ̪��Ҧ��������
    IEnumerator GetUserQuizRecords()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        string url = $"{GameData.getUserQuizRecordsUrl}?user_id={GameData.userId}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
        if (request.result != UnityWebRequest.Result.Success)
#else
        if (request.isNetworkError || request.isHttpError)
#endif
        {
            Debug.LogError("API ���~: " + request.error);
            yield break;
        }

        // �ʸ˦� JSON ���c��K�ѪR
        string json = "{\"records\":" + request.downloadHandler.text + "}";
        QuizRecordList data = JsonUtility.FromJson<QuizRecordList>(json);

        foreach (var record in data.records)
        {
            GameObject item = Instantiate(recordItemPrefab, content);
            Transform quizNameText = item.transform.Find("QuizNameText");
            Transform correctCountText = item.transform.Find("CorrectCountText");
            Transform timeText = item.transform.Find("TimeText");
           
            if (quizNameText && correctCountText && timeText)
            {
                quizNameText.GetComponent<Text>().text = record.quizName;
                correctCountText.GetComponent<Text>().text = "�����D�ơG" + record.correctCount;
                timeText.GetComponent<Text>().text = "�ɶ��G" + record.dateTime;
            }
            else
            {
                Debug.LogError("�䤣�� RecordItem �̪���r����A���ˬd����W�٬O�_���T�I");
            }



            // �I�����ܸӦ����絲�G
            int thisTestId = record.test_id;
            item.GetComponent<Button>().onClick.AddListener(() => ShowResultPanel(thisTestId));
        }
    }

    //  ��ܿ�ܪ����絲�G
    void ShowResultPanel(int testId)
    {
        resultPanel.SetActive(true);
        wrongWordsText.text = "���~����r�G";

        // �M���ª����~��r���s
        foreach (Transform child in wrongWordsContainer)
        {
            Destroy(child.gameObject);
        }

        StartCoroutine(GetWrongWords(testId));
    }

    // �ھ� TestID ���o���~��r�M��
    IEnumerator GetWrongWords(int testId)
    {
        string url = $"{GameData.getWrongWordsUrl}?test_id={testId}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("���o���~��r���ѡG" + request.error);
            yield break;
        }

        WrongWordList wrongList = JsonUtility.FromJson<WrongWordList>(request.downloadHandler.text);
        Debug.Log("���~��r JSON: " + request.downloadHandler.text);

        if (wrongList.wrong_words.Count == 0)
        {
            wrongWordsText.text = "��������I�W�ΡI";
            yield break;
        }

        foreach (WrongWord word in wrongList.wrong_words)
        {
            CreateWrongWordButton(word, testId);
        }
    }

    // ���Ϳ��~��r���s
    void CreateWrongWordButton(WrongWord word,int testId)
    {
        GameObject btnObj = Instantiate(answerButtonPrefab, wrongWordsContainer);
        Button btn = btnObj.GetComponent<Button>();
        Text btnText = btn.GetComponentInChildren<Text>();

        if (btnText == null)
        {
            Debug.LogError("�䤣�� Text ����I�нT�{ prefab ���c");
            return;
        }

        btnText.text = word.word_text;
        Debug.Log("�]�w���s��r��: " + word.word_text);

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => { ReplayWrongWord(word, testId); });
    }


    // Replay ��r�ʵe
    void ReplayWrongWord(WrongWord word, int testId)
    {
        GameData.targetPanelName = "HisresultPanel";
        GameData.selectedTestId = testId;
        PlayerPrefs.SetInt("SelectedTestId", testId);
        PlayerPrefs.Save();

        PlayerPrefs.SetString("SelectedAnimation", word.model_name);
        PlayerPrefs.SetString("SelectedWord", word.word_text);
        PlayerPrefs.Save();

        UnityEngine.SceneManagement.SceneManager.LoadScene("ModelView");
    }


}

