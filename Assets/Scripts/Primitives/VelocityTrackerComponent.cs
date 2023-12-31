using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTrackerComponent : MonoBehaviour
{
    [SerializeField]
    VelocityTracker velocityTracker;
    [SerializeField]
    Rigidbody toTrack;
    private void Awake()
    {
        velocityTracker = new VelocityTracker(6);
    }

    private void FixedUpdate()
    {
        velocityTracker.Push(toTrack.velocity);
    }

    public Vector3 AverageVel()
    {
        return velocityTracker.GetAverageVelocity();
    }
    public Vector3 AverageVelXFramesDelay(int x)
    {
        return velocityTracker.GetAverageVelocityXFramesAgo(x);
    }

}
