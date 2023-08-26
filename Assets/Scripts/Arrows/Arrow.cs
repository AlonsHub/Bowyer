using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    private float arrowLerp;
    [SerializeField]
    Transform gfx;
    private void Awake()
    {
        rb.isKinematic = true;
    }

    public void ForceMe(Vector3 force)
    {
        rb.isKinematic = false;
        rb.AddForce(force, ForceMode.Impulse);
    }
    public void ForceMeFWD(float force)
    {
        rb.isKinematic = false;
        rb.AddForce(force * transform.forward, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (!rb.isKinematic && rb.velocity.magnitude >= 2)
            //gfx.transform.LookAt(transform.position + rb.velocity );
            gfx.forward = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Sticky"))
        {
            gfx.Translate(Vector3.forward* arrowLerp);
            rb.isKinematic = true;
        }
    }
}
