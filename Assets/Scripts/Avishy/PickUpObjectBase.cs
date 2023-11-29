using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PickUpObjectBase :MonoBehaviour, IPickable
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform grabPointTransform;
    [SerializeField] private float moveSpeed;

    private void OnValidate()
    {
        transform.TryGetComponent(out rb);
    }
    public void PickUp(Transform pickUpPoint)
    {
        grabPointTransform = pickUpPoint;
        rb.useGravity = false;
    }
    public void Drop()
    {
        grabPointTransform = null;
        rb.useGravity = true;
    }

    private void FixedUpdate()
    {
        if(grabPointTransform)
        {
            Vector3 newPostion = Vector3.Lerp(transform.position, grabPointTransform.position, Time.deltaTime * moveSpeed);
            rb.MovePosition(newPostion);
        }
    }

    public GameObject ReturnConnectedGO()
    {
        return gameObject;
    }

    public void PlaySound()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.itemCollected, transform.position);
    }
}
