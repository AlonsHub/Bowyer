using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobber : MonoBehaviour
{
    [SerializeField]
    float freq;
    [SerializeField]
    Vector2 amplitudes;
    [SerializeField]
    float smooth;
    [SerializeField]
    float returnSmooth;

    Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.localPosition;
    }

    private void Update()
    {
        if(new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude > 0)
        {
            AddHeadBob();
        }

        MoveToStart();
    }

    private void AddHeadBob()
    {
       float x = Mathf.Lerp(0f ,Mathf.Sin(Time.time * freq) * amplitudes.x, smooth * Time.deltaTime);
       float y = Mathf.Lerp(0f ,Mathf.Cos(Time.time * freq/2f) * amplitudes.y, smooth * Time.deltaTime);

        transform.localPosition += new Vector3(x, y, 0);
    }

    private void MoveToStart()
    {
        if (transform.localPosition == _startPos) 
            return;

        transform.localPosition = Vector3.Lerp(transform.localPosition, _startPos, returnSmooth * Time.deltaTime);

    }

}
