using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetReporter : MonoBehaviour
{
    [SerializeField]
    RingedTarget ringedTarget;
    [SerializeField]
    int points;
    [SerializeField]
    Renderer rend;

   void ReportHit()
    {
        ringedTarget.RecieveHitReport(new TargetHitReport(points, rend));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Arrow"))
        {
            ReportHit();
        }
    }
}
