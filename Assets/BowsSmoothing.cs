using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowsSmoothing : MonoBehaviour
{
    [SerializeField]
    Transform followTarget;
    [SerializeField]
    PlayerController pc;

    [SerializeField]
    float scaler;
    [SerializeField]
    float smoother;

    [SerializeField]
    Vector3 offset;

    Vector3 _force;
    Vector3 _vel;



    private void Update()
    {
        //_force = pc.GetVelocity/ scaler;

        //_force = (followTarget.position + offset) + pc.GetVelocity / scaler;
        _force = offset + pc.GetVelocity / scaler;

        if(pc.CurrentMoveType != MoveType.MidAir)
        {
            _force.y = followTarget.position.y;
        }
        //_force = -1f * (followTarget.forward * _force.z + followTarget.up * _force.y + followTarget.right * _force.x);

        //transform.position = Vector3.SmoothDamp(transform.position, _force, ref _vel, smoother);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, _force, ref _vel, smoother);

    }

    //private void LateUpdate()
    //{
    //    transform.localPosition += offset;
    //}

}
