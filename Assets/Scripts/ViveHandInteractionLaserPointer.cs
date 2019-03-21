using UnityEngine;

[RequireComponent(typeof(Valve.VR.InteractionSystem.Hand), typeof(LineRenderer))]
public class ViveHandInteractionLaserPointer : MonoBehaviour
{
    private GameObject m_targetObject;
    private bool m_isHoldingSomething = false;

    private RaycastHit m_raycast;

    private Valve.VR.InteractionSystem.Hand m_hand;
    private LineRenderer m_lineRenderer;
    private Vector3 m_endLinePos;

    static bool hasTriggered = false;

    private bool m_controllerConnected
    {
        get { return m_hand.isActive; }
    }

    // ---------- ---------- ---------- ---------- ----------
    void Start ()
    {
        m_hand = GetComponent<Valve.VR.InteractionSystem.Hand>();
        m_lineRenderer = GetComponent<LineRenderer>();
    }


    // ---------- ---------- ---------- ---------- ----------
    void Update()
    {
        #region original lazer
        //if (m_controllerConnected)
        //{
        //    if (CheckObjectHit())
        //    {
        //        if (CheckForInteractableObject(m_targetObject))
        //        {
        //            if (CheckInput())
        //            {
        //                OnObjectInteract();
        //            }
        //        }
        //    }
        //}


        //UpdateLaser();
        #endregion


        if (m_controllerConnected)
        {
            if (CheckLongInput())   // We are holding down the trigger
            {
                CheckObjectHit();
            }

            else if (CheckOffInput() && m_targetObject != null) // We have let go of the trigger, AND have a target object
            {
                if (CheckForInteractableObject(m_targetObject) != null)
                {
                    OnObjectInteract();
                }
            }
        }

        bool[] extraLazerConditions = { CheckLongInput() };
        UpdateLaser(extraLazerConditions);
    }


    // ---------- ---------- ---------- ---------- ----------
    public bool CheckObjectHit()
    {
        if (Physics.Raycast(m_hand.transform.position, m_hand.transform.forward, out m_raycast))
        {
            if (m_targetObject != m_raycast.transform.gameObject && !m_isHoldingSomething)
            {
                if (m_targetObject != null)
                {
                    Mouledoux.Components.Mediator.instance.NotifySubscribers
                        (m_targetObject.GetInstanceID().ToString() + "->offhighlight", new Mouledoux.Callback.Packet());
                }
                
                m_targetObject = m_raycast.transform.gameObject;

                Mouledoux.Components.Mediator.instance.NotifySubscribers
                    (m_targetObject.GetInstanceID().ToString() + "->onhighlight", new Mouledoux.Callback.Packet());
            }

            m_endLinePos = m_raycast.point;
            return true;
        }

        else if (m_targetObject != null)
        {
            Mouledoux.Components.Mediator.instance.NotifySubscribers
                (m_targetObject.GetInstanceID().ToString() + "->offhighlight", new Mouledoux.Callback.Packet());

            m_targetObject = null;
        }

        m_endLinePos = m_hand.transform.position + m_hand.transform.forward;
        return false;
    }


    // ---------- ---------- ---------- ---------- ----------
    public InteractableObject CheckForInteractableObject(GameObject go)
    {
        return go.GetComponent<InteractableObject>();
    }


    // ---------- ---------- ---------- ---------- ----------
    public bool CheckInput()
    {
        //m_hand.controller.TriggerHapticPulse();
        return (m_hand.grabPinchAction.stateDown);
    }


    // ---------- ---------- ---------- ---------- ----------
    public bool CheckLongInput()
    {
        //m_hand.controller.TriggerHapticPulse();
        return (m_hand.GetBestGrabbingType() == Valve.VR.InteractionSystem.GrabTypes.Pinch);
    }


    // ---------- ---------- ---------- ---------- ----------
    public bool CheckOffInput()
    {
        //m_hand.controller.TriggerHapticPulse();
        return (m_hand.grabPinchAction.stateUp);
    }


    // ---------- ---------- ---------- ---------- ----------
    public int OnObjectInteract()
    {
        InteractableObject io = m_targetObject.GetComponent<InteractableObject>();

        Mouledoux.Components.Mediator.instance.NotifySubscribers
            (m_targetObject.GetInstanceID().ToString() + "->oninteract", new Mouledoux.Callback.Packet());

        if (io.m_interactionType == InteractableObject.InteractionType.PICKUP && !io.m_lockedInPlace)
        {
            if (m_isHoldingSomething) return -1;
            io.m_lockedInPlace = true;

            StartCoroutine(HoldObject(m_raycast.collider));
        }
        
        else if (io.m_interactionType == InteractableObject.InteractionType.LONGINTERACT)
        {
            StartCoroutine(LongInteract(m_raycast.transform.gameObject));
        }

        else
        {
            StartCoroutine(OffInteract(m_raycast.transform.gameObject));
        }

        return 0;
    }


    // ---------- ---------- ---------- ---------- ----------
    public void UpdateLaser(bool[] extraConditions)
    {
        bool extraCondition = true;
        foreach(bool condition in extraConditions)
        {
            if (condition == false)
            {
                extraCondition = false;
                break;
            }
        }

        m_lineRenderer.enabled = m_controllerConnected && extraCondition;
        m_lineRenderer.SetPositions( new Vector3[] {m_hand.transform.position, m_endLinePos});
    }


    // ---------- ---------- ---------- ---------- ----------
    public System.Collections.IEnumerator HoldObject(Collider go)
    {
        Vector3 lastPos = Vector3.zero;
        Collider collider = go;
        Color lineColor = m_lineRenderer.endColor;

        collider.enabled = false;
        m_isHoldingSomething = true;

        bool canDrop = false;
        while (CheckInput() || canDrop ==  false)
        {
            go.transform.position = m_lineRenderer.GetPosition(m_lineRenderer.positionCount - 1);
            m_lineRenderer.endColor = canDrop ? Color.green : Color.red;

            yield return null;
        }

        Mouledoux.Components.Mediator.instance.NotifySubscribers
            (go.gameObject.GetInstanceID().ToString() + "->offinteract", new Mouledoux.Callback.Packet());

        go.transform.parent = m_raycast.transform;
        go.transform.position = m_raycast.point;

        collider.enabled = true;
        m_isHoldingSomething = false;
        m_lineRenderer.endColor = lineColor;


        if (!hasTriggered)
        {
            Mouledoux.Components.Mediator.instance.NotifySubscribers("trigger", new Mouledoux.Callback.Packet());
            hasTriggered = true;
        }
    }


    // ---------- ---------- ---------- ---------- ----------
    public System.Collections.IEnumerator LongInteract(GameObject go)
    {
        yield return new WaitWhile(() => (CheckLongInput()));

        Mouledoux.Components.Mediator.instance.NotifySubscribers
            (go.GetInstanceID().ToString() + "->offinteract", new Mouledoux.Callback.Packet());

        Mouledoux.Components.Mediator.instance.NotifySubscribers
            (m_raycast.transform.gameObject.GetInstanceID().ToString() + "->offinteract", new Mouledoux.Callback.Packet());
    }


    // ---------- ---------- ---------- ---------- ----------
    public System.Collections.IEnumerator OffInteract(GameObject go)
    {
        yield return new WaitWhile(() => (CheckInput()));

        Mouledoux.Components.Mediator.instance.NotifySubscribers
            (go.GetInstanceID().ToString() + "->offinteract", new Mouledoux.Callback.Packet());
    }


    // ---------- ---------- ---------- ---------- ----------
    public void OnDestroy()
    {
        hasTriggered = false;
    }
}