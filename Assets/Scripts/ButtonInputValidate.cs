using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonInputValidate : MonoBehaviour
{
    private Button thisButton;
    public InputField[] inputToValidate;

    // Start is called before the first frame update
    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.interactable = false;

        foreach(InputField input in inputToValidate)
        {
            input.onEndEdit.AddListener(delegate {ValidateInputs();});
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ValidateInputs()
    {
        foreach(InputField input in inputToValidate)
        {
            if(input.text == "")
            {
                thisButton.interactable = false;
                return;
            }
        }

        thisButton.interactable = true;
    }
}
