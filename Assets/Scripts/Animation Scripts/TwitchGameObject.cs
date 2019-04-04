using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchGameObject : MonoBehaviour
{
    public float m_twitchFactor;
    public float m_speed;

    private Quaternion origin;
    private bool m_increasing;
    // Start is called before the first frame update
    void Awake()
    {
        m_increasing = true;
        origin = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_increasing)
            transform.Rotate(0,0, m_speed*Time.deltaTime);
        else
            transform.Rotate(0,0, -m_speed*Time.deltaTime);
        
        if(transform.localRotation.z >= origin.z + m_twitchFactor && m_increasing == true)
            m_increasing = false;
        else if(transform.localRotation.z <= origin.z - m_twitchFactor && m_increasing == false)
            m_increasing = true;
    }
}
