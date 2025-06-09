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
    public GameObject popupPanel; // 新增提示視窗
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
            ShowPopup("請填寫所有欄位。");
            yield break;
        }

        if (!IsValidEmail(userEmail))
        {
            ShowPopup("請輸入有效的 Email 格式。");
            yield break;
        }

        if (!IsValidPassword(userPassword))
        {
            ShowPopup("密碼需包含英文字母與數字，且長度至少6位。");
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

                // 清空註冊欄位
                registerNameInput.text = "";
                registerEmailInput.text = "";
                registerPasswordInput.text = "";
                ShowPopup("註冊成功，請到信箱驗證您的帳號！");
                ShowLoginPanel();
            }

            else
            {
                ShowPopup("註冊失敗：" + request.downloadHandler.text);
            }
        }
    }

    IEnumerator LoginUser()
    {
        string userEmail = loginEmailInput.text;
        string userPassword = loginPasswordInput.text;

        if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userPassword))
        {
            ShowPopup("請填寫所有欄位。");
            yield break;
        }

        if (!IsValidEmail(userEmail))
        {
            ShowPopup("請輸入有效的 Email 格式。");
            yield break;
        }

        if (!IsValidPassword(userPassword))
        {
            ShowPopup("密碼需包含英文字母與數字，且長度至少6位。");
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
                        ShowPopup("請先驗證您的電子郵件。請檢查您的信箱。");
                    }
                    else
                    {
                        ShowPopup("登入失敗：" + userResponse.message);
                    }

                }
            }
            else
            {
                ShowPopup("登入錯誤：" + request.downloadHandler.text);
            }
        }
    }

    // 驗證 email 格式
    bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    // 驗證密碼格式
    bool IsValidPassword(string password)
    {
        return password.Length >= 6 && Regex.IsMatch(password, @"[A-Za-z]") && Regex.IsMatch(password, @"[0-9]");
    }

    // 顯示錯誤視窗
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
