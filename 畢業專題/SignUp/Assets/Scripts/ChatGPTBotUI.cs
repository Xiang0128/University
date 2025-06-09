using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Text;

public class LegacyChatBotUI : MonoBehaviour
{
    [Header("Play/Pause Images")]
    public Sprite playSprite;
    public Sprite pauseSprite;
    public Text detectionState;

    [Header("UI Elements")]
    public InputField userInput;
    public Button sendButton;
    public Button toggleRegisterButton;
    public RawImage cameraPreview;
    public GameObject chatMessagePrefab;
    public Transform chatContent;

    private string serverUrl = "https://sign.ngrok.pro/api/process_input";
    private string lastRegisteredWord = "";
    private string currentDetectedWord = "";
    private int correctCount = 0;
    private int requiredCount = 3;
    private bool isRegistering = true;

    [System.Serializable]
    public class ServerResponse
    {
        public string response;
    }

    [System.Serializable]
    public class InputPayload
    {
        public string user_input;
    }
    public Slider progressBar;

    void UpdateProgressBar()
    {
        progressBar.value = (float)correctCount / requiredCount;
    }

    private void OnEnable()
    {
        sendButton.onClick.AddListener(OnSendClicked);
        toggleRegisterButton.onClick.AddListener(ToggleRegistering);
        UpdateToggleButtonText();

        StartCoroutine(StartCameraAndDetectionLoop());
    }

    private void OnDisable()
    {
        sendButton.onClick.RemoveListener(OnSendClicked);
        toggleRegisterButton.onClick.RemoveListener(ToggleRegistering);
    }

    void OnSendClicked()
    {
        string input = userInput.text;
        if (string.IsNullOrWhiteSpace(input)) return;

        AddMessage("�A: " + input);
        userInput.text = "";
        StartCoroutine(SendInputToServer(input));
    }
    private string currentQuizWord = "";

    IEnumerator WaitForCorrectDetectionRoutine()
    {
        
        correctCount = 0;
        currentDetectedWord = "";

        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            string detected = currentDetectedWord;

            if (detected == currentQuizWord)
            {
                correctCount++;
                // Update progress bar here
            }
            else
            {
                correctCount = 0;
            }

            if (correctCount >= requiredCount)
            {
                AddMessage($"��y�����H: �ӴΤF�A�A���T�a��X�F {currentQuizWord}�I");

                // Send system result to backend to trigger Claude's next quiz
                string systemConfirm = $"[system] result: correct, word: {currentQuizWord}";
                StartCoroutine(SendInputToServer(systemConfirm));
                break;
            }

        }
    }


    void AddMessage(string msg)
    {
        GameObject message = Instantiate(chatMessagePrefab, chatContent);

        message.GetComponent<Text>().text = msg;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)chatContent);
    }

    IEnumerator SendInputToServer(string input)
    {
        InputPayload payload = new InputPayload { user_input = input };
        string jsonPayload = JsonUtility.ToJson(payload);

        using (UnityWebRequest request = new UnityWebRequest(serverUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string serverResponse = request.downloadHandler.text;
                Debug.Log("Server Response: " + serverResponse);

                // Deserialize the JSON response
                ServerResponse resObj = JsonUtility.FromJson<ServerResponse>(serverResponse);

                // Display the AI's response in the chat
                string aiResponse = resObj.response;
                AddMessage("��y�����H: " + aiResponse);

                // Check if there's a [target_word] line
                foreach (string line in aiResponse.Split('\n'))
                {
                    if (line.StartsWith("[target_word]"))
                    {
                        currentQuizWord = line.Replace("[target_word]", "").Trim();
                        Debug.Log("AI���w���ؼг�r: " + currentQuizWord);

                        // Begin checking for correct sign
                        StartCoroutine(WaitForCorrectDetectionRoutine());
                        break;
                    }
                }

            }
            else
            {
                Debug.LogError("Error sending input to server: " + request.error);
                AddMessage("��y�����H: �L�k�s������A��");
            }
        }
    }

    IEnumerator StartCameraAndDetectionLoop()
    {
        string selectedName = GameData.selectedCameraName;

        if (string.IsNullOrEmpty(selectedName))
        {
            Debug.LogWarning("�S����ܬ۾��A�ϥιw�]�۾�");
            selectedName = WebCamTexture.devices.Length > 0 ? WebCamTexture.devices[0].name : null;
        }

        if (string.IsNullOrEmpty(selectedName))
        {
            Debug.LogError("�S���i�Ϊ��۾�");
            yield break;
        }

        if (GameData.sharedWebcamTexture == null)
        {
            GameData.sharedWebcamTexture = new WebCamTexture(selectedName);
        }

        if (!GameData.sharedWebcamTexture.isPlaying)
        {
            GameData.sharedWebcamTexture.Play();
            yield return new WaitForSeconds(0.5f);
        }

        cameraPreview.texture = GameData.sharedWebcamTexture;

        if (!GameData.sharedWebcamTexture.isPlaying)
        {
            Debug.LogError("�۾��Ұʥ���!");
            yield break;
        }

        yield return StartCoroutine(DetectionLoop());
    }

    IEnumerator DetectionLoop()
    {
        WebCamTexture webcam = GameData.sharedWebcamTexture;

        while (true)
        {
            yield return new WaitForSeconds(0.033f);

            if (webcam == null || !webcam.isPlaying)
                continue;

            Texture2D frame = new Texture2D(webcam.width, webcam.height);
            frame.SetPixels(webcam.GetPixels());
            frame.Apply();

            byte[] imageBytes = frame.EncodeToJPG();

            WWWForm form = new WWWForm();
            form.AddBinaryData("image", imageBytes, "frame.jpg", "image/jpeg");

            using (UnityWebRequest request = UnityWebRequest.Post(GameData.predictUrl, form))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string detected = request.downloadHandler.text.Trim();

                    if (detected == currentDetectedWord)
                    {
                        correctCount++;
                    }
                    else
                    {
                        currentDetectedWord = detected;
                        correctCount = 1;
                    }

                    if (correctCount >= requiredCount)
                    {
                        if (isRegistering && detected != lastRegisteredWord && detected != "�L�k���ѥX�����" && detected != "Waiting")
                        {
                            userInput.text += detected + " ";
                            lastRegisteredWord = detected;
                        }
                        correctCount = 0;
                    }
                }
                else
                {
                    Debug.LogError("�������A�����~: " + request.error);
                }
            }
        }
    }

    void ToggleRegistering()
    {
        isRegistering = !isRegistering;
        UpdateToggleButtonText();
        Debug.Log(isRegistering ? "�}�l���U���" : "�Ȱ����U���");
    }

    void UpdateToggleButtonText()
    {
        Image btnImage = toggleRegisterButton.GetComponent<Image>();
        Text btnText = toggleRegisterButton.GetComponentInChildren<Text>();

        if (btnImage != null)
        {
            btnImage.sprite = isRegistering ? pauseSprite : playSprite;
            detectionState.text = isRegistering ? "���ѹB�椤" : "���ѼȰ�";
        }

        if (btnText != null)
        {
            btnText.text = ""; // Icon only
        }
    }
}
