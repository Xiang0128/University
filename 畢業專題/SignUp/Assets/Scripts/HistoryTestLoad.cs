using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class HistoryTestLoad : MonoBehaviour
{
    public Transform wrongWordsContainer;    // 錯誤單字按鈕放置區域
    public GameObject answerButtonPrefab;    // 錯誤單字按鈕 prefab
    public Text wrongWordsTitle;              // 錯誤單字標題文字

    void Start()
    {
        if (GameData.selectedTestId != 0)
        {
            StartCoroutine(LoadWrongWords(GameData.selectedTestId));
        }
        else
        {
            Debug.LogWarning("selectedTestId 尚未設定");
            wrongWordsTitle.text = "找不到測驗資料";
        }
    }

    IEnumerator LoadWrongWords(int testId)
    {
        string url = $"{GameData.getWrongWordsUrl}?test_id={testId}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("取得錯誤單字失敗：" + request.error);
            wrongWordsTitle.text = "取得錯誤單字失敗";
            yield break;
        }

        WrongWordList wrongList = JsonUtility.FromJson<WrongWordList>(request.downloadHandler.text);

        if (wrongList.wrong_words.Count == 0)
        {
            wrongWordsTitle.text = "全部答對！超棒！";
            yield break;
        }

        wrongWordsTitle.text = "錯誤的單字：";

        foreach (Transform child in wrongWordsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var word in wrongList.wrong_words)
        {
            GameObject btnObj = Instantiate(answerButtonPrefab, wrongWordsContainer);
            Text btnText = btnObj.GetComponentInChildren<Text>();
            if (btnText != null)
            {
                btnText.text = word.word_text;
            }
            else
            {
                Debug.LogError("找不到 Text 元件！");
            }
            Button btn = btnObj.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => PlayAnimation(word));
            }
            else
            {
                Debug.LogError("找不到 Button 元件！");
            }
        }
        void PlayAnimation(WrongWord word)
        {
            Debug.Log("播放動畫，單字：" + word.word_text + ", 模型：" + word.model_name);
            GameData.targetPanelName = "HisresultPanel";
            PlayerPrefs.SetString("SelectedAnimation", word.model_name);
            PlayerPrefs.SetString("SelectedWord", word.word_text);
            PlayerPrefs.Save();

            UnityEngine.SceneManagement.SceneManager.LoadScene("ModelView");
        }
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
}
