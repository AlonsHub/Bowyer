using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectorCollider : MonoBehaviour
{
    public UnityEvent<Collider> OnTriggerEnterEvent;
    public Collider coll;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent.Invoke(other);
    }
}
