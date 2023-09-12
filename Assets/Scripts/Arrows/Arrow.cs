using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    Collider col;

    [SerializeField]
    private float arrowStickInAmount;
    [SerializeField]
    Transform gfx;

    [SerializeField]
    VelocityTrackerComponent vtc;
    [SerializeField]
    float damage;

    Vector3 _pushDir;
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
        if (!rb.isKinematic && rb.velocity.magnitude >= 6f)
            transform.forward = Vector3.Lerp(transform.forward, rb.velocity.normalized, .6f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Sticky"))
        {
            //gfx.Translate(Vector3.forward* arrowStickInAmount);
            _pushDir = vtc.AverageVelXFramesDelay(3) * arrowStickInAmount;
            transform.position += _pushDir;
            rb.isKinematic = true;
            transform.SetParent(collision.transform);

            BodyPart bp = collision.gameObject.GetComponent<BodyPart>();
            if (bp)
            {
                bp.TakeDamage(damage);
            }
            else
            {
                bp = collision.gameObject.GetComponentInParent<BodyPart>();
                if (bp)
                    bp.TakeDamage(damage);
            }
            Destroy(vtc);
            col.enabled = false;
            //Some sort of Destroy with a delay
            //StartCoroutine(LatePush());
        }
    }

    //IEnumerator LatePush()
    //{
    //    yield return new WaitForSeconds(.02f);
    //    //rb.isKinematic = false;
    //    rb.AddForce(vtc.AverageVel(), ForceMode.Impulse);
    //    yield return new WaitForSeconds(.01f);
    //    rb.isKinematic = true;
    //    col.enabled = false;

    //}
}
