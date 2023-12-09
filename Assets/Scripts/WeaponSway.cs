using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private float swayMultiplier;
    [SerializeField] private float smoothMultiplier;
    //[SerializeField] private float time;
    //[SerializeField] private float duration;
    [SerializeField] private AnimationCurve MoveCurve;
    [SerializeField] private float _animationTimePosition;

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(-mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        if (transform.localRotation != targetRotation)
        {
            _animationTimePosition += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, MoveCurve.Evaluate(_animationTimePosition) * smoothMultiplier * Time.deltaTime);
        }
        else
        {
            _animationTimePosition = 0;
        }
    }
}
