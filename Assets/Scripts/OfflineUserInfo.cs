using UnityEngine;
using UnityEngine.UI;

using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

public class OfflineUserInfo : MonoBehaviour
{
    #region Singleton
    private static OfflineUserInfo _instance;

    public static OfflineUserInfo Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<OfflineUserInfo>();

                if (_instance == null)
                {
                    var singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<OfflineUserInfo>();
                    singletonObject.name = "----- " + typeof(OfflineUserInfo) + " -----";
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }
    #endregion

    #region CallBacks and Subscriptions
    public Mouledoux.Components.Mediator.Subscriptions subscriptions =
        new Mouledoux.Components.Mediator.Subscriptions();

    public Mouledoux.Callback.Callback cLoginUI;
    public Mouledoux.Callback.Callback cLogout;

    public Mouledoux.Callback.Callback cRegisterNewUser;

    public Mouledoux.Callback.Callback cERR;
    #endregion


    void Awake()
    {
        if (Instance != this)
        {
            print(Instance.name);
            Destroy(gameObject);
            return;
        }
    }

    public void InitDataPaths()
    {
        if (!Directory.Exists(csvFile))
        {
            Directory.CreateDirectory(csvFile);
        }
    }

    
    public static void AppendToFile(string newLine)
    {
        string info = "";

        if (!File.Exists(csvFile))
        {
            info += ("Date, User email, Location, Difficulty, Total Score, Grade, Hazards Missed" + "\n");
        }

        System.IO.StreamWriter file = System.IO.File.AppendText(csvFile);



        info += Date() + ", ";
        info += PlayerPrefs.GetString("studentEmail") + ", ";
        info += PlayerPrefs.GetString("level") + ", ";
        info += RandomHazardManager.instance.m_hazardFrequency.ToString() + ", ";
        info += ScoreManager.instance.GetCurrentScore() + ", ";
        info += ScoreManager.instance.gradeResult.ToString("0") + ", ";
        //info += RandomHazardManager.

        file.WriteLine(info);
        file.Close();
    }



    public void OpenCSVLocation()
    {
        string winPath = (csvFile).Replace("/", "\\");
        System.Diagnostics.Process.Start("explorer.exe", ("/root," + winPath));
    }

    public static string Date()
    {
        return System.DateTime.Now.Year.ToString() + "/" +
            System.DateTime.Now.Month.ToString() + "/" +
            System.DateTime.Now.Day.ToString();
    }

    public readonly static string csvFile = UnityEngine.Application.dataPath + "/_PerformanceRecords/";
}
