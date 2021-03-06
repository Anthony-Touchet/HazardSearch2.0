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

    private int m_activeHazardCount = 0;

    public int m_seed;
    public List<GameObject> m_hazardList = new List<GameObject>();   //List of the hazards or the parents of the hazards.
    public bool m_parentIsGroup;            //Are the hazards children of GameObjects?

    public HazardFrequency m_hazardFrequency;

    private List<GameObject> m_activeObjects = new List<GameObject>();
    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions = 
    new Mouledoux.Components.Mediator.Subscriptions();

    Mouledoux.Callback.Callback nextRailHandeler;
    Mouledoux.Callback.Callback removeObjectFromList;

    void Awake(){
        removeObjectFromList = RemoveGameObject;
        nextRailHandeler = SetLocalHazards;
        m_subscriptions.Subscribe("teleporting", nextRailHandeler);
        m_subscriptions.Subscribe("nolongeractive", removeObjectFromList);
        m_subscriptions.Subscribe("Few Hazards", SetAFew);
        m_subscriptions.Subscribe("Some Hazards", SetSome);
        m_subscriptions.Subscribe("Most Hazards", SetMost);
        m_subscriptions.Subscribe("All Hazards", SetAll);
    }

    IEnumerator Start(){
        yield return new WaitForEndOfFrame();
        Initalize();
    }

    [ContextMenu("Initalize")]
    public void Initalize()
    {
        m_activeHazardCount = 0;
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
        var packet = new object[]{m_activeObjects.Count, null, null};

        Mouledoux.Components.Mediator.instance.NotifySubscribers("setmaxscore", packet);

        packet[1] = 0;
        packet[2] = true;
        
        //Mouledoux.Components.Mediator.instance.NotifySubscribers("setcurrentscore", packet);
        var pack = new object[]{0};
        
        SetLocalHazards(pack);
    }

    private void SetLocalHazards(object[] args){
        foreach(GameObject go in m_hazardList){
            if(m_hazardList[(int)args[0]] == go)
            {
                foreach(Transform trans in go.transform){
                trans.gameObject.SetActive(true);
                }
                continue;
            }
            
            foreach(Transform trans in go.transform){
                trans.gameObject.SetActive(false);
            }
        }
    }

    public void SetFrequency(int frequency){
        switch(frequency){
            case 1:
                m_hazardFrequency = HazardFrequency.AFEW;
                break;
            
            case 2:
                m_hazardFrequency = HazardFrequency.MOST;
                break;

            case 3:
                m_hazardFrequency = HazardFrequency.ALOT;
                break;

            default:
                m_hazardFrequency = HazardFrequency.ALL;
                break;
        }
    }

    public void SetAFew(object[] pack){
        m_hazardFrequency = HazardFrequency.AFEW;
        print("A few");
    }

    public void SetSome(object[] pack){
        m_hazardFrequency = HazardFrequency.MOST;
        print("Some");
    }

    public void SetMost(object[] pack){
        m_hazardFrequency = HazardFrequency.ALOT;
        print("Most");
    }

    public void SetAll(object[] pack){
        m_hazardFrequency = HazardFrequency.ALL;
        print("All");
    }

    public void SetSeed(int seed){
        m_seed = seed;
    }

    private void LoopList(float frequency, int seed){
        foreach(GameObject go in m_hazardList){ //Turn Everything on
            go.SetActive(true);
            m_activeHazardCount++;
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
                Destroy(m_hazardList[selection]);
                deactivateNumber--;
                m_activeHazardCount--;
            }
        }

        foreach(GameObject go in m_hazardList){ //Turn Everything on
            if(go.activeSelf == true)
                m_activeObjects.Add(go);
        }
    }

    private void LoopChildrenOfList(float frequency, int seed){
         int numberOfChildren = 0;

         foreach(GameObject go in m_hazardList){ //Turn Everything on
            foreach(Transform child in go.transform){
                child.gameObject.SetActive(true);
                numberOfChildren++;
                m_activeHazardCount++;
                foreach(Transform greatChild in child){ //Bad, Highlight, Good
                        if(greatChild.GetComponentInChildren<HazardObject>() != null){
                            m_activeHazardCount++;
                        }
                    }
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
                Destroy(m_hazardList[group].transform.GetChild(child).gameObject);
                deactivateNumber--;
                m_activeHazardCount--;
                foreach(Transform greatChild in m_hazardList[group].transform.GetChild(child)){ //Bad, Highlight, Good
                        if(greatChild.GetComponentInChildren<HazardObject>() != null){
                            m_activeHazardCount--;
                        }
                    }
            }
        }
        
        //Turn Everything on
        foreach(GameObject go in m_hazardList){             //points
            foreach(Transform child in go.transform){       //Hazards
                if(child.gameObject.activeSelf == true){
                    m_activeObjects.Add(child.gameObject);
                    foreach(Transform greatChild in child){ //Bad, Highlight, Good
                        if(greatChild.GetComponentInChildren<HazardObject>() != null){
                            var obj = greatChild.GetComponentInChildren<HazardObject>().gameObject;
                            m_activeObjects.Add(obj);
                        }
                    }
                }  
            }
        }


    }

    public string MakeResultString(){
        string result = "";
        
        var missedHazardsName = new List<string>();
        var allHazardList = new List<GameObject>();
        
        //Get all the hazards
        // if(m_parentIsGroup)
        // {
        //     foreach(GameObject go in m_hazardList){ //Turn Everything on
        //         foreach(Transform child in go.transform){
        //             allHazardList.Add(child.gameObject);
        //         }
        //     }
        // }

        // else
        //     allHazardList = m_hazardList;

        allHazardList = FindAllHazards();

        //See if they were found
        foreach(GameObject go in m_activeObjects)
        {
            missedHazardsName.Add(go.name);
        }

        //String header
        if(ScoreManager.instance.gradeResult >= ScoreManager.instance.passingGrade)
            result += "Good job!\n\n";
        else
            result += "Could use improvement.\n\n";

        //Score
        result += "You found " + ScoreManager.instance.currentScore + " of " + 
            ScoreManager.instance.maxScore + " (" + (int)(ScoreManager.instance.gradeResult * 100f) + 
            "%)\n\n";
        
        //Footer Message
        if(ScoreManager.instance.gradeResult >= 1f)
            result += "You missed none.";
        else{
            result += "Here are some you missed:\n";
            foreach(string s in missedHazardsName){
                result += s+", ";
            }
        }
            

        return result;
    }

    public string GiveScoreOnly(){
        return ScoreManager.instance.currentScore + " / " + 
            ScoreManager.instance.maxScore + " (" + (int)(ScoreManager.instance.gradeResult * 100) + 
            "%)";
    }

    public List<GameObject> FindAllHazards(){
        var hazards = new List<GameObject>();
        
        var hazScripts = GameObject.FindObjectsOfType<HazardObject>();
        for(int i = 0; i < hazScripts.Length; i++){
            hazards.Add(hazScripts[i].gameObject);
        }
        
        return hazards;
    }

    private void RemoveGameObject(object[] pack){
        var newList = m_activeObjects;

        foreach(GameObject go in newList){
            if(go.GetInstanceID() == (int)pack[0])
                m_activeObjects.Remove(go);
        }
    }



    public string GetMissingHazards(ref List<GameObject> hazards)
    {
        string returnString = "";

        foreach(GameObject go in hazards)
            returnString += $"[{go.name}] ";

        return returnString;
    }

    public string[] ReturnMissingHazards(){
        var missedHazardsName = new string[m_activeObjects.Count];

        for(int i = 0; i < m_activeObjects.Count; i++)
        {
            missedHazardsName[i] = m_activeObjects[i].name;
        }
        return missedHazardsName;
    }

    private void OnDestroy() {
        m_subscriptions.UnsubscribeAll();
    }
}