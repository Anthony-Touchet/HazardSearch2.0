﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreKeeper : MonoBehaviour
{
    // private static ScoreKeeper _instance;
    // public static ScoreKeeper instance{
    //     get{
    //         if(_instance == null)
    //             _instance = FindObjectOfType<ScoreKeeper>();
    //         return _instance;
    //     }
    // }

    public static bool m_demo = false;
    public ForceTeleport m_forceTeleport;
    public TMPro.TextMeshProUGUI m_resultsScreen;

    private QuestionHazardData data = new QuestionHazardData();

    public TransitionDataHolder m_dataHolder;

    public DelayEventOnStart m_wrongVoiceOver;
    public DelayEventOnStart m_rightVoiceOver;
    public AudioSource m_audioSource;
    public float m_passingGrade;

    public int Score{
        get{ return data.m_score;}
    }

    public float Percentage{get{ return data.m_score / data.m_maxScore;}}

    public QuestionHazardData Data => data;

    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions = new Mouledoux.Components.Mediator.Subscriptions();

    private Mouledoux.Callback.Callback onScored = null;

    // Start is called before the first frame update
    void Awake()
    {
        if(SceneManager.GetActiveScene().name == "main_Demo"){
            m_demo = true;
        }

        onScored += PacketRecieve;

        m_subscriptions.Subscribe("incrementcurrentscore", onScored);
        m_subscriptions.Subscribe("setreview", SetText);
        m_subscriptions.Subscribe("setmaxscore", SetMaxScore);
    }

    void Start(){
        //SetMaxScore();
    }

    public void AddToScore(int addition){
        if(addition > 0){
            data.m_score += addition;

            // if(!m_demo)
            //     MironDB_TestManager.instance.UpdateTest(DataBase.DBCodeAtlas.RIGHT, $"");
            m_rightVoiceOver.BeginCountdown();
        }
    }

    public void AppendHazard(List<string> names, int hazardCount){
        foreach(string s in names){
            data.m_hazardsMissed += "Z" + m_forceTeleport.currentPoint + " " + s + ",";
            data.m_hazardCount++;
        }

        if(!m_demo){
            string message = $"Hazard Area {m_forceTeleport.currentPoint}-- ";

            if(names.Count > 0){
                message += $"Missed {names.Count}/{hazardCount}: ";
                foreach(string s in names){
                    message += (names[names.Count - 1] == s) ? s : s +", ";
                }
                MironDB.MironDB_Manager.UpdateTest((int)DataBase.DBCodeAtlas.WRONG, message);
            }

            else{
                message += "All hazards found.";
                MironDB.MironDB_Manager.UpdateTest((int)DataBase.DBCodeAtlas.RIGHT, message);
            }

        }
            
    }

    public string ReturnResults(){
        string result = "";

        result += "Final Score" + data.m_score + "\n";
        result += "Questions Missed: " + data.m_questionsMissed + "\n";
        result += "Hazards Missed: " + data.m_hazardsMissed;

        return result;
    }

    public void SetMaxScore(){
        var hazardManagers = FindObjectsOfType<SearchAndFindManager>();

        //Hazards
        foreach(SearchAndFindManager saf in hazardManagers){
            foreach(GameObject go in saf.m_hazards)
                data.m_maxScore++;
        }
    }

    private void SetMaxScore(Mouledoux.Callback.Packet data){
        SetMaxScore(data.ints[0]);
    }

    private void SetMaxScore(int score){
        data.m_maxScore = score;
    }

    public void PacketRecieve(Mouledoux.Callback.Packet pack)
    {
        AddToScore((int)pack.floats[0]);
    }

    public void SetText(Mouledoux.Callback.Packet pack){
        string result = "";
        result += $"Congratulations!\n";
        result += "Your score is: " + data.m_score + "/" + data.m_maxScore + "\n";
        result += "You missed " + data.m_hazardCount +
            " hazard(s).";

        m_resultsScreen.text = result;

        string message = $"Test Complete! Final Score: {data.m_score}/{data.m_maxScore}-- User missed {data.m_hazardCount} hazard(s).";
        MironDB.MironDB_Manager.UpdateTest((int)DataBase.DBCodeAtlas.WRONG, message);

        MironDB_TestManager.instance.FinishTest(new Mouledoux.Callback.Packet());
    }

    public void GetQuestionAndGivenAnswer(GameObject go)
    {
        var question = go.transform.Find("Text Field").Find("Text")
            .GetComponent<TMPro.TextMeshProUGUI>().text;


        string answerGiven = "";
        string correctAnswer = "";
        Transform tTransform = null;

        foreach(Transform t in go.transform)
        {
            if(t.gameObject.activeSelf && (t.name.Contains("Wrong") || t.name.Contains(" Wrong ") || t.name.Contains("Wrong;")))
            {
                TMPro.TextMeshProUGUI text = t.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if(text != null)
                {
                    answerGiven = text.text;
                }

                else
                {
                    var split = t.name.Split(';');
                    answerGiven = split[1].Trim();
                }
                
            }

            else if(t.gameObject.activeSelf && (t.name.Contains("Correct") || t.name.Contains("Correct ") || t.name.Contains("Correct;")))
            {
                TMPro.TextMeshProUGUI text = t.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if(text != null)
                {
                    correctAnswer = text.text;
                }

                else
                {
                    var split = t.name.Split(';');
                    correctAnswer = split[1].Trim();
                }

                tTransform = t;
            }
        }

        if(answerGiven != "")
        {
            m_wrongVoiceOver.BeginCountdown();
            data.m_questionsMissed += "Z" + m_forceTeleport.currentPoint + " " + question + "Answer Given: " + answerGiven + "\n";
            data.m_questionCount++;
        }

        else
        {
            AddToScore(1);
        }

        var questionSplit = question.Split('\n');

        question = "";
        foreach(string s in questionSplit)
        {
            question += s + " ";
        }

        if(answerGiven != "")
        {
            var answerSplit = answerGiven.Split('.');
            if(answerSplit.Length <= 1)
                answerGiven = answerSplit[0];
            else
                answerGiven = answerSplit[1];
        }

        var correctAnswerSplit = correctAnswer.Split('.');
        if(correctAnswerSplit.Length <= 1)
            correctAnswer = correctAnswerSplit[0];
        else
            correctAnswer = correctAnswerSplit[1];


        if(!m_demo)
        {
            bool passed = answerGiven == "";
            string message = $"{(passed ? "Correct" : "Incorrect" )}-- {question.Trim()}: " +
                        $"Answer given: {(passed ? correctAnswer.Trim() : answerGiven.Trim())}/ " +
                        $"Expected answer: {correctAnswer.Trim()}";
            MironDB.MironDB_Manager.UpdateTest(passed ? (int)DataBase.DBCodeAtlas.RIGHT : (int)DataBase.DBCodeAtlas.WRONG, message);
        }
    }

    [ContextMenu("Save Data")]
    public void SaveScore(){
        SaveLocally.SaveScoreData(data, m_dataHolder.m_emailData);
    }

    [ContextMenu("Load Data")]
    public void LoadData(){
        QuestionHazardData d = SaveLocally.LoadScoreData("0001_anthonyjtouchet@gmail.com.xml");
        Debug.Log(d.m_questionCount);
    }
}