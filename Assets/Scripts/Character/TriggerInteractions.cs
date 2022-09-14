using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteractions : MonoBehaviour
{
    private System.Action<GameObject> OnTriggerInteractionEnter, OnTriggerInteractionExit;
    public void Init(System.Action<GameObject> OnTriggerInteractionEnter, System.Action<GameObject> OnTriggerInteractionExit)
    {
        this.OnTriggerInteractionEnter = OnTriggerInteractionEnter;
        this.OnTriggerInteractionExit = OnTriggerInteractionExit;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerInteractionEnter(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerInteractionExit(other.gameObject);
    }
}
