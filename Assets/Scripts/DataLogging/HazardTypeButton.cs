using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HazardTypeButton : MonoBehaviour
{
    [HideInInspector] public Button TheButton => GetComponent<Button>();
    private HazardTypeButtons m_buttonController;

    private void Start() {
        m_buttonController = transform.parent.gameObject.GetComponent<HazardTypeButtons>();
    }

    public void ButtonPressed(){
        m_buttonController.DisableOthers(gameObject.name);
    }
}
