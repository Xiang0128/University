using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CameraSelector : MonoBehaviour
{
    public Dropdown cameraDropdown;

    void Start()
    {
        List<string> cameraOptions = new List<string>();
        WebCamDevice[] devices = WebCamTexture.devices;

        for (int i = 0; i < devices.Length; i++)
        {
            cameraOptions.Add(devices[i].name);
        }

        cameraDropdown.ClearOptions();
        cameraDropdown.AddOptions(cameraOptions);

        // Set default value
        if (devices.Length > 0)
        {
            GameData.selectedCameraName = devices[0].name;
        }

        cameraDropdown.onValueChanged.AddListener(OnCameraSelected);
    }

    void OnCameraSelected(int index)
    {
        GameData.selectedCameraName = WebCamTexture.devices[index].name;
        Debug.Log("Selected Camera: " + GameData.selectedCameraName);
    }
}
