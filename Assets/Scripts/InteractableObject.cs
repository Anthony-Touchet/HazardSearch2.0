using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public enum InteractionType
    {
        DEFAULT,
        PICKUP,
        LONGINTERACT,
    }

    public InteractionType m_interactionType;
    public bool m_lockedInPlace = false;

    //public bool m_pickup;
    //[SerializeField]
    //private bool repickup;
    //[HideInInspector]
    //public bool m_repickup { get { return repickup; } }

    public UnityEngine.Events.UnityEvent m_onHighnight;
    public UnityEngine.Events.UnityEvent m_offHighnight;
    public UnityEngine.Events.UnityEvent m_onInteract;
    public UnityEngine.Events.UnityEvent m_offInteract;

    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions = new Mouledoux.Components.Mediator.Subscriptions();

    protected Mouledoux.Callback.Callback onHighlight;
    protected Mouledoux.Callback.Callback offHighlight;
    protected Mouledoux.Callback.Callback onInteract;
    protected Mouledoux.Callback.Callback offInteract;

    private void Start()
    {
        Initialize(gameObject);
    }

    protected void Initialize(GameObject self)
    {
        onHighlight = OnHighlight;
        offHighlight = OffHighlight;

        onInteract = OnInteract;
        offInteract = OffInteract;

        m_subscriptions.Subscribe(self.GetInstanceID().ToString() + "->onhighlight", onHighlight);
        m_subscriptions.Subscribe(self.GetInstanceID().ToString() + "->offhighlight", offHighlight);

        m_subscriptions.Subscribe(self.GetInstanceID().ToString() + "->oninteract", onInteract);
        m_subscriptions.Subscribe(self.GetInstanceID().ToString() + "->offinteract", offInteract);
    }



    protected void OnHighlight(Mouledoux.Callback.Packet packet)
    {
        m_onHighnight.Invoke();
    }

    protected void OffHighlight(Mouledoux.Callback.Packet packet)
    {
        m_offHighnight.Invoke();
    }


    protected void OnInteract(Mouledoux.Callback.Packet packet)
    {
        m_onInteract.Invoke();
    }

    protected void OffInteract(Mouledoux.Callback.Packet packet)
    {
        m_offInteract.Invoke();
    }

    public void ToggleGameObject(GameObject go)
    {
        go.SetActive(!go.activeSelf);
    }
}
