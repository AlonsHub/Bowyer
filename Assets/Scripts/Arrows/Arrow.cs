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

    bool _hasHit = false; 
    private void Awake()
    {
        rb.isKinematic = true;
    }

    //public void Init(Vector3 force, float forceBasedDamageMod)
    //{

    //}

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
        if (_hasHit)
            return;

        if(collision.gameObject.CompareTag("Sticky"))
        {
            _hasHit = true;
            //gfx.Translate(Vector3.forward* arrowStickInAmount);
            _pushDir = vtc.AverageVelXFramesDelay(1) * arrowStickInAmount;
            transform.position += _pushDir;
            rb.isKinematic = true;
            //transform.SetParent(collision.transform);

            BodyPart bp = collision.gameObject.GetComponent<BodyPart>();

            damage += vtc.AverageVel().magnitude * rb.mass;

            Debug.Log(damage);
            if (bp)
            {
                bp.TakeDamage(damage);
                transform.SetParent(bp.transform);
            }
            else
            {
                bp = collision.gameObject.GetComponentInParent<BodyPart>();
                if (bp)
                {
                    bp.TakeDamage(damage);
                    transform.SetParent(bp.transform);
                }
            }
            Destroy(vtc, .3f);
            col.enabled = false;

            this.enabled = false;
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
