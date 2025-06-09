using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using static CourseButtonGenerator;

[System.Serializable]
public class Sentence
{
    public string sign_grammar;
    public string chinese_grammar;
    public int sentence_order;
}

[System.Serializable]
public class SentenceResponse
{
    public string status;
    public List<Sentence> sentences;
}

[System.Serializable]
public class WordDataItem
{
    public int model_id;
    public string word_text;
    public string model_name;
}

[System.Serializable]
public class WordWrapper
{
    public List<WordDataItem> words;
}

public class LessonTextController : MonoBehaviour
{
    public Transform sentenceContainer;
    public GameObject signButtonPrefab;
    public Text courseTitleText;
    public GameObject sentenceRowPrefab;
    

    public int courseId;

    private List<WordDataItem> loadedWords = new List<WordDataItem>();

    void OnEnable()
    {
        courseId = GameData.selectedCourseId;
        if (courseTitleText != null)
        {
            courseTitleText.text = $"第{GameData.selectedCourseId}課 {GameData.selectedCourseName}";
        }
        StartCoroutine(GetWordsFromServer(courseId));
    }

    IEnumerator GetWordsFromServer(int courseId)
    {
        string url = $"{GameData.baseUrl}/get_words/{courseId}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("連線失敗: " + request.error);
            yield break;
        }

        string rawJson = request.downloadHandler.text;

        // Debug 回傳內容
        Debug.Log("Words JSON 原始資料: " + rawJson);

        if (!rawJson.Trim().StartsWith("{"))
        {
            rawJson = "{\"words\":" + rawJson + "}";
        }

        WordWrapper wrapper = JsonUtility.FromJson<WordWrapper>(rawJson);
        loadedWords = wrapper.words;

        Debug.Log("成功載入單字數量：" + loadedWords.Count);

        StartCoroutine(LoadSentences(courseId)); // 等單字載入後再載句子
    }


    IEnumerator LoadSentences(int courseId)
    {
        string url = $"{GameData.baseUrl}/get_sentences/{courseId}";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error loading sentences: " + www.error);
        }
        else
        {
            SentenceResponse response = JsonUtility.FromJson<SentenceResponse>(www.downloadHandler.text);

            foreach (Transform child in sentenceContainer)
                Destroy(child.gameObject);
            foreach (var w in loadedWords)
            {
                Debug.Log($"Word in list: [{w.word_text}] (Length: {w.word_text.Length})");
            }

            foreach (Sentence s in response.sentences)
            {
                GameObject row = Instantiate(sentenceRowPrefab, sentenceContainer);
                Transform signButtonContainer = row.transform.Find("SignButtonContainer");
                Text chineseText = row.GetComponentInChildren<Text>();

                string[] signWords = s.sign_grammar.Split(' ');
                List<string> animationList = new List<string>();
                List<string> modelNames = new List<string>();
                List<string> wordTexts = new List<string>();

                foreach (string word in signWords)
                {
                    string cleanWord = word.Trim().Normalize();

                    WordDataItem match = loadedWords.FirstOrDefault(w => w.word_text.Trim().Normalize() == cleanWord);
                    GameObject btn = Instantiate(signButtonPrefab, signButtonContainer);
                    Text btnText = btn.GetComponentInChildren<Text>();
                    btnText.text = cleanWord;

                    if (match != null)
                    {
                        animationList.Add(match.model_name); // 儲存 model_name
                        modelNames.Add(match.model_name);
                        wordTexts.Add(match.word_text);

                        string modelName = match.model_name;
                        string wordText = match.word_text;

                        btn.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            PlayerPrefs.SetString("SelectedAnimation", modelName);
                            PlayerPrefs.SetString("SelectedWord", wordText);
                            PlayerPrefs.Save();
                            GameData.targetPanelName = "LearnCoursePanel";
                            SceneManager.LoadScene("ModelView");
                        });
                    }
                    else
                    {
                        btn.GetComponent<Button>().interactable = false;
                    }
                }

                // 找到你手動加在 sentenceRowPrefab 裡的撥整句按鈕
                Transform playBtnTransform = row.transform.Find("TextArea/PlaySentenceButton");
                
                if (playBtnTransform != null)
                {
                    Button playBtn = playBtnTransform.GetComponent<Button>();
                    if (playBtn != null)
                    {
                        List<string> modelsToPlay = new List<string>(animationList); // 複製動畫列表
                        
                        playBtn.onClick.AddListener(() =>
                        {
                            if (modelsToPlay.Count > 0)
                            {
                                string data = string.Join(",", modelsToPlay);
                                PlayerPrefs.SetString("QueuedAnimations", data);
                                PlayerPrefs.SetString("QueuedAnimations", string.Join(",", modelNames));
                                PlayerPrefs.SetString("QueuedWords", string.Join(",", wordTexts));
                                PlayerPrefs.Save();
                                GameData.targetPanelName = "LearnCoursePanel";
                                SceneManager.LoadScene("ModelView");
                            }
                            else
                            {
                                Debug.LogWarning("找不到句子的任何動畫");
                            }
                        });
                    }
                    else
                    {
                        Debug.LogWarning("找不到按鈕組件：PlaySentenceButton");
                    }
                }
                else
                {
                    Debug.LogWarning("找不到物件：PlaySentenceButton");
                }

                if (chineseText != null)
                {
                    chineseText.text = s.chinese_grammar;
                }
            }


        }
    }
}
