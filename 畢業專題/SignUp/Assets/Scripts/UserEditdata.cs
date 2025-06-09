using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class ProfileUpdater : MonoBehaviour
{
    public InputField nameInput;
    public InputField emailInput;
    public InputField oldPasswordInput;
    public InputField newPasswordInput;

    private string serverURL = GameData.baseUrl;

    public void OnSubmitEdit()
    {
        StartCoroutine(UpdateProfile());
    }

    IEnumerator UpdateProfile()
    {
        string name = nameInput.text;
        string email = emailInput.text;
        string oldPassword = oldPasswordInput.text;
        string newPassword = newPasswordInput.text;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(oldPassword))
        {
            Debug.Log("請填寫所有欄位（新密碼可留空）");
            yield break;
        }

        // 準備 JSON
        string jsonData = JsonUtility.ToJson(new ProfileUpdateRequest
        {
            user_id = GameData.userId,
            user_name = name,
            user_email = email,
            old_password = oldPassword,
            new_password = newPassword
        });


        using (UnityWebRequest request = new UnityWebRequest(serverURL + "/update_profile", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("修改成功！");
                // 可以提示用戶「修改成功」
            }
            else
            {
                Debug.LogError("修改失敗：" + request.downloadHandler.text);
                // 顯示錯誤訊息
            }
        }
    }
    public void OnUpdateButtonClicked()
    {
        StartCoroutine(UpdateProfile());
    }


    [System.Serializable]
    public class ProfileUpdateRequest
    {
        public int user_id;
        public string user_name;
        public string user_email;
        public string old_password;
        public string new_password;
    }

}
