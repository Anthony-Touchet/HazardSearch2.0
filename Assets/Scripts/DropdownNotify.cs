using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownNotify : MonoBehaviour
{
    private TMPro.TMP_Dropdown _dropdown;

    private void Start()
    {
        _dropdown = GetComponent<TMPro.TMP_Dropdown>();
    }

    public void BroadcastSelectedElement()
    {
        Mouledoux.Components.Mediator.instance.NotifySubscribers(_dropdown.captionText.text);
    }

    public void SetPlayerPref(string playerPref)
    {
        PlayerPrefs.SetString(playerPref, _dropdown.captionText.text);
    }
}
