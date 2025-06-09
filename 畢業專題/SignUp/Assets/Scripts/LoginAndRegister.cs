using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class AuthUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject popupPanel; // �s�W���ܵ���
    public Text popupText;
    public Button popupCloseButton;

    [Header("Login UI Elements")]
    public InputField loginEmailInput;
    public InputField loginPasswordInput;
    public Button loginButton;
    public Button goToRegisterButton;

    [Header("Register UI Elements")]
    public InputField registerNameInput;
    public InputField registerEmailInput;
    public InputField registerPasswordInput;
    public Button registerButton;
    public Button goToLoginButton;

    private string serverURL = GameData.baseUrl;

    void Start()
    {
        ShowLoginPanel();

        loginButton.onClick.AddListener(() => StartCoroutine(LoginUser()));
        registerButton.onClick.AddListener(() => StartCoroutine(RegisterUser()));
        goToRegisterButton.onClick.AddListener(ShowRegisterPanel);
        goToLoginButton.onClick.AddListener(ShowLoginPanel);
        popupCloseButton.onClick.AddListener(() => popupPanel.SetActive(false)); 
    }

    public void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
    }

    public void ShowRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

    IEnumerator RegisterUser()
    {
        string userName = registerNameInput.text;
        string userEmail = registerEmailInput.text;
        string userPassword = registerPasswordInput.text;

        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userPassword))
        {
            ShowPopup("�ж�g�Ҧ����C");
            yield break;
        }

        if (!IsValidEmail(userEmail))
        {
            ShowPopup("�п�J���Ī� Email �榡�C");
            yield break;
        }

        if (!IsValidPassword(userPassword))
        {
            ShowPopup("�K�X�ݥ]�t�^��r���P�Ʀr�A�B���צܤ�6��C");
            yield break;
        }

        string jsonData = $"{{\"user_name\": \"{userName}\", \"user_email\": \"{userEmail}\", \"user_password\": \"{userPassword}\"}}";

        using (UnityWebRequest request = new UnityWebRequest(serverURL + "/register", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("User registered successfully!");

                // �M�ŵ��U���
                registerNameInput.text = "";
                registerEmailInput.text = "";
                registerPasswordInput.text = "";
                ShowPopup("���U���\�A�Ш�H�c���ұz���b���I");
                ShowLoginPanel();
            }

            else
            {
                ShowPopup("���U���ѡG" + request.downloadHandler.text);
            }
        }
    }

    IEnumerator LoginUser()
    {
        string userEmail = loginEmailInput.text;
        string userPassword = loginPasswordInput.text;

        if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userPassword))
        {
            ShowPopup("�ж�g�Ҧ����C");
            yield break;
        }

        if (!IsValidEmail(userEmail))
        {
            ShowPopup("�п�J���Ī� Email �榡�C");
            yield break;
        }

        if (!IsValidPassword(userPassword))
        {
            ShowPopup("�K�X�ݥ]�t�^��r���P�Ʀr�A�B���צܤ�6��C");
            yield break;
        }

        string jsonData = $"{{\"user_email\": \"{userEmail}\", \"user_password\": \"{userPassword}\"}}";

        using (UnityWebRequest request = new UnityWebRequest(serverURL + "/login", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                UserResponse userResponse = JsonUtility.FromJson<UserResponse>(request.downloadHandler.text);

                if (userResponse.status == "success")
                {

                    GameData.userId = userResponse.user.user_id;
                    GameData.targetPanelName = "EnterPanel";
                    SceneManager.LoadScene("HomeUI");
                }
                else
                {
                    if (userResponse.status == "fail" && userResponse.message.Contains("Email not verified"))
                    {
                        ShowPopup("�Х����ұz���q�l�l��C���ˬd�z���H�c�C");
                    }
                    else
                    {
                        ShowPopup("�n�J���ѡG" + userResponse.message);
                    }

                }
            }
            else
            {
                ShowPopup("�n�J���~�G" + request.downloadHandler.text);
            }
        }
    }

    // ���� email �榡
    bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    // ���ұK�X�榡
    bool IsValidPassword(string password)
    {
        return password.Length >= 6 && Regex.IsMatch(password, @"[A-Za-z]") && Regex.IsMatch(password, @"[0-9]");
    }

    // ��ܿ��~����
    void ShowPopup(string message)
    {
        popupText.text = message;
        popupPanel.SetActive(true);
    }

    [System.Serializable]
    public class User
    {
        public int user_id;
        public string user_name;
        public string user_email;
        public string last_progress;
    }

    [System.Serializable]
    public class UserResponse
    {
        public string status;
        public string message;
        public User user;
    }
}
