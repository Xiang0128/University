using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics; // Add at the top 
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
[System.Serializable]
public class QuizItem
{
    public int word_id;
    public string word;
    public string clip_name;
}

[System.Serializable]
public class QuizItemList
{
    public List<QuizItem> items;
}

[System.Serializable]
public class TestHistoryData
{
    public int user_id;
    public int test_score;
    public int course_id;
    public List<int> wrong_word_ids;
    

}

public class TestManager : MonoBehaviour
{
    [Header("Server")]
    public string quizDataURL = $"{GameData.baseUrl}/api/quiz_words/{GameData.selectedCourseId}";

    [Header("UI Elements")]
    public Canvas canvas;
    public Font uiFont;
    public Slider progressBar;
    public Button nextQuestionButton;
    public GameObject answerButtonPrefab;
    public GameObject startDetectionButtonPrefab;
    public GameObject textPrefab;
    public Image progressBarFill;
    public GameObject ProgressBar;
    [Header("3D Model")]
    public GameObject animatedModelPrefab;

    private Dictionary<string, string> wordToAnimation = new Dictionary<string, string>();
    private List<string> words = new List<string>();

    private Text questionText;
    private List<Button> answerButtons = new List<Button>();
    private Button startDetectionButton;
    private GameObject currentModelInstance;
    private Animator currentAnimator;

    private string currentAnswer;
    private int currentQuestionIndex = 0;
    private bool isDIYQuestion = false;

    public RawImage cameraPreviewUI; // Drag the RawImage in Inspector
    public RawImage Outer;

    private List<string> wrongAnswers = new List<string>();
    private int correctCount = 0;
    public GameObject resultPanel;
    public Text resultSummaryText;
    public Text wrongWordsText;
    public Button leaveButton;
    public Transform wrongWordsContainer;  // Drag your layout container in Inspector

    List<string> recognitionWrongAnswers = new List<string>();
    List<string> selectionWrongAnswers = new List<string>();
    private Dictionary<string, int> wordTextToId = new Dictionary<string, int>();



    void Start()
    {
        quizDataURL = $"{GameData.baseUrl}/api/quiz_words/{GameData.selectedCourseId}";
        ListCameras();
        StartCoroutine(FetchQuizData());
        resultPanel.SetActive(false);
        leaveButton.onClick.AddListener(() => GoToHomeUI());
        

    }
    IEnumerator FetchQuizData()
    {
        UnityWebRequest request = UnityWebRequest.Get(quizDataURL);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            UnityEngine.Debug.LogError("Failed to get quiz data: " + request.error);
            yield break;
        }

        string json = request.downloadHandler.text;
        QuizItemList quizList = JsonUtility.FromJson<QuizItemList>(json);

        foreach (var item in quizList.items)
        {
            if (!wordToAnimation.ContainsKey(item.word))
            {
                wordToAnimation[item.word] = item.clip_name;
                words.Add(item.word);
            }
            wordTextToId[item.word] = item.word_id; // ✅ 存到類別層級的變數中
        }

        Shuffle(words);
        SetupUI();
        SetupQuestion();
        progressBar.maxValue = words.Count;
        progressBar.value = 0;
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = i + Random.Range(0, list.Count - i);
            T tmp = list[r];
            list[r] = list[i];
            list[i] = tmp;
        }
    }

    void SetupUI()
    {
        // Create background GameObject with Image
        GameObject bgObj = new GameObject("TextBackground", typeof(RectTransform), typeof(Image));
        bgObj.transform.SetParent(canvas.transform, false);
        Image bgImage = bgObj.GetComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.1f, 1f); // Light blue with alpha

        RectTransform bgRect = bgObj.GetComponent<RectTransform>();
        bgRect.sizeDelta = new Vector2(600, 100);
        bgRect.anchoredPosition = new Vector2(0, 280);

        // Instantiate text inside background
        GameObject qText = Instantiate(textPrefab, bgObj.transform);
        questionText = qText.GetComponent<Text>();
        questionText.font = uiFont;
        questionText.alignment = TextAnchor.MiddleCenter;
        questionText.fontSize = 32;

        RectTransform textRect = questionText.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(600, 100);
        textRect.anchoredPosition = Vector2.zero;

        // Answer buttons
        for (int i = 0; i < 4; i++)
        {
            GameObject btnObj = Instantiate(answerButtonPrefab, canvas.transform);
            Button btn = btnObj.GetComponent<Button>();
            btn.GetComponentInChildren<Text>().font = uiFont;

            float xPos = 375 - (i * 250); // manually position each button
            btn.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, -350);

            btn.gameObject.SetActive(false);
            answerButtons.Add(btn);
        }

        // Start detection button
        GameObject detectBtnObj = Instantiate(startDetectionButtonPrefab, canvas.transform);
        startDetectionButton = detectBtnObj.GetComponent<Button>();
        startDetectionButton.gameObject.SetActive(false);
        nextQuestionButton.gameObject.SetActive(false);
    }
    IEnumerator StartCameraAndDetect()
    {
        string selectedName = GameData.selectedCameraName;

        if (string.IsNullOrEmpty(selectedName))
        {
            UnityEngine.Debug.LogWarning("No camera selected in GameData. Using default camera.");
            selectedName = WebCamTexture.devices.Length > 0 ? WebCamTexture.devices[0].name : null;
        }

        if (string.IsNullOrEmpty(selectedName))
        {
            UnityEngine.Debug.LogError("No available camera found.");
            yield break;
        }

        UnityEngine.Debug.Log("Selected Camera: " + selectedName);

        // ? Use global webcam texture
        if (GameData.sharedWebcamTexture == null)
        {
            GameData.sharedWebcamTexture = new WebCamTexture(selectedName);
        }

        if (!GameData.sharedWebcamTexture.isPlaying)
        {
            GameData.sharedWebcamTexture.Play();
            yield return new WaitForSeconds(0.5f); // small wait to let it start
        }

        cameraPreviewUI.texture = GameData.sharedWebcamTexture;

        if (!GameData.sharedWebcamTexture.isPlaying)
        {
            UnityEngine.Debug.LogError("Webcam failed to start!");
            yield break;
        }

        UnityEngine.Debug.Log("Webcam started.");
        yield return StartCoroutine(SendImageToServerAndCheckAnswer());
    }


    void SetupQuestion()
    {
        if (words == null || words.Count == 0)
        {
            UnityEngine.Debug.LogError("No quiz words loaded.");
            questionText.text = "?? No words found. Please check server data.";
            return;
        }

        if (currentQuestionIndex >= words.Count)
        {
            UnityEngine.Debug.LogError("Question index out of range.");
            questionText.text = "?? No more questions.";
            return;
        }

        isDIYQuestion = (Random.value > 0.5f);
        currentAnswer = words[currentQuestionIndex];

        if (currentModelInstance) Destroy(currentModelInstance);

        if (isDIYQuestion)
        {
            questionText.text = "請打開鏡頭比出對應動作: " + currentAnswer;
            foreach (var btn in answerButtons) btn.gameObject.SetActive(false);

            // 顯示相機預覽和進度條
            Outer.gameObject.SetActive(true);
            ProgressBar.gameObject.SetActive(true);
            progressBarFill.gameObject.SetActive(true);
            progressBarFill.fillAmount = 0f;
            startDetectionButton.gameObject.SetActive(true);

            // 顯示下一題按鈕用於跳過
            nextQuestionButton.gameObject.SetActive(true);

            // 設定開始辨識按鈕
            startDetectionButton.onClick.RemoveAllListeners();
            startDetectionButton.onClick.AddListener(() =>
            {
                StartCoroutine(StartCameraAndDetect());
            });

            // 設定下一題按鈕（跳過功能）
            nextQuestionButton.onClick.RemoveAllListeners();
            nextQuestionButton.onClick.AddListener(() => SkipRecognitionQuestion());
        }
        else
        {
            // 隱藏相機預覽和進度條
            ProgressBar.gameObject.SetActive(false);
            Outer.gameObject.SetActive(false);
            progressBarFill.gameObject.SetActive(false);
            startDetectionButton.gameObject.SetActive(false);
            nextQuestionButton.gameObject.SetActive(false);

            questionText.text = "請依據動作選出正確單字:";

            currentModelInstance = Instantiate(animatedModelPrefab, Vector3.zero, Quaternion.identity);
            currentAnimator = currentModelInstance.GetComponent<Animator>();
            currentAnimator.Play(wordToAnimation[currentAnswer]);

            List<string> options = GetRandomOptionsWithCorrectAnswer(words, currentAnswer);
            if (options.Count < 4)
            {
                UnityEngine.Debug.LogWarning("Not enough options generated.");
            }

            for (int i = 0; i < answerButtons.Count; i++)
            {
                answerButtons[i].gameObject.SetActive(true);
                string option = options[i];
                answerButtons[i].GetComponentInChildren<Text>().text = option;

                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => AnswerSelected(option));
            }
        }
    }

    public void GoToHomeUI()
    {
        if (GameData.sharedWebcamTexture != null && GameData.sharedWebcamTexture.isPlaying)
            GameData.sharedWebcamTexture.Stop();

        GameData.sharedWebcamTexture = null;

        GameData.targetPanelName = "TestMainPanel";
        SceneManager.LoadScene("HomeUI");
    }

    private Coroutine detectionCoroutine;

    public void StartRecognition()
    {
        if (detectionCoroutine == null)
        {
            detectionCoroutine = StartCoroutine(SendImageToServerAndCheckAnswer());
        }
    }

    private void OnRecognitionSucceeded()
    {
        questionText.text = "辨識題答對了！";
        cameraPreviewUI.texture = null;
        Resources.UnloadUnusedAssets();
        detectionCoroutine = null;
        StartCoroutine(DelayBeforeNextQuestion());
    }

    private void OnRecognitionFailed()
    {
        questionText.text = "辨識失敗，加油~";
        cameraPreviewUI.texture = null;
        Resources.UnloadUnusedAssets();
        detectionCoroutine = null;
        StartCoroutine(DelayBeforeNextQuestion());
        wrongAnswers.Add(currentAnswer);
        recognitionWrongAnswers.Add(currentAnswer);
    }

    private bool skipRequested = false;

    public void SkipRecognitionQuestion()
    {
        UnityEngine.Debug.Log("跳過辨識題：" + currentAnswer);

        // 停止檢測協程
        if (detectionCoroutine != null)
        {
            skipRequested = true;
            StopCoroutine(detectionCoroutine);
            detectionCoroutine = null;
        }

        // 停止攝影機
        if (GameData.sharedWebcamTexture != null && GameData.sharedWebcamTexture.isPlaying)
        {
            GameData.sharedWebcamTexture.Stop();
            GameData.sharedWebcamTexture = null;
            cameraPreviewUI.texture = null;
        }

        // 將跳過的題目記錄為錯題
        wrongAnswers.Add("辨識題" + currentAnswer);
        recognitionWrongAnswers.Add(currentAnswer);

        questionText.text = "已跳過辨識題：" + currentAnswer;

        // 重置跳過標記
        skipRequested = false;

        // 延遲後進入下一題
        StartCoroutine(DelayBeforeNextQuestion());
    }

    private IEnumerator SendImageToServerAndCheckAnswer()
    {
        WebCamTexture webcam = GameData.sharedWebcamTexture;
        if (webcam == null || !webcam.isPlaying)
        {
            questionText.text = "攝影機尚未啟動";
            detectionCoroutine = null;
            yield break;
        }

        int correctCounter = 0;
        int requiredCount = 2;
        bool isCorrect = false;

        string originalQuestionText = questionText.text;
        string lastDetected = "";

        while (!isCorrect && !skipRequested)
        {
            yield return new WaitForSeconds(0.1f);

            Texture2D screenshot = new Texture2D(webcam.width, webcam.height);
            screenshot.SetPixels(webcam.GetPixels());
            screenshot.Apply();

            byte[] imageBytes = screenshot.EncodeToJPG();

            WWWForm form = new WWWForm();
            form.AddBinaryData("image", imageBytes, "frame.jpg", "image/jpeg");

            using (UnityWebRequest request = UnityWebRequest.Post(GameData.predictUrl, form))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string detectedGesture = request.downloadHandler.text.Trim();
                    UnityEngine.Debug.Log("Detected: " + detectedGesture);

                    if (detectedGesture != lastDetected)
                    {
                        lastDetected = detectedGesture;
                        questionText.text = originalQuestionText + "\n辨識結果：" + detectedGesture;
                    }

                    if (detectedGesture == currentAnswer)
                    {
                        correctCounter++;
                        progressBarFill.fillAmount = (float)correctCounter / requiredCount;

                        if (correctCounter >= requiredCount)
                        {
                            isCorrect = true;
                            break;
                        }
                    }
                }
                else
                {
                    UnityEngine.Debug.LogError("伺服器錯誤: " + request.error);
                }
            }
        }

        detectionCoroutine = null;

        // 如果是被跳過，不需要處理結果
        if (skipRequested)
        {
            skipRequested = false;
            yield break;
        }

        // 停止攝影機
        if (GameData.sharedWebcamTexture != null && GameData.sharedWebcamTexture.isPlaying)
        {
            GameData.sharedWebcamTexture.Stop();
            GameData.sharedWebcamTexture = null;
            cameraPreviewUI.texture = null;
        }

        if (isCorrect)
        {
            questionText.text = "辨識題答對了！";
            correctCount++;
        }
        else
        {
            questionText.text = "辨識題答錯了";
            wrongAnswers.Add("辨識題" + currentAnswer);
            recognitionWrongAnswers.Add(currentAnswer);
        }

        StartCoroutine(DelayBeforeNextQuestion());
    }

    void ListCameras()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++)
        {
            UnityEngine.Debug.Log($"Camera {i}: {devices[i].name}");
        }
    }

    void AnswerSelected(string selected)  // 選擇題
    {
        if (selected == currentAnswer)
        {
            questionText.text = "選擇題答對了！";
            correctCount++;
            StartCoroutine(DelayBeforeNextQuestion());
        }
        else
        {
            questionText.text = $"選擇題答錯了！正確答案是：{currentAnswer}";
            wrongAnswers.Add("選擇題" + currentAnswer);
            selectionWrongAnswers.Add(currentAnswer);
            StartCoroutine(DelayBeforeNextQuestion());
        }
    }

    IEnumerator DelayBeforeNextQuestion()//等一下
    {
        yield return new WaitForSeconds(1f); // 等 1 秒
        NextQuestion();
    }

    void NextQuestion()
    {
        // 確保攝影機已停止
        if (GameData.sharedWebcamTexture != null && GameData.sharedWebcamTexture.isPlaying)
        {
            GameData.sharedWebcamTexture.Stop();
            GameData.sharedWebcamTexture = null;
            cameraPreviewUI.texture = null;
        }

        currentQuestionIndex++;
        if (currentQuestionIndex >= words.Count)
        {
            questionText.text = "~~~~ 練習結束!~~~~";
            ShowResultPanel();

            // 隱藏所有答案按鈕和其他UI元素
            foreach (var btn in answerButtons)
            {
                btn.gameObject.SetActive(false);
            }
            startDetectionButton.gameObject.SetActive(false);
            nextQuestionButton.gameObject.SetActive(false);
            Outer.gameObject.SetActive(false);
            ProgressBar.gameObject.SetActive(false);
            progressBarFill.gameObject.SetActive(false);

            return;
        }

        SetupQuestion();
        progressBar.value = currentQuestionIndex;
    }

    void ShowResultPanel()
    {
        resultPanel.SetActive(true);
        resultSummaryText.text = $"你答對了 {correctCount} / {words.Count} 題！";
        List<int> wrongWordIds = new List<int>();
        foreach (string word in recognitionWrongAnswers.Concat(selectionWrongAnswers))
        {
            if (wordTextToId.ContainsKey(word))
            {
                int id = wordTextToId[word];
                wrongWordIds.Add(id);
            }
        }

        StartCoroutine(SendTestHistory(correctCount, words.Count, wrongWordIds)); //存入資料庫
        // 清除舊的按鈕
        foreach (Transform child in wrongWordsContainer)
        {
            Destroy(child.gameObject);
        }

        if (wrongAnswers.Count == 0)
        {
            wrongWordsText.text = "全部答對！超棒！";
        }
        else
        {
            wrongWordsText.text = "錯誤的單字：";

            if (recognitionWrongAnswers.Count > 0)
            {
                GameObject title1 = Instantiate(answerButtonPrefab, wrongWordsContainer);
                title1.GetComponentInChildren<Text>().text = "辨識題";
                title1.GetComponent<Button>().interactable = false;

                foreach (string word in recognitionWrongAnswers)
                {
                    CreateWrongWordButton(word);
                }
            }

            if (selectionWrongAnswers.Count > 0)
            {
                GameObject title2 = Instantiate(answerButtonPrefab, wrongWordsContainer);
                title2.GetComponentInChildren<Text>().text = "選擇題";
                title2.GetComponent<Button>().interactable = false;

                foreach (string word in selectionWrongAnswers)
                {
                    CreateWrongWordButton(word);
                }
            }
        }
    }

    void CreateWrongWordButton(string word)
    {
        GameObject btnObj = Instantiate(answerButtonPrefab, wrongWordsContainer);
        Button btn = btnObj.GetComponent<Button>();
        Text btnText = btn.GetComponentInChildren<Text>();
        btnText.text = word;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            ReplayWrongWord(word);
        });
    }

    void ReplayWrongWord(string word)
    {
        UnityEngine.Debug.Log("Replaying: " + word);

        if (currentModelInstance) Destroy(currentModelInstance);

        questionText.text = "復習單字：" + word;
        Outer.gameObject.SetActive(false);
        progressBarFill.gameObject.SetActive(false);
        startDetectionButton.gameObject.SetActive(false);

        currentModelInstance = Instantiate(animatedModelPrefab, Vector3.zero, Quaternion.identity);
        currentAnimator = currentModelInstance.GetComponent<Animator>();
        currentAnimator.Play(wordToAnimation[word]);
    }

    List<string> GetRandomOptionsWithCorrectAnswer(List<string> all, string correct)
    {
        List<string> options = new List<string>(all.Where(w => w != correct));
        options = options.OrderBy(x => Random.value).Take(3).ToList();
        options.Add(correct);
        return options.OrderBy(x => Random.value).ToList();
    }

    IEnumerator SendTestHistory(int correctCount, int totalQuestions, List<int> wrongWordIds)
    {
        int score = totalQuestions-correctCount;

        TestHistoryData data = new TestHistoryData
        {
            user_id = GameData.userId,
            test_score = score,
            course_id = GameData.selectedCourseId,
            wrong_word_ids = wrongWordIds
        };

        string json = JsonUtility.ToJson(data);
        UnityEngine.Debug.Log("送出的 JSON：" + json);
        UnityWebRequest request = new UnityWebRequest(GameData.saveWrongAnswersUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            UnityEngine.Debug.Log("測驗結果送出成功！");
        else
            UnityEngine.Debug.LogError("錯誤：" + request.error);

    }


}