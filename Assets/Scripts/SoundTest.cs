using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundTest : MonoBehaviour
{
    public AudioMixer m_audioMixer;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void test(UnityEngine.UI.Slider slider)
    {
        m_audioMixer.SetFloat("Volume", slider.value);
    }
}
