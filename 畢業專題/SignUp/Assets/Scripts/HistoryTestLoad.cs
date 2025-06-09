using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class HistoryTestLoad : MonoBehaviour
{
    public Transform wrongWordsContainer;    // ���~��r���s��m�ϰ�
    public GameObject answerButtonPrefab;    // ���~��r���s prefab
    public Text wrongWordsTitle;              // ���~��r���D��r

    void Start()
    {
        if (GameData.selectedTestId != 0)
        {
            StartCoroutine(LoadWrongWords(GameData.selectedTestId));
        }
        else
        {
            Debug.LogWarning("selectedTestId �|���]�w");
            wrongWordsTitle.text = "�䤣�������";
        }
    }

    IEnumerator LoadWrongWords(int testId)
    {
        string url = $"{GameData.getWrongWordsUrl}?test_id={testId}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("���o���~��r���ѡG" + request.error);
            wrongWordsTitle.text = "���o���~��r����";
            yield break;
        }

        WrongWordList wrongList = JsonUtility.FromJson<WrongWordList>(request.downloadHandler.text);

        if (wrongList.wrong_words.Count == 0)
        {
            wrongWordsTitle.text = "��������I�W�ΡI";
            yield break;
        }

        wrongWordsTitle.text = "���~����r�G";

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
                Debug.LogError("�䤣�� Text ����I");
            }
            Button btn = btnObj.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => PlayAnimation(word));
            }
            else
            {
                Debug.LogError("�䤣�� Button ����I");
            }
        }
        void PlayAnimation(WrongWord word)
        {
            Debug.Log("����ʵe�A��r�G" + word.word_text + ", �ҫ��G" + word.model_name);
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
