﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHazardManager : MonoBehaviour
{
    public enum HazardFrequency
    {
        ALL,
        ALOT,   //3/4
        MOST,   //2/3
        AFEW,   //1/2
    }

    public int m_seed;
    public List<GameObject> m_hazardList;   //List of the hazards or the parents of the hazards.
    public bool m_parentIsGroup;            //Are the hazards children of GameObjects?

    public HazardFrequency m_hazardFrequency;

    [ContextMenu("Initalize")]
    public void Initalize()
    {
        switch(m_hazardFrequency){
            case HazardFrequency.ALL:
                if(m_parentIsGroup)
                    LoopChildrenOfList(1, m_seed);
                     
                else
                    LoopList(1, m_seed);
 
                break;

            case HazardFrequency.ALOT:
                if(m_parentIsGroup)
                    LoopChildrenOfList(.75f, m_seed);
                     
                else
                    LoopList(.75f, m_seed);
                       
                break;

            case HazardFrequency.MOST:
                if(m_parentIsGroup)
                    LoopChildrenOfList((2/3), m_seed);
                     
                else
                    LoopList((2/3), m_seed);
                                    
                break;

            case HazardFrequency.AFEW:
                if(m_parentIsGroup)
                    LoopChildrenOfList(.5f, m_seed);
                   
                else
                    LoopList(.5f, m_seed);
                                        
                break;
        }
    }

    private void LoopList(float frequency, int seed){
        foreach(GameObject go in m_hazardList){ //Turn Everything on
            go.SetActive(true);
        }

        //Number of objects that will be turned off
        int deactivateNumber = m_hazardList.Count - Mathf.RoundToInt(m_hazardList.Count * frequency);
        System.Random random = seed == 0 ? new System.Random() : new System.Random(seed);

        while(deactivateNumber > 0)
        {
            
            int selection = Mathf.RoundToInt(random.Next(0, m_hazardList.Count));

            if(m_hazardList[selection].activeSelf == true)
            {
                m_hazardList[selection].SetActive(false);
                deactivateNumber--;
            }
        }
    }

    private void LoopChildrenOfList(float frequency, int seed){
         int numberOfChildren = 0;

         foreach(GameObject go in m_hazardList){ //Turn Everything on
            foreach(Transform child in go.transform){
                child.gameObject.SetActive(true);
                numberOfChildren++;
            }
        }

        //Number of objects that will be turned off
        int deactivateNumber = numberOfChildren - Mathf.RoundToInt(numberOfChildren * frequency);
        System.Random random = seed == 0 ? new System.Random() : new System.Random(seed);

        while(deactivateNumber > 0)
        {
            int group = Mathf.RoundToInt(random.Next(0, m_hazardList.Count));                          //Get Group Number
            int child = Mathf.RoundToInt(random.Next(0, m_hazardList[group].transform.childCount));    //Get Child Number

            if(m_hazardList[group].transform.GetChild(child) == null)   //Chect to make sure it exists
                continue;

            if(m_hazardList[group].transform.GetChild(child).gameObject.activeSelf == true)
            {
                m_hazardList[group].transform.GetChild(child).gameObject.SetActive(false);
                deactivateNumber--;
            }
        }
    }
}
