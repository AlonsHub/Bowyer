using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MoveType { Walk, Run, Step, MidAir, Crouch, Prone};

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    //handles input grab and processing
    //controls the player character, NOT THE BOW!
    //the player interacts with the bow as an interface - and that can recat however it would like
    //
    //[SerializeField]
    //Transform xRotator;
    //[SerializeField]
    //Transform crouchPos;
    //[SerializeField]
    //Transform standPos;
    [SerializeField]
    Transform gfxScaler;


    [SerializeField]
    CharacterController cc;
    [SerializeField]
    float walkSpeed;
    [SerializeField]
    float runSpeed;
    [SerializeField]
    float crouchSpeed;
    [SerializeField]
    float proneSpeed;

    [SerializeField]
    private float stepSpeed;
        [SerializeField]
    private float midairSpeed;

        [SerializeField]
    private float jumpForce;
        [SerializeField]
    private float minMoveMagnitude;
        [SerializeField]
    private float drag;

    Vector3 _currentJumpForce;

    float _originalHeight = 4.635122f;

    [Header("Keys")]
    [SerializeField]
    KeyCode sprintKey;
    [SerializeField]
    KeyCode stepKey;
    [SerializeField]
    KeyCode crouchKey;

    [SerializeField]
    KeyCode jumpKey;
    [SerializeField]
    KeyCode proneKey;

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
            case MoveType.Crouch:
                return crouchSpeed;
                break;
                case MoveType.Prone: 
                return proneSpeed;
                break;

            default:
                return 0f;
                break;
        }
    }

MoveType _currentMoveType;

    Vector3 _inputVector;
    [SerializeField]
    private float crouchYValue;
    [SerializeField]
    private float proneYValue;

    // Start is called before the first frame update
    void Awake()
    {
        _currentMoveType = MoveType.Walk;
        _currentJumpForce = Vector3.zero;
        if (!cc)
        {
            cc = GetComponent<CharacterController>(); //this actually must exist, since it is a required component
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        _inputVector = Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right;
        
        if (_inputVector.magnitude > 1)
            _inputVector.Normalize();


        HandleMoveStates();
        //temp!
        _inputVector *= _currentSpeed();

        if (!cc.isGrounded)
            _currentJumpForce += Physics.gravity *Time.deltaTime; //This will decay the jump force correctly + it will become negative after peeking, adding to the fall
        else
        {
            _currentJumpForce = Vector3.zero;
            //TEMP! jump should be independant since it may not depend only on IsGrounded (double jump is the obvious example)
            if (Input.GetKeyDown(jumpKey))
            {
                Jump();
            }
        }

        //_inputVector *= Time.deltaTime;
        Move();

    }

    void Move()
    {
        Vector3 _moveVector = _inputVector + _currentJumpForce;
        if (_moveVector.magnitude > minMoveMagnitude)
            cc.Move((_moveVector + Physics.gravity) * Time.deltaTime);
    }

    void HandleMoveStates()
    {
        if (!cc.isGrounded)
        {   
            _currentMoveType = MoveType.MidAir;
        }
        else if (Input.GetKey(sprintKey))
        {
            _currentMoveType = MoveType.Run;
        }
        else if (Input.GetKey(stepKey))
        {
            _currentMoveType = MoveType.Step;
        }
        else if (Input.GetKey(crouchKey))
        {
            _currentMoveType = MoveType.Crouch;

            //xRotator.localPosition = crouchPos.localPosition;
            //gfxScaler.localScale = new Vector3(1, crouchYValue, 1);
            cc.height = _originalHeight * crouchYValue;
        }
        else if (Input.GetKey(proneKey))
        {
            _currentMoveType = MoveType.Prone;

            //xRotator.localPosition = crouchPos.localPosition;
            //gfxScaler.localScale = new Vector3(1, crouchYValue, 1);
            cc.height = _originalHeight * proneYValue;
        }
        else
        {
            //xRotator.localPosition = standPos.localPosition;
            cc.height = _originalHeight;
            _currentMoveType = MoveType.Walk;
        }

        if (Input.GetKeyUp(crouchKey))
        {
            //xRotator.localPosition = standPos.localPosition;
            //gfxScaler.localScale = Vector3.one;
            cc.height = _originalHeight;
        }
    }

    void Jump()
    {
        _currentJumpForce = Vector3.up * jumpForce + cc.velocity/5f;
    }
}
