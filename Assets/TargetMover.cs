using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMover : MonoBehaviour
{
    [SerializeField]
    float frequency; 
    [SerializeField]
    float amplitude;
    [SerializeField]
    Vector3 axisOfMovement;

    [SerializeField]
    Transform childToMove;

    private void Update()
    {
        float sin = Mathf.Sin(Time.time * frequency) * amplitude;

        childToMove.localPosition = new Vector3(axisOfMovement.x == 0 ? 0 : axisOfMovement.x * sin, axisOfMovement.y == 0 ? 0 : axisOfMovement.y * sin, axisOfMovement.z == 0 ? 0 : axisOfMovement.z * sin);
    }
}
