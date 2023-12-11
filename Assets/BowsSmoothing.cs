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

    Vector3 _force;

    private void Update()
    {
        _force = pc.GetVelocity/ scaler;

        //transform.localPosition = Mathf.Lerp(, _force, scaler);
        //_force *= smoothing;
    }

}
