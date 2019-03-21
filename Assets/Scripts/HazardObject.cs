using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardObject : InteractableObject
{
    public int m_scoreValue;
    
    public void IncreaseScore()
    {
        //ScoreManager.instance.m_currentScore += m_scoreValue;
    }
}