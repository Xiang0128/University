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
        // �M���쥻�����e
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        string url = $"{GameData.baseUrl}/get_favorite_words/{GameData.userId}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("���J���ó�r����: " + request.error);
            yield break;
        }

        string json = request.downloadHandler.text;
        Debug.Log("���� JSON: " + json);

        WordList wordList = JsonUtility.FromJson<WordList>(json);

        foreach (collWord word in wordList.words)
        {
            GameObject wordGO = Instantiate(wordPrefab, contentParent);
            wordGO.SetActive(true);

            Text wordText = wordGO.transform.Find("WordText").GetComponent<Text>();
            wordText.text = word.word_text;

            Button favButton = wordGO.transform.Find("collButton").GetComponent<Button>();
            int capturedId = word.word_id;  // �קK���]���D
            favButton.onClick.AddListener(() => StartCoroutine(UnfavoriteWord(capturedId, wordGO)));

            Button playBtn = wordGO.transform.Find("palyButton").GetComponent<Button>();
            playBtn.onClick.AddListener(() =>
            {
                Debug.Log("�����r�ʵe�G" + word.word_text + " / " + word.model_name);
                PlayerPrefs.SetString("SelectedAnimation", word.model_name);
                PlayerPrefs.Save();
                GameData.targetPanelName = "CollPanel";
                SceneManager.LoadScene("ModelView");  // ������ҫ��i�ܳ���
            });
        }
    }

    IEnumerator UnfavoriteWord(int wordId, GameObject wordGO)
    {
        string url = GameData.unfavoriteWordUrl;

        // ��ʧǦC�Ʀ� JSON
        string jsonData = $"{{\"user_id\":{GameData.userId}, \"word_id\": {wordId}}}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("�������å���: " + request.error);
            yield break;
        }

        Debug.Log("�������æ��\: " + wordId);

        // �����R���Ӫ���
        Destroy(wordGO);
    }
}
