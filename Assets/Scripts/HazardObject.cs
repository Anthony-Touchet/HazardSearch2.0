using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardObject : InteractableObject
{
    public float m_scoreValue;
    
    public new void Start(){
        offInteract += AppendName;
        base.Start();
    }

    public void IncreaseScore()
    {
        var packet = new object[]{m_scoreValue};
        Mouledoux.Components.Mediator.instance.NotifySubscribers("incrementcurrentscore", packet);
    }

    public void NotActive(){
        var packet = new object[]{gameObject.GetInstanceID()};
        Mouledoux.Components.Mediator.instance.NotifySubscribers("nolongeractive", packet);
    }

    private void AppendName(object[] args){
        var data = new object[]{gameObject.name + ", "};
        Mouledoux.Components.Mediator.instance.NotifySubscribers("appendbigtext", data);
    }

    [ContextMenu("Off Interact")]
    private void EditorOffInteract(){
        offInteract.Invoke(new object[]{});
    }

    [ContextMenu("On Highlight")]
    private void EditorOnHighlight(){
        onHighlight.Invoke(new object[]{});
    }

    [ContextMenu("Off Highlight")]
    private void EditorOffHighlight(){
        offHighlight.Invoke(new object[]{});
    }
    
    private void OnDestroy() {
        ClearSubscriptions();
    }
}