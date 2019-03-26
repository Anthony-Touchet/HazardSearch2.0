using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Dropdown))]
public class DropdownNotify : MonoBehaviour
{
    private UnityEngine.UI.Dropdown _dropdown;

    private void Start()
    {
        _dropdown = GetComponent<UnityEngine.UI.Dropdown>();
    }

    public void BroadcastSelectedElement()
    {
        Mouledoux.Components.Mediator.instance.NotifySubscribers(_dropdown.captionText.text);
    }
}
