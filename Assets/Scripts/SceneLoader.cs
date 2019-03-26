using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public string[] onLoadMessages;
    
    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions =
    new Mouledoux.Components.Mediator.Subscriptions();

    public static SceneLoader _instance;

    public SceneLoader instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<SceneLoader>();
            return _instance;
        }

    }

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetLoadMessage(string newMessage)
    {
        string[] newMessages = newMessage.Split(',');
        SetLoadMessage(newMessages);
    }

    public void SetLoadMessage(string[] newMessages)
    {
        instance.onLoadMessages = newMessages;
    }

    private void OnLevelWasLoaded()
    {
        foreach(string onLoadMessage in onLoadMessages)
        {
            if (onLoadMessage == null) continue;
            
            Mouledoux.Components.Mediator.instance.NotifySubscribers(onLoadMessage.Trim());
            print(onLoadMessage);
        }
    }
}