using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    Collider myCollider;
    [SerializeField]
    Animator anim;
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
        if (!anim)
            anim = GetComponent<Animator>();
    }

    public void ForceMe(Vector3 force)
    {
        anim.SetTrigger("Fly");
        rb.isKinematic = false;
        rb.AddForce(force, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        if (!rb.isKinematic && rb.velocity.magnitude >= 6f)
            transform.forward = Vector3.Lerp(transform.forward, rb.velocity.normalized, .9f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasHit)
        {

            return;
        }

        if(collision.gameObject.CompareTag("Sticky"))
        {

            anim.SetTrigger("Stick");

            _hasHit = true;
            //gfx.Translate(Vector3.forward* arrowStickInAmount);
            _pushDir = vtc.AverageVelXFramesDelay(1) * arrowStickInAmount;
            transform.position += _pushDir;
            rb.isKinematic = true;

            //if (collision.transform.localScale.x == collision.transform.localScale.y && collision.transform.localScale.x == collision.transform.localScale.z)
                transform.SetParent(collision.transform);


            BodyPart bp = collision.gameObject.GetComponent<BodyPart>();

            damage += vtc.AverageVel().magnitude * rb.mass;


            Debug.Log(damage);
            if (bp)
            {
                bp.TakeDamage(damage);
                //transform.SetParent(bp.transform);
            }
            else
            {
                bp = collision.gameObject.GetComponentInParent<BodyPart>();
                if (bp)
                {
                    bp.TakeDamage(damage);

                }
            }
            StartCoroutine(LateStop());
        }
    }

    //IEnumerator LatePush()
    //{
    //    yield return new WaitForSeconds(.02f);
    //    //rb.isKinematic = false;
    //    rb.AddForce(vtc.AverageVel(), ForceMode.Impulse);
    //    yield return new WaitForSeconds(.01f);
    //    rb.isKinematic = true;
    //    col.enabled = false
    //}
    IEnumerator LateStop()
    {
        yield return new WaitForSeconds(.02f);
        myCollider.enabled = false;
        Destroy(vtc);

        //this.enabled = false;

    }
}
