using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class VelocityTracker : OpenEndedStack<Vector3>
{
     public VelocityTracker(int length) : base(length)
    {
        Reset();
    }

    void Reset()
    {
        for (int i = 0; i < ts.Length; i++)
        {
            ts[i] = Vector3.zero;
        }
    }

    public Vector3 GetAverageVelocity()
    {
        Vector3 toReturn = Vector3.zero;

        for (int i = 0; i < ts.Length; i++)
        {
            //if(ts[i].magnitude <= 0.01f)
            //    break;

            toReturn += ts[i];
        }

        //toReturn /= ts.Length;

        return toReturn / ts.Length;
    }
}
