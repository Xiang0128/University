using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NewWordButtonGenerator : MonoBehaviour
{
    public GameObject wordShowPrefab;
    public Transform contentParent;
    public Text courseTitleText;

    public Sprite favoritedSprite;
    public Sprite unfavoritedSprite;

    private string serverURL = GameData.baseUrl;

    [System.Serializable]
    public class Word
    {
        public int model_id;
        public string word_text;
        public string model_name;
    }

    [System.Serializable]
    public class WordList
    {
        public List<Word> words;
    }

    public class FavResult
    {
        public string favorited;
    }

    private void OnEnable()
    {
        courseTitleText.text = "新單字列表";

        // 清除舊的顯示
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // 只抓第 0 課單字
        StartCoroutine(FetchWords(0));
    }

    IEnumerator FetchWords(int courseId)
    {
        string url = $"{serverURL}/get_words/{courseId}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                WordList wordList = JsonUtility.FromJson<WordList>(request.downloadHandler.text);
                GenerateWordItems(wordList.words);
            }
            else
            {
                Debug.LogError("獲取單字失敗：" + request.error);
            }
        }
    }

    void GenerateWordItems(List<Word> words)
    {
        foreach (Word word in words)
        {
            GameObject item = Instantiate(wordShowPrefab, contentParent);
            item.SetActive(true);

            Text wordText = item.transform.Find("WordText").GetComponent<Text>();
            wordText.text = word.word_text;

            Button collBtn = item.transform.Find("collButton").GetComponent<Button>();
            Image collImg = collBtn.GetComponent<Image>();

            StartCoroutine(UpdateFavoriteIcon(GameData.userId, word.model_id, collImg));

            collBtn.onClick.AddListener(() =>
            {
                StartCoroutine(ToggleFavorite(word.model_id, collImg));
            });

            Button playBtn = item.transform.Find("playButton").GetComponent<Button>();
            playBtn.onClick.AddListener(() =>
            {
                PlayerPrefs.SetString("SelectedAnimation", word.model_name);
                PlayerPrefs.SetString("SelectedWord", word.word_text);
                PlayerPrefs.Save();

                GameData.targetPanelName = "LearnWordPanel";
                SceneManager.LoadScene("ModelView");
            });
        }
    }

    IEnumerator UpdateFavoriteIcon(int userId, int modelId, Image iconImage)
    {
        string url = $"{serverURL}/is_favorited/{userId}/{modelId}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                bool isFavorited = request.downloadHandler.text.Contains("true");
                iconImage.sprite = isFavorited ? favoritedSprite : unfavoritedSprite;
            }
            else
            {
                Debug.LogError("檢查收藏狀態失敗：" + request.error);
            }
        }
    }

    IEnumerator ToggleFavorite(int modelId, Image iconImage)
    {
        string checkUrl = $"{serverURL}/is_favorited/{GameData.userId}/{modelId}";
        UnityWebRequest checkRequest = UnityWebRequest.Get(checkUrl);
        yield return checkRequest.SendWebRequest();

        if (checkRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("檢查收藏狀態失敗：" + checkRequest.error);
            yield break;
        }

        bool isFavorited = JsonUtility.FromJson<FavResult>(checkRequest.downloadHandler.text).favorited == "true";

        string url = isFavorited
            ? $"{serverURL}/unfavorite_word_from_word"
            : $"{serverURL}/favorite_word";

        string jsonData = JsonUtility.ToJson(new FavoriteData
        {
            user_id = GameData.userId,
            model_id = modelId
        });

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            iconImage.sprite = isFavorited ? unfavoritedSprite : favoritedSprite;
        }
        else
        {
            Debug.LogError("切換收藏失敗：" + request.error);
        }
    }

    [System.Serializable]
    public class FavoriteData
    {
        public int user_id;
        public int model_id;
    }
}
