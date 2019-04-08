using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;

    public static ScoreManager instance{
        get{
            if(_instance == null)
                _instance = FindObjectOfType<ScoreManager>();

            return _instance;
        }
    }

    void Awake()
    {
        if(instance != this)
            Destroy(gameObject);

        Initialize();
    }

    private float m_currentScore;
    private float m_maxScore;

    public float currentScore{get => m_currentScore;} 
    public float maxScore{get => m_maxScore;}
    public float gradeResult{get => m_currentScore/m_maxScore;}

    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions =
        new Mouledoux.Components.Mediator.Subscriptions();

    private Mouledoux.Callback.Callback setCurrent;
    private Mouledoux.Callback.Callback incrementCurrent;
    private Mouledoux.Callback.Callback setMaxScore;
    private Mouledoux.Callback.Callback clearScores;

    [SerializeField, Range(0f,1f)]
    private float m_PassingGrade;
    public float passingGrade => m_PassingGrade;

    [SerializeField]
    private UnityEngine.Events.UnityEvent onPass;
    [SerializeField]
    private UnityEngine.Events.UnityEvent onFail;

    public void Initialize(){
        setCurrent = SetCurrentScore;
        incrementCurrent = SetCurrentScore;
        setMaxScore = SetMaxScore;
        clearScores = ClearScores;

        m_subscriptions.Subscribe("setcurrentscore", setCurrent);
        m_subscriptions.Subscribe("incrementcurrentscore", incrementCurrent);
        m_subscriptions.Subscribe("setmaxscore", setMaxScore);
        m_subscriptions.Subscribe("clearscores", clearScores);
    }

    public bool CheckPass(){
        if(currentScore/maxScore >= m_PassingGrade)
            return true;
        else
            return false;
    }

    public string GetCurrentScore()
    {
        return currentScore.ToString() + "/" + maxScore.ToString();
    }

    private void SetMaxScore(float score){
        m_maxScore = score;
    }

    private void SetCurrentScore(float score, bool setScore = false){
        if(!setScore)
            m_currentScore += score;
        else
            m_currentScore = score;
    }

    private void ClearBothScores(){
        m_currentScore = 0;
        m_maxScore = 0;
    }

    private void SetMaxScore(Mouledoux.Callback.Packet data){
        SetMaxScore(data.floats[0]);
    }

    private void SetCurrentScore(Mouledoux.Callback.Packet data){
        SetCurrentScore(data.floats[0], data.bools[0]);
    }

    private void ClearScores(Mouledoux.Callback.Packet data){
       ClearBothScores();
    }
}
