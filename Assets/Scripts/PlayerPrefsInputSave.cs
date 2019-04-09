using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.InputField))]
public class PlayerPrefsInputSave : MonoBehaviour
{
    public bool autoFill;

    [SerializeField]
    private string playerPref;
    private UnityEngine.UI.InputField inputField;

    void Start()
    {
        inputField = GetComponent<UnityEngine.UI.InputField>();

        if(PlayerPrefs.HasKey(playerPref) && autoFill)
            inputField.text = PlayerPrefs.GetString(playerPref);

        inputField.onEndEdit.AddListener(delegate {SaveInput();});
    }

    void SaveInput()
    {
        PlayerPrefs.SetString(playerPref, inputField.text);
    }
}