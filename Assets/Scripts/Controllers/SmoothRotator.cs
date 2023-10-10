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
                return "Mouse X";
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
    MouseInputSettings mouseInputSettings;

    [SerializeField]
    float _currentRot;

    float _currentInput;
  

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
        _currentInput = 0;
    }

    public void GetInput(float delta)
    {
        _currentRot += delta * rotSpeed;


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
        float rawinput = Input.GetAxisRaw(inputAxis());
        if (rawinput != 0)
        {
            if (SpeedsAndSensitivities.CurrentMouseInputSettings.sensitivity == 0)
            {
                _currentInput = rawinput;
            }
            else
            {
                _currentInput += rawinput * Time.deltaTime * SpeedsAndSensitivities.CurrentMouseInputSettings.sensitivity;
            }
            //GetInput(_currentInput);
        }
        else
        {
            if (Mathf.Abs(_currentInput) > SpeedsAndSensitivities.CurrentMouseInputSettings.dead)
            {
                if (SpeedsAndSensitivities.CurrentMouseInputSettings.gravity == 0)
                {
                    _currentInput = 0f; // same as rawInput;
                }
                else
                {
                    if (_currentInput > 0f)
                        _currentInput -= Time.deltaTime * SpeedsAndSensitivities.CurrentMouseInputSettings.gravity;
                    else
                        _currentInput += Time.deltaTime * SpeedsAndSensitivities.CurrentMouseInputSettings.gravity;
                }
            }
            else
            {
                _currentInput = 0f;
            }
        }
        _currentInput = Mathf.Clamp(_currentInput, -1f, 1f);

        //if(_currentInput !=0)
        GetInput(_currentInput);
    }
}
