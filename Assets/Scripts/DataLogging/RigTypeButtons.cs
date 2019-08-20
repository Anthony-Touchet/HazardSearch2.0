using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RigTypeButtons : MonoBehaviour
{
    private string m_currentSelection;
    private static List<HazardObject> m_rigTypeButtons = new List<HazardObject>();
    // Use this for initialization
    void Start()
    {
        foreach (var button in transform.GetComponentsInChildren<HazardObject>())
        {
            m_rigTypeButtons.Add(button);
        }

        m_currentSelection = m_rigTypeButtons[0].gameObject.name;
        DisableOthers(m_currentSelection);
    }

    public void DisableOthers(string p_newName){
        foreach(HazardObject ho in m_rigTypeButtons){
            var button = ho.gameObject.GetComponent<Button>();

            if(ho.gameObject.name == p_newName){
                button.interactable = true;
                ho.enabled = false;
                m_currentSelection = p_newName;
            }

            else{
                button.interactable = false;
                ho.enabled = true;
            }
        }
    }

    public void DisableAll(){
        foreach(HazardObject ho in m_rigTypeButtons){
            var go = ho.gameObject;

            ho.enabled = false;
            go.GetComponent<Button>().interactable = false;
        }
    }

    public void LoadSelectedScene(){
        var uiUtils = FindObjectOfType<UIUtils>();

        uiUtils.GoToSceneAsync(m_currentSelection);
    }

    void OnDestroy()
    {
        m_rigTypeButtons.Clear();
    }
}
