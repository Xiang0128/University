using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ModelViewManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button playPauseButton;
    public Slider speedSlider;
    public Slider rotationSlider;
    public Button openDetectorButton;
    public Button backButton;
    public Text titleText;
    public GameObject outerPanel;
    public RawImage cameraPreviewUI; // ✅ Show webcam image
    public Image progressBarFill; // ✅ Fill bar
    public Text questionText;
    public Button closeButton;

    [Header("3D Model Components")]
    public Animator modelAnimator;
    public Transform modelTransform;

    private bool isPlaying = false;
    private float initialRotationY;

    private WebCamTexture webcamTexture;
    private string currentAnswer;
    private Coroutine detectionCoroutine;
    private Queue<string> animationQueue = new Queue<string>();
    private Queue<string> wordQueue = new Queue<string>(); // 對應 titleText 顯示的單字

    void Start()
    {
        playPauseButton.onClick.AddListener(TogglePlayPause);
        speedSlider.onValueChanged.AddListener(SetPlaybackSpeed);
        rotationSlider.onValueChanged.AddListener(RotateModel);
        openDetectorButton.onClick.AddListener(OpenDetector);
        closeButton.onClick.AddListener(CloseDetector);
        backButton.onClick.AddListener(() => BackToHomeUI());

        speedSlider.minValue = 0.5f;
        speedSlider.maxValue = 2.0f;
        speedSlider.value = 1.0f;

        rotationSlider.minValue = -180f;
        rotationSlider.maxValue = 180f;
        initialRotationY = modelTransform.eulerAngles.y;
        rotationSlider.value = initialRotationY;

        string animData = PlayerPrefs.GetString("QueuedAnimations", "");
        string wordData = PlayerPrefs.GetString("QueuedWords", "");

        if (!string.IsNullOrEmpty(animData) && !string.IsNullOrEmpty(wordData))
        {
            string[] anims = animData.Split(',');
            string[] words = wordData.Split(',');

            StartCoroutine(PlayAnimationsSequentially(anims, words));
        }
        else
        {
            PlaySelectedAnimation(); // 單個播放模式
        }
    }

    void TogglePlayPause()
    {
        isPlaying = !isPlaying;
        modelAnimator.speed = isPlaying ? speedSlider.value : 0;
    }

    void SetPlaybackSpeed(float speed)
    {
        if (isPlaying)
        {
            modelAnimator.speed = speed;
        }
    }

    void RotateModel(float value)
    {
        modelTransform.rotation = Quaternion.Euler(0, value, 0);
    }

    void OpenDetector()
    {
        outerPanel.SetActive(true);

        if (detectionCoroutine != null)
            StopCoroutine(detectionCoroutine);

        // ✅ Start camera if needed
        if (GameData.sharedWebcamTexture == null)
        {
            GameData.sharedWebcamTexture = new WebCamTexture(GameData.selectedCameraName);
        }

        if (!GameData.sharedWebcamTexture.isPlaying)
        {
            GameData.sharedWebcamTexture.Play();
        }

        cameraPreviewUI.texture = GameData.sharedWebcamTexture; // ✅ Show on UI

        detectionCoroutine = StartCoroutine(SendImageToServerAndCheckAnswer());
    }

    void CloseDetector()
    {
        outerPanel.SetActive(false);

        if (detectionCoroutine != null)
        {
            StopCoroutine(detectionCoroutine);
            detectionCoroutine = null;
        }

        // ✅ Don't stop the camera as it's shared across scenes
    }

    void PlaySelectedAnimation()
    {
        string selectedAnimation = PlayerPrefs.GetString("SelectedAnimation", "");
        string selectedWord = PlayerPrefs.GetString("SelectedWord", "");
        currentAnswer = selectedWord; // ✅ Save the correct answer

        if (!string.IsNullOrEmpty(selectedAnimation))
        {
            titleText.text = "單字: " + selectedWord;

            if (modelAnimator.HasState(0, Animator.StringToHash(selectedAnimation)))
            {
                modelAnimator.CrossFade(selectedAnimation, 0f); // Smooth transition with 0.2 seconds
                isPlaying = true;
                modelAnimator.speed = speedSlider.value;
            }
            else UnityEngine.Debug.LogError("動畫名稱不存在於 Animator 中: " + selectedAnimation);
        }
        else UnityEngine.Debug.LogError("未收到動畫名稱");
    }

    IEnumerator PlayAnimationsSequentially(string[] anims, string[] words)
    {
        for (int i = 0; i < anims.Length; i++)
        {
            string anim = anims[i];
            string word = i < words.Length ? words[i] : anim;

            if (modelAnimator.HasState(0, Animator.StringToHash(anim)))
            {
                titleText.text = "單字: " + word;
                modelAnimator.CrossFade(anim, 0.5f); // Smooth transition with 0.2 seconds

                yield return new WaitUntil(() =>
                    modelAnimator.GetCurrentAnimatorStateInfo(0).IsName(anim) &&
                    modelAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
                );
            }
            else
            {
                UnityEngine.Debug.LogError("Animator 中找不到動畫: " + anim);
            }
        }

        titleText.text = "播放結束";
        isPlaying = false;
        modelAnimator.speed = 0;

        PlayerPrefs.DeleteKey("QueuedAnimations");
        PlayerPrefs.DeleteKey("QueuedWords");
    }

    IEnumerator SendImageToServerAndCheckAnswer()
    {
        WebCamTexture webcam = GameData.sharedWebcamTexture;

        if (webcam == null || !webcam.isPlaying)
        {
            questionText.text = "⚠️ 攝影機尚未啟動";
            yield break;
        }

        float endTime = Time.time + 15f;
        int correctCounter = 0;
        int requiredCount = 3;
        bool isCorrect = false;

        while (Time.time < endTime)
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
                    UnityEngine.Debug.Log("[Detected] " + detectedGesture);

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

        questionText.text = isCorrect ? "答對了!" : "辨識失敗 再看看教學動畫吧!";
        CloseDetector(); // keep the camera running globally
    }

    void BackToHomeUI()
    {
        PlayerPrefs.DeleteKey("QueuedAnimations");
        PlayerPrefs.DeleteKey("QueuedWords");
        if (!string.IsNullOrEmpty(GameData.targetPanelName))
        {
            SceneManager.LoadScene("HomeUI");
        }
        else
        {
            UnityEngine.Debug.LogError("targetPanelName 尚未設定，無法導航！");
        }
    }
}