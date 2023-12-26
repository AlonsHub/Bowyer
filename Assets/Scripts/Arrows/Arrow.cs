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
    GameObject trailer;
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
        col.enabled = true;
        trailer.SetActive(true);
    }
    //public void ForceMeFWD(float force)
    //{
    //    rb.isKinematic = false;
    //    rb.AddForce(force * transform.forward, ForceMode.Impulse);
    //}


    // Update is called once per frame
    void Update()
    {
        if (!rb.isKinematic && rb.velocity.magnitude >= 6f)
            transform.forward = Vector3.Lerp(transform.forward, rb.velocity.normalized, .8f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasHit)
            return;
        Destroy(gameObject, 5f);


        if (collision.gameObject.CompareTag("Sticky"))
        {
            _hasHit = true;
            trailer.SetActive(false);

            _pushDir = vtc.AverageVelXFramesDelay(1) * arrowStickInAmount;
            transform.position += _pushDir;
            rb.isKinematic = true;

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

            //if(bp)
            //{
            //    /
            //}

            Destroy(vtc, .3f);
            col.enabled = false;
            this.enabled = false;
            //Some sort of Destroy with a delay
            //StartCoroutine(LatePush());
        }
    }
}
