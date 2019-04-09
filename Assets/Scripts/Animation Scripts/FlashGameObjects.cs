using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashGameObjects : MonoBehaviour
{
    public float m_lifeTime;
    public AudioSource m_audioSource;
    public float m_moveFactor;

    public List<GameObject> m_gameObjects;

    private List<Vector3> origins = new List<Vector3>();

    public void Awake()
    {
        foreach(GameObject go in m_gameObjects){
            go.SetActive(false);
            origins.Add(go.transform.localPosition);
        }
        StopAllCoroutines();
        StartCoroutine(ToggleAfter());
    }

    public IEnumerator ToggleAfter()
    {
        while(true){
            foreach (GameObject go in m_gameObjects){
                go.SetActive(true);
                go.transform.localPosition = new Vector3(origins[0].x + Random.Range(-m_moveFactor, m_moveFactor),
                origins[0].y, origins[0].z);
            }
            m_audioSource.Play();

            yield return new WaitForSeconds(m_lifeTime);
        
            foreach (GameObject go in m_gameObjects){
                go.SetActive(false);
            }

            yield return new WaitForSeconds(Random.Range(1f, 10f));
        }
        
    }
}
