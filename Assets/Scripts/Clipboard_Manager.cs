using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clipboard_Manager : MonoBehaviour
{
    public void ToggleOnOff(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void ToggleOnOff(Image image)
    {
        image.enabled = !image.enabled;
    }

    public void ToggleButton(Button button)
    {
        button.interactable = !button.interactable;
    }
}
