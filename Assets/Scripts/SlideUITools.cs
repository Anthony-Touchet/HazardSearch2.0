using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideUITools : MonoBehaviour
{
    public void DisableChildren(RectTransform a_parent)
    {
        for(int i = 0; i < a_parent.childCount; i++)     
        {
            a_parent.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void StartDisableChildren(RectTransform a_parent)
    {
        StartCoroutine(IDisableChildren(a_parent));
    }

    private IEnumerator IDisableChildren(RectTransform a_parent){
        yield return new WaitForSeconds(.5f);
        for(int i = 0; i < a_parent.childCount; i++)     
        {
            a_parent.GetChild(i).gameObject.SetActive(false);
        }
    }
}
