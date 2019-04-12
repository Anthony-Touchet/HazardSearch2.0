using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherEmailSender : MonoBehaviour
{
    private static TeacherEmailSender _instance;
    public static TeacherEmailSender instance
    {
        get
        {
            if(_instance == null)
                _instance = FindObjectOfType<TeacherEmailSender>();

            return _instance;
        }
    }


    private void Awake()
    {
        if(instance != this) Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if(instance == this)
            EmailScoreManager.SendTeacherMasterScoreEmail();
    }
}