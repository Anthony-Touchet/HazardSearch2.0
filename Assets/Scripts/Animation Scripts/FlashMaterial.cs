using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashMaterial : MonoBehaviour
{
    public float m_brightnessFactor;
    public float m_speed;
    private Material m_material;

    private float origin;
    private bool m_brightining;
    // Start is called before the first frame update
    void Awake()
    {
        m_material = GetComponent<Renderer>().material;
        m_brightining = true;
        origin = m_material.GetColor("_EmissionColor").r;
    }

    // Update is called once per frame
    void Update()
    {
        var oldColor = m_material.GetColor("_EmissionColor");

        if(m_brightining)
            {
               m_material.SetColor("_EmissionColor", 
               new Color(oldColor.r + (m_speed* Time.deltaTime),
                oldColor.g, oldColor.b));
            }
        else
            {
               m_material.SetColor("_EmissionColor", 
               new Color(oldColor.r - (m_speed* Time.deltaTime),
                oldColor.g, oldColor.b));
            }
        
        if(m_material.GetColor("_EmissionColor").r >= origin + m_brightnessFactor && m_brightining == true)
            m_brightining = false;
        else if(m_material.GetColor("_EmissionColor").r <= origin - m_brightnessFactor && m_brightining == false)
            m_brightining = true;
    }
}
