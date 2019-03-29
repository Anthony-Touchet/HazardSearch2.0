using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardObject : InteractableObject
{
    public float m_scoreValue;
    
    public void IncreaseScore()
    {
        var packet = new Mouledoux.Callback.Packet();
        packet.floats = new float[]{m_scoreValue};
        Mouledoux.Components.Mediator.instance.NotifySubscribers("incrementcurrentscore", packet);
    }
}