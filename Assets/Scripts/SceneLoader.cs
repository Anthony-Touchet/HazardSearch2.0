using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public List<string> onLoadMessages = new List<string>();
    public static float TimeInScene = 0;
    [SerializeField]
    public bool examMode;

    [SerializeField]
    private static bool canSubmitScores = false;
    
    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions
    = new Mouledoux.Components.Mediator.Subscriptions();



    public static SceneLoader _instance;

    public SceneLoader instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<SceneLoader>();
            return _instance;
        }
    }


    // ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void EnableExamMode()
    {
        Mouledoux.Callback.Callback scenarioComplete = LoadNextGuidedScenario;
        m_subscriptions.Subscribe("ScenarioComplete", scenarioComplete);
    }

    public void DisableExamMode()
    {
        m_subscriptions.UnsubscribeAll();
    }

    void OnDestroy()
    {
        m_subscriptions.UnsubscribeAll();
    }

    // ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------


    // ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public void AddToLoadMessages(string newMessage)
    {
        instance.onLoadMessages.Add(newMessage);
    }

    public void SetSoloMessage(string newMessage)
    {
        instance.onLoadMessages.Clear();
        AddToLoadMessages(newMessage);
    }

    // ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    private void OnLevelWasLoaded()
    {
        if (onLoadMessages.Count > 0)
        {
            foreach(string message in onLoadMessages)
            {
                Mouledoux.Components.Mediator.instance.NotifySubscribers(message, new Mouledoux.Callback.Packet());
                print($"{message} from {name}");
            }
        }
        
        onLoadMessages.Clear();

        // if (canSubmitScores)
        // {
        //     SubmitScores();
        // }

        SetTimeInTest(0f);
    }

    // ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public void LoadNextGuidedScenario(Mouledoux.Callback.Packet dataPack)
    {
        DelayLoadNextGuidedScenario();
    }

    // ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public void DelayLoadNextGuidedScenario()
    {
        StartCoroutine(iDelayLoadNextGuidedScenario());
    }

    // ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public IEnumerator iDelayLoadNextGuidedScenario()
    {
        print("Loading Next Scenario...");
        yield return new WaitForSeconds(10f);
    }


    // ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public float GetTimeInTest()
    {
        return TimeInScene;
    }

    // ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public void SetTimeInTest(float value)
    {
        TimeInScene = value;
    }

    // ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    void SubmitScores()
    {
        if (!canSubmitScores) return;

        #region Combu
        // if (Combu.CombuManager.isInitialized && TantrumDemo.combuFail == false)
        // {
        //     print("Score Time!");
        //     Combu.CombuManager.platform.ReportScore((long)GetTimeInTest(), "total_time",
        //         (bool success) =>
        //         {
        //             Mouledoux.Components.Mediator.instance.NotifySubscribers("db_total_time",
        //                 new Mouledoux.Callback.Packet());
        //         });

        //     Combu.CombuManager.platform.ReportScore((long)(AccuracyScoreManager.Instance.GetDropOffGraded() * 100f),
        //         "accuracy_dropoff",
        //         (bool success) =>
        //         {
        //             Mouledoux.Components.Mediator.instance.NotifySubscribers("db_accuracy_dropoff",
        //                 new Mouledoux.Callback.Packet());
        //         });

        //     Combu.CombuManager.platform.ReportScore((long)(AccuracyScoreManager.Instance.GetLoadUpGraded() * 100f),
        //         "accuracy_pickup",
        //         (bool success) =>
        //         {
        //             Mouledoux.Components.Mediator.instance.NotifySubscribers("db_accuracy_pickup",
        //                 new Mouledoux.Callback.Packet());
        //         });
        // }
        #endregion

        else
        {
            
        }
    }
}