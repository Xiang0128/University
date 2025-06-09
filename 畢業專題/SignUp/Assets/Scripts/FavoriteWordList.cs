using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.SceneManagement;

[System.Serializable]
public class collWord
{
    public int word_id;
    public string word_text;
    public string model_name;
}

[System.Serializable]
public class WordList
{
    public List<collWord> words;
}

public class FavoriteWordList : MonoBehaviour
{
    public GameObject wordPrefab;
    public Transform contentParent;

    void OnEnable()
    {
        StartCoroutine(LoadFavoriteWords());
    }

    IEnumerator LoadFavoriteWords()
    {
        // 清除原本的內容
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        string url = $"{GameData.baseUrl}/get_favorite_words/{GameData.userId}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("載入收藏單字失敗: " + request.error);
            yield break;
        }

        string json = request.downloadHandler.text;
        Debug.Log("收到 JSON: " + json);

        WordList wordList = JsonUtility.FromJson<WordList>(json);

        foreach (collWord word in wordList.words)
        {
            GameObject wordGO = Instantiate(wordPrefab, contentParent);
            wordGO.SetActive(true);

            Text wordText = wordGO.transform.Find("WordText").GetComponent<Text>();
            wordText.text = word.word_text;

            Button favButton = wordGO.transform.Find("collButton").GetComponent<Button>();
            int capturedId = word.word_id;  // 避免閉包問題
            favButton.onClick.AddListener(() => StartCoroutine(UnfavoriteWord(capturedId, wordGO)));

            Button playBtn = wordGO.transform.Find("palyButton").GetComponent<Button>();
            playBtn.onClick.AddListener(() =>
            {
                Debug.Log("播放單字動畫：" + word.word_text + " / " + word.model_name);
                PlayerPrefs.SetString("SelectedAnimation", word.model_name);
                PlayerPrefs.Save();
                GameData.targetPanelName = "CollPanel";
                SceneManager.LoadScene("ModelView");  // 切換到模型展示場景
            });
        }
    }

    IEnumerator UnfavoriteWord(int wordId, GameObject wordGO)
    {
        string url = GameData.unfavoriteWordUrl;

        // 手動序列化成 JSON
        string jsonData = $"{{\"user_id\":{GameData.userId}, \"word_id\": {wordId}}}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("取消收藏失敗: " + request.error);
            yield break;
        }

        Debug.Log("取消收藏成功: " + wordId);

        // 直接刪掉該物件
        Destroy(wordGO);
    }
}
