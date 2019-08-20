using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clipboard_Manager : MonoBehaviour
{
    public Text m_bigText;
    public Text m_smallText;
    public Text m_endScreen;

     private Mouledoux.Components.Mediator.Subscriptions m_subscriptions =
        new Mouledoux.Components.Mediator.Subscriptions();

    private Mouledoux.Callback.Callback appendBigText;
    private Mouledoux.Callback.Callback setBigText;
    private Mouledoux.Callback.Callback appendSmallText;
    private Mouledoux.Callback.Callback setSmallText;
    private Mouledoux.Callback.Callback setReview;


    void Awake(){
        Initalize();
    }

    void Initalize(){
        appendBigText = AppendBigText;
        setBigText = SetBigText;
        appendSmallText = AppendSmallText;
        setSmallText = SetSmallText;
        setReview = PrintReview;

        m_subscriptions.Subscribe("appendbigtext", appendBigText);
        m_subscriptions.Subscribe("setbigtext", setBigText);
        m_subscriptions.Subscribe("appendsmalltext", appendSmallText);
        m_subscriptions.Subscribe("setsmalltext", setSmallText);
        m_subscriptions.Subscribe("setreview", setReview);
    }

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

    private void AppendText(Text textField, string appendage){
        textField.text += appendage;
    }

    private void SetText(Text textField, string message){
        textField.text = message;
    }

    private void SetBigText(object[] args){
        SetText(m_bigText, (string)args[0]);
    }

    private void AppendBigText(object[] args){
        AppendText(m_bigText, (string)args[0]);
    }

    private void SetSmallText(object[] args){
        SetText(m_smallText, (string)args[0]);
    }

    private void AppendSmallText(object[] args){
        AppendText(m_smallText, (string)args[0]);
    }

    private void PrintReview(object[] args){
        var randomHazards = FindObjectOfType<RandomHazardManager>();
        SetText(m_bigText, randomHazards.MakeResultString());
        SetText(m_endScreen, randomHazards.MakeResultString());
    }


    private void OnDestroy()
    {
        m_subscriptions.UnsubscribeAll();
    }
}
