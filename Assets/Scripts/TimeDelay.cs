using UnityEngine;
using System.Collections;

public class TimeDelay : MonoBehaviour
{
    public float m_TimeDelay;
    public UnityEngine.Events.UnityEvent m_DelayedEvent;

    public void StartDelayEvent()
    {
        StopAllCoroutines();
        StartCoroutine(Wait());
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(m_TimeDelay);
        m_DelayedEvent.Invoke();
    }
}
