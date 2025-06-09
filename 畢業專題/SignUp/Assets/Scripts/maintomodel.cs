using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    public void LoadModelViewScene()
    {
        SceneManager.LoadScene("ModelView");
    }

    public void LoadTestViewScene()
    {
        SceneManager.LoadScene("TestPanel");
    }

    public void LoadHomeScene()
    {
        SceneManager.LoadScene("HomeUI");
    }
}
