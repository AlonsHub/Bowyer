using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AxisDirection {Up, Down, Left, Right};
public class SmoothRotator : MonoBehaviour
{
    //Rotator Data
    [SerializeField]
    Vector3 rotAxis;
    [SerializeField]
    string inputAxis;

    [SerializeField]
    float rotSpeed;
    [SerializeField]
    bool doLimit;
    [SerializeField]
    bool localRotation;
    [SerializeField]
    float minRot;
    [SerializeField]
    float maxRot;
    

    [SerializeField]
    float _currentRot;
    float _targetRot;

    float _currentT;
    [SerializeField]
    float _maxT = 1f;


    Vector3 _targetRotVector;
    Vector3 _targetTargetRotVector;
    Vector3 _vel1;
    Vector3 _vel2;
    [SerializeField]
    private float lerpStep;
    [SerializeField]
    private float noInputDrag;

    private void Awake()
    {
        _currentRot = 0;
        _targetRot = 0;
        _vel1 = Vector3.zero;
    }

    public void GetInput(float delta)
    {
        _currentT = 0;
        if (doLimit)
        {
            _currentRot += delta * rotSpeed * Time.deltaTime;
            _currentRot = Mathf.Clamp(_currentRot, minRot, maxRot);
            _targetRotVector = new Vector3(rotAxis.x == 0 ? transform.localEulerAngles.x : _currentRot * rotAxis.x, rotAxis.y == 0 ? transform.localEulerAngles.y : _currentRot * rotAxis.y, rotAxis.z == 0 ? transform.localEulerAngles.z : _currentRot * rotAxis.z);
        }
        else
        {
            _targetTargetRotVector = new Vector3(rotAxis.x * delta * rotSpeed , rotAxis.y * delta * rotSpeed , rotAxis.z * delta * rotSpeed);
            _targetRotVector = Vector3.Lerp(_targetRotVector, _targetTargetRotVector, lerpStep);
        }
        
    }

    //TEMP INPUT
    private void Update()
    {
        if(Input.GetAxis(inputAxis) != 0)
        {
            GetInput(Input.GetAxis(inputAxis));
        //_currentT += Time.deltaTime;
        //if (_currentT < _maxT)
        //    transform.localRotation = Quaternion.Euler(Vector3.Lerp(transform.localEulerAngles, _targetRotVector, _currentT / _maxT));
        }
        SmoothToTarget();

    }

    void SmoothToTarget()
    {
        //transform.localRotation = Quaternion.Euler(Vector3.Lerp(transform.localEulerAngles, _targetRotVector, _currentT / _maxT));
        //Vector3 a = Vector3.zero;
        //transform.localRotation = Quaternion.Euler(Vector3.SmoothDamp(transform.localEulerAngles, _targetRotVector, ref _vel, _maxT));
        if (doLimit)
        {


            //_targetTargetRotVector.x =(_targetTargetRotVector.x >= maxRot || _targetTargetRotVector.x <= minRot) ? 
            transform.localEulerAngles = _targetRotVector;
            //_vel2 *= noInputDrag;

        }
        else
        {
            //transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(_targetRotVector + transform.localEulerAngles), _maxT * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(Vector3.SmoothDamp(transform.localEulerAngles, _targetRotVector + transform.localEulerAngles,ref _vel1,_maxT * Time.deltaTime));
            _targetRotVector *= noInputDrag;
        }

    }
}
