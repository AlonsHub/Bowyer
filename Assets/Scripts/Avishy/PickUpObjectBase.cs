using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class PickUpObjectBase :MonoBehaviour, IPickable
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform grabPointTransform;
    [SerializeField] private float moveSpeed;



    [SerializeField] private float minSoundDistance;
    [SerializeField] private float maxSoundDistance;


    StudioEventEmitter emitter;


    private void Start()
    {
        emitter = AudioManager.Instance.CreateEventEmitter(FMODEvents.Instance.itemIdle, gameObject, minSoundDistance, maxSoundDistance);
        emitter.Play();
    }

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

    public void PlaySound()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.itemCollected, transform.position);
    }

    public void DestroyItem()
    {
        PlaySound();
        emitter.Stop();
        Destroy(gameObject);
    }
}
