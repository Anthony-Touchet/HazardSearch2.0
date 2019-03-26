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

    private int m_score;

    public int score{
        get{return m_score;}
    }

    public void ChangeScore(int points){
        m_score = points;
    }
}
