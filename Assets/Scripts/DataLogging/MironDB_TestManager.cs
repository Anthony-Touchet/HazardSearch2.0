using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MironDB_TestManager : MonoBehaviour
{
    public int testScenarioID;

    [SerializeField]
    bool testComplete = false;
    [SerializeField]
    bool passed = true;

    [SerializeField]
    bool isInstance = false;

    private static MironDB_TestManager _instance;
    public static MironDB_TestManager instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MironDB_TestManager>();
                DontDestroyOnLoad(_instance.gameObject);
                _instance.isInstance = true;
            }

            return _instance;
        }
    }

    void Awake()
    {
        transform.parent = null;
        if(instance != this) Destroy(gameObject);
        //subscriptions.Subscribe("SceneUnlocked", InitalizeScene);
        InitalizeScene(new Mouledoux.Callback.Packet());
    }

    Mouledoux.Callback.Callback finishTest = null;

    Mouledoux.Components.Mediator.Subscriptions subscriptions =
        new Mouledoux.Components.Mediator.Subscriptions();

    bool m_waitingForNewScene = false;
    //static bool isExam = false;

    private AudioSource m_audioSource;
    private bool m_gelatoUnlocked = false;
    private bool m_finishing = false;

    // ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    void InitalizeScene(Mouledoux.Callback.Packet pack)
    {
        m_audioSource = GetComponent<AudioSource>();

        if(!MironDB.MironDB_Manager.isExam)
        {
            MironDB.MironDB_Manager.StartTest(testScenarioID);
        }


        else
        {
            MironDB.MironDB_Manager.UpdateTest((int)MironDB.DB_CODES.SCENE_EVENT, $"Exam scenario: {transform.root.name} begin!");
        }

        

        finishTest += FinishTest;
        subscriptions.Subscribe("endtest", finishTest);

        subscriptions.Subscribe("testcomplete", PassTest);
    }

    private void PassTest(Mouledoux.Callback.Packet packet){
        testComplete = true;
    }


    // ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    private void FailTest(Mouledoux.Callback.Packet packet)
    {
        testComplete = true;

        var TMPs = Resources.FindObjectsOfTypeAll(typeof(TMPro.TextMeshProUGUI));
            foreach(TMPro.TextMeshProUGUI t in TMPs){
                if(t.gameObject.name == "Fail Text (TMP)" && t.transform.parent.gameObject.activeInHierarchy)
                {
                    t.gameObject.SetActive(true);
                }
            }

        Mouledoux.Components.Mediator.instance.NotifySubscribers("PlayFailAudio", new Mouledoux.Callback.Packet());

        if(!passed) return;
        passed = false;

        //MironDB.MironDB_Manager.UpdateTest(1, "Fail");
        Mouledoux.Components.Mediator.instance.NotifySubscribers("DB_FAIL", new Mouledoux.Callback.Packet());
        Mouledoux.Components.Mediator.instance.NotifySubscribers("endtest", new Mouledoux.Callback.Packet());
    }





#region Subscriptions

    

#endregion




    // ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    // Finish test, and send any edge case messages
    public void FinishTest(Mouledoux.Callback.Packet packet)
    {
        StartCoroutine(FinishTestRoutine());
    }

    // ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public IEnumerator FinishTestRoutine()
    {
        if(m_finishing == true){
            yield break;
        }

        m_finishing = true;

        print("ending test...");

        // UIUtils util = FindObjectOfType<UIUtils>();

        // yield return null;

        // if(!testComplete && passed)
        // {
        //     MironDB.MironDB_Manager.UpdateTest((int)MironDB.DB_CODES.HAZARD_EVENT, "DNF");
        // }

        // else if(passed && !MironDB.MironDB_Manager.isExam)
        // {
        //     var canvas = GameObject.Find("TaskCanvas");
        //     foreach(Transform t in canvas.transform){
        //         t.gameObject.SetActive(false);
        //     }   

        //     var TMPs = Resources.FindObjectsOfTypeAll(typeof(TMPro.TextMeshProUGUI));
        //     foreach(TMPro.TextMeshProUGUI t in TMPs){
        //         if(t.gameObject.name == "Pass Text (TMP)" && t.transform.parent.gameObject.activeInHierarchy)
        //         {
        //             t.gameObject.SetActive(true);
        //         }
        //     }
        // }

        // yield return new WaitForSeconds(1f);
        
        MironDB.MironDB_Manager.FinishTest();
        subscriptions.UnsubscribeAll();

        Destroy(gameObject);
    }




    // ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    private void OnDestroy()
    {
        subscriptions.UnsubscribeAll();   
    }
}