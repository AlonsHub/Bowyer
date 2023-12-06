using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType { Run, Sprint, Step, MidAir, Crouch, Prone};

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, InputPanel
{
    //handles input grab and processing
    //controls the player character, NOT THE BOW!
    //the player interacts with the bow as an interface - and that can recat however it would like

    public static bool ActionInputPanelsEnabled;

    //[SerializeField]
    //bool inputPanelEnabled;

    [SerializeField]
    Transform gfxScaler;

    [SerializeField]
    CharacterController cc;
    [SerializeField]
    Bow bow;

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
    //Vector3 _jumpTime;

    float _originalHeight = 2.69f;

    [SerializeField]
    SmoothRotator yRotator;
    [SerializeField]
    SmoothRotator xRotator;


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

    MoveType _previousMoveType;

    public Vector3 GetVelocity => cc.velocity;

    //EventInstance allows us to start and stop events of sound as needed.
    //requires using FMOD.Studio;

    private EventInstance playerFootsteps;
    private EventInstance playerFootstepsSprint;

    private void Start()
    {
        //Creating the instances here in order to allow us full control of when the sound will activate and stop.
        playerFootsteps = AudioManager.Instance.CreateEventIsntace(FMODEvents.Instance.playerFootsteps);
        playerFootstepsSprint = AudioManager.Instance.CreateEventIsntace(FMODEvents.Instance.playerFootstepsSprint);
    }

    public MoveType CurrentMoveType;
    float _currentSpeed()
    {
        switch (CurrentMoveType)
        {
            case MoveType.Run:
                return walkSpeed;
            case MoveType.Sprint:
                return runSpeed;
            case MoveType.Step:
                return stepSpeed;
                case MoveType.MidAir:
                return midairSpeed;
            case MoveType.Crouch:
                return crouchSpeed;
                case MoveType.Prone: 
                return proneSpeed;

            default:
                return 0f;
        }
    }


    Vector3 _inputVector;
    [SerializeField]
    private float crouchYValue;
    [SerializeField]
    private float proneYValue;

    // Start is called before the first frame update
    void Awake()
    {
        _previousMoveType = MoveType.Run;
        CurrentMoveType = MoveType.Run;
        _currentJumpForce = Vector3.zero;
        if (!cc)
        {
            cc = GetComponent<CharacterController>(); //this actually must exist, since it is a required component
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        GrabInput();

        //jump should be timed better, perhaps using a "regular/normalized jump time" will permit the use of an acceleration curve

        if (!cc.isGrounded)
        {
            if (cc.velocity.y > 0f)
            {
                _currentJumpForce += Physics.gravity * Time.deltaTime;
            }
            else
            {
                _currentJumpForce += 2f * Physics.gravity * Time.deltaTime;
            }
        }
        //else
        //{
        //    _currentJumpForce = Vector3.zero;
        //    //TEMP! jump should be independant since it may not depend only on IsGrounded (double jump is the obvious example)
        //    if (Input.GetKeyDown(jumpKey))
        //    {
        //        Jump();
        //    }
        //}

        //_inputVector *= Time.deltaTime;
        Move();



        UpdateSound();

    }

    public void GrabInput()
    {
        if (!IsEnabled())
            return;

        _inputVector = Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right;

        if (_inputVector.magnitude > 1)
            _inputVector.Normalize();

        if (cc.isGrounded)
        {
            //_currentJumpForce = Vector3.zero;
            //TEMP! jump should be independant since it may not depend only on IsGrounded (double jump is the obvious example)
            if (Input.GetKeyDown(jumpKey))
            {
                Jump();
            }
        }

        yRotator.GetInput(Input.GetAxis("Mouse X"));
        xRotator.GetInput(Input.GetAxis("Mouse Y"));

        HandleMoveStates();
        _inputVector *= _currentSpeed();

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
            CurrentMoveType = MoveType.MidAir;
        }
        else if (Input.GetKey(sprintKey) && ((int)CurrentMoveType <= 2))
        {
            CurrentMoveType = MoveType.Sprint;
        }
        else if (Input.GetKey(stepKey))
        {
            CurrentMoveType = MoveType.Step;
        }
        else if (Input.GetKey(crouchKey))
        {
            CurrentMoveType = MoveType.Crouch;
            cc.height = _originalHeight * crouchYValue;
        }
        else if (Input.GetKey(proneKey))
        {
            CurrentMoveType = MoveType.Prone;

            cc.height = _originalHeight * proneYValue;
        }
        else
        {
            cc.height = _originalHeight;
            CurrentMoveType = MoveType.Run;
        }

        if (Input.GetKeyUp(crouchKey)) //this may be a problem
        {
            cc.height = _originalHeight;
        }
    }

    void Jump()
    {
        _currentJumpForce = Vector3.up * jumpForce + cc.velocity/2f;
    }

    public bool IsGrounded => cc.isGrounded;

    private void UpdateSound()
    {
        Vector3 _moveVector = _inputVector + _currentJumpForce;
        if (_moveVector.magnitude > minMoveMagnitude && cc.isGrounded)
        {
            //PLAYBACK_STATE returns the state that the sound is currently in.
            //the options are PLAYING, SUSTAINING, STOPPED, STARTING, STOPPING

            PLAYBACK_STATE playbackState;

            if(CurrentMoveType == MoveType.Sprint)
            {
                playerFootsteps.stop(STOP_MODE.IMMEDIATE);

                //This is how we get the PLAYBACK_STATE
                playerFootstepsSprint.getPlaybackState(out playbackState);

                //This is how we check what the PLAYBACK_STATE is
                if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
                {
                    playerFootstepsSprint.start();
                }
            }
            else
            {
                playerFootstepsSprint.stop(STOP_MODE.IMMEDIATE);

                playerFootsteps.getPlaybackState(out playbackState);

                if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
                {
                    playerFootsteps.start();
                }
            }
        }
        else
        {
            //When we stop we can eitehr stop with allowing Fadeout for the sound
            //Or we can stop immediately 
            playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
            playerFootstepsSprint.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }


    public bool IsEnabled()
    {
        return ActionInputPanelsEnabled;
    }

    public void SetInputPanelEnable(bool isEnable)
    {
        ActionInputPanelsEnabled = isEnable;
    }
}
