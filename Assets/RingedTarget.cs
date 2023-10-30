using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingedTarget : MonoBehaviour
{
    TargetHitReport _bestTargetHitReport;

    Coroutine waiter;
    public void RecieveHitReport(TargetHitReport newReport)
    {
        if (_bestTargetHitReport == null)
        {
            _bestTargetHitReport = newReport;
            waiter = StartCoroutine(WaitForAllCollisions());
        }
        else if (_bestTargetHitReport.CompareHits(newReport) == true)
        {
            _bestTargetHitReport = newReport;
        }

        //if (waiter == null) 
        //waiter = StartCoroutine(WaitForAllCollisions());
    }

    IEnumerator WaitForAllCollisions()
    {
        yield return new WaitForEndOfFrame();
        _bestTargetHitReport.relatedRenderer.material.color = Color.red;
        _bestTargetHitReport.relatedRenderer.material.SetColor("_EmissiveColor", Color.red *3f);

        Debug.LogWarning($"{_bestTargetHitReport.points} points recieved!");
    }
}

public class TargetHitReport
{
    public int points;
    public Renderer relatedRenderer;

    public TargetHitReport(int p, Renderer r)
    {
        points = p;
        relatedRenderer = r;
    }

    /// <summary>
    /// Returns TRUE if "other" is better than this, False if this is better than "other", and null if they are equal.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool? CompareHits(TargetHitReport other)
    {
        if(points == other.points)
            return null; //Meaning both are equal (currently this case is very unlikely)

        if (points < other.points)
            return true; //Meaning other is better
        
            return false; //Meaning other is worse
    }
}
