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

        [SerializeField]
    private float jumpForce;

    Vector3 _currentJumpForce;

    [Header("Keys")]
    [SerializeField]
    KeyCode sprintKey;
    [SerializeField]
    KeyCode stepKey;

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
        _currentJumpForce = Vector3.zero;
        if (!cc)
        {
            cc = GetComponent<CharacterController>(); //this actually must exist, since it is a required component
        }    
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
            _currentJumpForce += Physics.gravity *Time.deltaTime;
        else
        {
            if (Input.GetKeyDown(jumpKey))
            {
                Jump();
            }
        }

        _inputVector *= Time.deltaTime;
        Move();

    }

    void Move()
    {
        cc.Move(_inputVector + _currentJumpForce * Time.deltaTime);
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
        else
            _currentMoveType = MoveType.Walk;
    }

    void Jump()
    {
        _currentJumpForce = Vector3.up * jumpForce + cc.velocity/5f;
    }
}
