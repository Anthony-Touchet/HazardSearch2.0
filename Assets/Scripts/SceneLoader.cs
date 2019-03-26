using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public string onLoadMessage;
    
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
        instance.onLoadMessage = newMessage;
    }

    private void OnLevelWasLoaded()
    {
        if (onLoadMessage == null) onLoadMessage = "";
        
        Mouledoux.Components.Mediator.instance.NotifySubscribers(onLoadMessage, new Mouledoux.Callback.Packet());
        print(onLoadMessage);
    }
}