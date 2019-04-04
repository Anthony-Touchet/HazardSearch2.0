using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGameObjectOnZAxis : MonoBehaviour
{
    public float m_speed;
    public GameObject m_RotateHighlight;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,m_speed*Time.deltaTime);
        if(m_RotateHighlight != null)
            m_RotateHighlight.transform.Rotate(0,0,m_speed*Time.deltaTime);
    }
}
