using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleCamera : MonoBehaviour
{
    public Transform m_holeCameraTransform;
    public Transform m_playerCameraTransform;
    public Transform m_holeTransform;

    // Update is called once per frame
    void Update()
    {
        float angle = Vector3.Angle(m_playerCameraTransform.forward, m_holeCameraTransform.position.normalized);
        if(angle > 90){
            m_holeCameraTransform.forward = m_playerCameraTransform.forward; 
        }
    }
}
