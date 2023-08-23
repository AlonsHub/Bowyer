using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MoveType { Walk, Run, Step, MidAir};

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    //handles input grab and processing
    //controls the player character, NOT THE BOW!
    //the player interacts with the bow as an interface - and that can recat however it would like
    //
    [SerializeField]
    CharacterController cc;
    [SerializeField]
    float walkSpeed;
    [SerializeField]
    float runSpeed;
    [SerializeField]
    private float stepSpeed;
        [SerializeField]
    private float midairSpeed;
    [Header("Keys")]
    [SerializeField]
    KeyCode sprintKey;
    [SerializeField]
    KeyCode jumpKey;

    float _currentSpeed()
    {
        switch (_currentMoveType)
        {
            case MoveType.Walk:
                return walkSpeed;
                break;
            case MoveType.Run:
                return runSpeed;
                break;
            case MoveType.Step:
                return stepSpeed;
                break;
                case MoveType.MidAir:
                return midairSpeed;
                break;
            default:
                return 0f;
                break;
        }
    }

MoveType _currentMoveType;

    Vector3 _inputVector;

    // Start is called before the first frame update
    void Awake()
    {
        _currentMoveType = MoveType.Walk;
        if (!cc)
        {
            cc = GetComponent<CharacterController>(); //this actually must exist, since it is a required component
        }    
    }

    
    void Update()
    {
        _inputVector = Vector3.zero;

        _inputVector = Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right;
        
        if (_inputVector.magnitude > 1)
            _inputVector.Normalize();


        HandleMoveStates();
        //temp!
        _inputVector *= _currentSpeed();

        if (!cc.isGrounded)
            _inputVector += Physics.gravity;

        _inputVector *= Time.deltaTime;
        Move();
    }

    void Move()
    {
        cc.Move(_inputVector);
    }

    void HandleMoveStates()
    {
        if(Input.GetKeyDown())
        {

        }
    }
}
