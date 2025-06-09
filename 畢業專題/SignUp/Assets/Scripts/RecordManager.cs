using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.XR;

public class RecordManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject recordItemPrefab;       // 項目 prefab (用於測驗紀錄)
    public Transform content;                 // ScrollView 的 Content
    public GameObject answerButtonPrefab;     // 錯誤單字按鈕 prefab
    public GameObject resultPanel;            // 顯示測驗結果的面板
    public Text wrongWordsText;               // 顯示錯誤單字標題
    public Transform wrongWordsContainer;     // 錯誤單字按鈕放置區域

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

    // 取得使用者的所有測驗紀錄
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
            Debug.LogError("API 錯誤: " + request.error);
            yield break;
        }

        // 封裝成 JSON 結構方便解析
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
                correctCountText.GetComponent<Text>().text = "答錯題數：" + record.correctCount;
                timeText.GetComponent<Text>().text = "時間：" + record.dateTime;
            }
            else
            {
                Debug.LogError("找不到 RecordItem 裡的文字元件，請檢查物件名稱是否正確！");
            }



            // 點選後顯示該次測驗結果
            int thisTestId = record.test_id;
            item.GetComponent<Button>().onClick.AddListener(() => ShowResultPanel(thisTestId));
        }
    }

    //  顯示選擇的測驗結果
    void ShowResultPanel(int testId)
    {
        resultPanel.SetActive(true);
        wrongWordsText.text = "錯誤的單字：";

        // 清除舊的錯誤單字按鈕
        foreach (Transform child in wrongWordsContainer)
        {
            Destroy(child.gameObject);
        }

        StartCoroutine(GetWrongWords(testId));
    }

    // 根據 TestID 取得錯誤單字清單
    IEnumerator GetWrongWords(int testId)
    {
        string url = $"{GameData.getWrongWordsUrl}?test_id={testId}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("取得錯誤單字失敗：" + request.error);
            yield break;
        }

        WrongWordList wrongList = JsonUtility.FromJson<WrongWordList>(request.downloadHandler.text);
        Debug.Log("錯誤單字 JSON: " + request.downloadHandler.text);

        if (wrongList.wrong_words.Count == 0)
        {
            wrongWordsText.text = "全部答對！超棒！";
            yield break;
        }

        foreach (WrongWord word in wrongList.wrong_words)
        {
            CreateWrongWordButton(word, testId);
        }
    }

    // 產生錯誤單字按鈕
    void CreateWrongWordButton(WrongWord word,int testId)
    {
        GameObject btnObj = Instantiate(answerButtonPrefab, wrongWordsContainer);
        Button btn = btnObj.GetComponent<Button>();
        Text btnText = btn.GetComponentInChildren<Text>();

        if (btnText == null)
        {
            Debug.LogError("找不到 Text 元件！請確認 prefab 結構");
            return;
        }

        btnText.text = word.word_text;
        Debug.Log("設定按鈕文字為: " + word.word_text);

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => { ReplayWrongWord(word, testId); });
    }


    // Replay 單字動畫
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

