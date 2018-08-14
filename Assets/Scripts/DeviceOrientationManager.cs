using UnityEngine;
using UnityEngine.UI;

public enum Orientations
{
    NORMAL = 0, FLAT = 1
}

public class DeviceOrientationManager : MonoBehaviour
{
    [SerializeField] GameObject[] selectedImages;

    int orientation;
    Button[] orientationButtons;

    void Start()
    {
        orientation = PlayerPrefs.GetInt("deviceOrientation", 1);
        orientationButtons = GetComponentsInChildren<Button>();

        if (gameObject.name != "AskDevicePosition")
        {
            // disabling the current selected orientation
            orientationButtons[orientation].interactable = false;
            selectedImages[orientation].SetActive(true);
        }
    }

    // the buttons in the scene call this function
    public void ChangeDeviceOrientation(int newOrientation)
    {
        toggleSelected(true);
        orientation = newOrientation;
        PlayerPrefs.SetInt("deviceOrientation", orientation);
        toggleSelected(false);
    }

    void toggleSelected(bool state)
    {
        Button temp = orientationButtons[orientation];
        if(gameObject.name != "AskDevicePosition")
            selectedImages[orientation].SetActive(!state);
        if(temp != null)
            temp.interactable = state;
    }
}