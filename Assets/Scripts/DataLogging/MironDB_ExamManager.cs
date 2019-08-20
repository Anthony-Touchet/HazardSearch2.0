// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;


// namespace MironDB
// {
//     public class MironDB_ExamManager : MonoBehaviour
//     {
//         private static bool hasLoaded = false;
//         private static MironDB_ExamManager _instance = null;

//         [SerializeField]
// 	    private GuidedSceneStepper m_sceneStepper;

//         private Mouledoux.Components.Mediator.Subscriptions m_subscriptions = 
// 		    new Mouledoux.Components.Mediator.Subscriptions();
        
//         Mouledoux.Callback.Callback loadExam;

//         private void Awake()
//         {
//             if(_instance != null) Destroy(gameObject);

//             _instance = this;
//             loadExam = LoadExamDetails;
//             m_subscriptions.Subscribe("LoadExam", loadExam);
//         }


//         public void LoadExamDetails(object[] args)
//         {
//             StopAllCoroutines();
//             StartCoroutine(LoadExamDetailsRoutine());
//         }

//         public IEnumerator LoadExamDetailsRoutine()
//         {
//             if(hasLoaded) yield break;
//             else hasLoaded = true;

//             MironDB_ExamUI exui = FindObjectOfType<MironDB_ExamUI>();

//             int examID = exui.m_exams[exui.m_dropdown.value.ToString()];
//             print(examID);
//             print(exui.m_dropdown.options);
//             print(exui.m_dropdown.value);

//             MironDB_Manager.LoadExamDetails(examID);
            
//             yield return new WaitWhile(() => (MironDB_Manager.examDetails == null));

//             if(MironDB_Manager.examDetails.requirements.Length < 1)
//             {
//                 hasLoaded = false;
//                 Mouledoux.Components.Mediator.instance.NotifySubscribers("EmptyExam", null);
//                 yield break;
//             }

//             m_sceneStepper.random = true;
//             m_sceneStepper.originalScenarios = new List<GuidedSceneStepper.SceneScenario>();
//             m_sceneStepper.scenarios = new List<GuidedSceneStepper.SceneScenario>();

//             foreach(MironDB_Manager.Requirements r in MironDB_Manager.examDetails.requirements)
//             {   
//                 GuidedSceneStepper.SceneScenario scenario =
//                     new GuidedSceneStepper.SceneScenario("ExamRig2.0", (System.Convert.ToInt32(r.stepName.Trim()) - 1000).ToString(), r.difficultyID, r.attemptsNo);

//                 m_sceneStepper.originalScenarios.Add(scenario);
//                 m_sceneStepper.scenarios.Add(scenario);
//             }

//             MironDB_Manager.isExam = true;
//             Mouledoux.Components.Mediator.instance.NotifySubscribers("ScenarioComplete", null);

//         }

//         private void OnDestroy()
//         {
//             m_subscriptions.UnsubscribeAll();    
//         }
//     }
// }