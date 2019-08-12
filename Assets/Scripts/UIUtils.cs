using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIUtils : MonoBehaviour
{
    public void GoToScene(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }

    public void GoToScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

    public void ReloadCurrentScene()
    {
        GoToScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void GoToScene(Dropdown dropdown)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(dropdown.captionText.text);
    }


    public void GoToSceneAsync(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(index);
    }

    public void GoToSceneAsync(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
    }

    public void LoadRandomScene(Transform parent)
    {
        var buttonList = parent.GetComponentsInChildren<Button>();
        buttonList[Random.Range(0, buttonList.Length)].onClick.Invoke();
    }

    public void ReloadCurrentSceneAsync()
    {
        GoToSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void GoToSceneAsync(TMPro.TMP_Dropdown dropdown)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(dropdown.captionText.text);
    }


    public static void StaticReloadCurrentSceneAsync()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void ToggleGameobjectEnable(GameObject go)
    {
        go.SetActive(!go.activeSelf);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void NotifySubscribers(string subscription)
    {
        Mouledoux.Components.Mediator.instance.NotifySubscribers(subscription, new Mouledoux.Callback.Packet());
    }

    public void SetSceneLoadMessage(string message)
    {
        SceneLoader._instance.SetSoloMessage(message);
        print(message);
    }

    public void DebugAction(string message){
        print(message);
    }
    
}