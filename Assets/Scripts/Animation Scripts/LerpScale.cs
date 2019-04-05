using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpScale : MonoBehaviour
{
    public float m_lerpFactor;
    public float m_speed;

    private Vector3 origin;
    private bool m_increasing;
    // Start is called before the first frame update
    void Awake()
    {
        m_increasing = true;
        origin = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_increasing)
            transform.localScale = 
                new Vector3(transform.localScale.x + m_speed * Time.deltaTime,
                    transform.localScale.y + m_speed * Time.deltaTime,
                        transform.localScale.z + m_speed * Time.deltaTime);
        else
            transform.localScale = 
                new Vector3(transform.localScale.x - m_speed * Time.deltaTime,
                    transform.localScale.y - m_speed * Time.deltaTime,
                        transform.localScale.z - m_speed * Time.deltaTime);
        
        if(transform.localScale.z >= origin.z + m_lerpFactor && m_increasing == true)
            m_increasing = false;
        else if(transform.localScale.z <= origin.z - m_lerpFactor && m_increasing == false)
            m_increasing = true;
    }
}
