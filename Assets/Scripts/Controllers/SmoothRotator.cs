using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AxisDirection {X, Y, Z};
public class SmoothRotator : MonoBehaviour
{
    //Rotator Data
    [SerializeField]
    AxisDirection axisDirection;
    string inputAxis()
    {
        switch (axisDirection)
        {
            case AxisDirection.X:
                return "Mouse Y";
                break;
            case AxisDirection.Y:
                return "Mouse X ";
                break;
            case AxisDirection.Z:
                return "The Fuck?";
                break;
            default:
                break;
        }
        return "again, the fuck?";
    }

    

    [SerializeField]
    float rotSpeed;

    [SerializeField]
    bool doLimit;
    [SerializeField]
    float minRot;
    [SerializeField]
    float maxRot;
    
    [SerializeField]
    bool doMomentum;

    [SerializeField]
    float momentumDrag;
    [SerializeField]
    float minMomentum;
    [SerializeField]
    float factorMomentum;
    [SerializeField]
    float _currentMomentum;

    [SerializeField]
    float _currentRot;
  

    Vector3 _targetRotVector;

    private void Awake()
    {
        switch (axisDirection)
        {
            case AxisDirection.X:
        _currentRot = transform.localEulerAngles.x; 
                break;
            case AxisDirection.Y:
        _currentRot = transform.localEulerAngles.y; 
                break;
            case AxisDirection.Z:
        _currentRot = transform.localEulerAngles.z; 
                break;
            default:
                break;
        }

        _currentMomentum = 0;
    }

    public void GetInput(float delta)
    {
        _currentRot += delta * rotSpeed * Time.deltaTime;

        if(doMomentum)
        {
            _currentMomentum += delta * rotSpeed * Time.deltaTime;

            _currentRot += _currentMomentum * factorMomentum;

        }

        if (doLimit)
            _currentRot = Mathf.Clamp(_currentRot, minRot, maxRot);

        //_targetRotVector = new Vector3(rotAxis.x == 0 ? transform.localEulerAngles.x : _currentRot * rotAxis.x, rotAxis.y == 0 ? transform.localEulerAngles.y : _currentRot * rotAxis.y, rotAxis.z == 0 ? transform.localEulerAngles.z : _currentRot * rotAxis.z);
        switch (axisDirection)
        {
            case AxisDirection.X:
        _targetRotVector = new Vector3(_currentRot, transform.localEulerAngles.y ,transform.localEulerAngles.z);
                break;
            case AxisDirection.Y:
        _targetRotVector = new Vector3(transform.localEulerAngles.x, _currentRot, transform.localEulerAngles.z);
                break;
            case AxisDirection.Z:
        _targetRotVector = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, _currentRot);
                break;
            default:
                break;
        }

        transform.localEulerAngles = _targetRotVector;
    }

    //TEMP INPUT
    private void Update()
    {
        if (Input.GetAxis(inputAxis()) != 0)
        {
            GetInput(Input.GetAxis(inputAxis()));
        }
        else
        {
            if (doMomentum)
            {
                GetInput(_currentMomentum);
            }
        }
    }

    private void FixedUpdate()
    {
        if (doMomentum &&  Mathf.Abs(_currentMomentum) > minMomentum)
        {
            if(_currentMomentum > 0)
            _currentMomentum -= momentumDrag *Time.fixedDeltaTime;
            else
            _currentMomentum += momentumDrag * Time.fixedDeltaTime;
        }
    }

}
