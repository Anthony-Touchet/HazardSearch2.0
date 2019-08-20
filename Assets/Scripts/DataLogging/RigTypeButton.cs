using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RigTypeButton : MonoBehaviour
{
    [HideInInspector] public Button TheButton => GetComponent<Button>();
    private RigTypeButtons m_buttonController;

    private void Start() {
        m_buttonController = transform.parent.gameObject.GetComponent<RigTypeButtons>();
    }

    public void ButtonPressed(){
        m_buttonController.DisableOthers(gameObject.name);
    }
}
