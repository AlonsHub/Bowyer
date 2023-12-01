using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
enum BowState { Empty, Loaded, Pulling, CancelShot}
public class Bow : MonoBehaviour
{
    [SerializeField]
    PlayerController pc;

    [SerializeField]
    Animator anim;
    [SerializeField]
    GameObject arrowPrefab;
    [SerializeField]
    Transform arrowNotchTransform;
    Vector3 ogArrowNotchLocalPos;

    //should be arrow!
    GameObject _loadedArrow;

    //Vector3 ogArrowNotchPos;
    // Start is called before the first frame update
    [SerializeField]
    KeyCode loadArrowKey;
    [SerializeField]
    KeyCode pullAndReleaseArrowKey;

    BowState _currentBowState;

    [SerializeField]
    BowStats _bowStats;

    [SerializeField]
    float TEMP_armStrength;
    [SerializeField]
    float TEMP_perfectShotBonus;

    float _currentPullTime;
    float _currentPull;


    float _currentZoom;
    float _targetZoom;

    [SerializeField]
    AnimationCurve pullCurve;
    [SerializeField]
    AnimationCurve cancleCurve;

    Vector3 _canclePosition;
    [SerializeField]
    float _cancleShotSpeed;
    [SerializeField]
    MoveType[] disablingMoveTypes;
   
    Camera _cam; //TEMP AND BAD!

    [SerializeField]
    LayerMask layerMask;

    MoveType _currentMoveAnimation;
    private void Awake()
    {
        _currentBowState = BowState.Empty;
        ogArrowNotchLocalPos = arrowNotchTransform.localPosition;

        _cam = Camera.main; //TEMP AND BADDDD

        //_currentMoveAnimation = MoveType.Walk;


        if (!anim)
            anim = GetComponent<Animator>();

        _targetZoom = SpeedsAndSensitivities.BaseCameraFOV;
        SetFov(SpeedsAndSensitivities.BaseCameraFOV);
    }

    private void Start()
    {
        //MovementAnimation();
    }

    void SetFov(float zoom)
    {
        _currentZoom = zoom;
        _cam.fieldOfView = _currentZoom;
    }
    private void OnEnable()
    {
        SpeedsAndSensitivities.SetBowWeight(_bowStats.Weight);
    }
    private void OnDisable()
    {
        SpeedsAndSensitivities.SetBowWeight(0);
    }

    // Update is called once per frame
    void Update()
    {
        MovementAnimation();

        if (disablingMoveTypes.Length > 0 && disablingMoveTypes.Contains(pc.CurrentMoveType))
        {
            if(pc.CurrentMoveType == MoveType.Sprint)
            {
                anim.SetTrigger("Run");
            }
        }
        //RIGHT CLICK TO ZOOM!
        if(Input.GetMouseButton(1))
        {
            _targetZoom = _bowStats.AimAmount;
        }    
        else
        {
            _targetZoom = SpeedsAndSensitivities.BaseCameraFOV;
        }


        //RIGHT CLICK TO ZOOM!


        if(Mathf.Abs(_currentZoom - _targetZoom) >= .1f)
        {
            float delta = Time.deltaTime*_bowStats.ToAimTime;
            if(_currentZoom > _targetZoom)
            {
                delta *= -1f;
            }
            SetFov(_currentZoom + delta);
        }

        switch (_currentBowState)
        {
            case BowState.Empty:
                if (Input.GetKeyDown(loadArrowKey))
                    LoadArrow();
               
                

                break;
            case BowState.Loaded:
                if (Input.GetMouseButtonDown(0))
                {
                    _currentBowState = BowState.Pulling;
                    //Apply "Pulling-Weight"
                    SpeedsAndSensitivities.SetPullWeight(_bowStats.PullWeight);

                    //anim.SetTrigger("Aim");
                    anim.SetBool("IsAim", true);

                    //ZOOM BY SHOOTING!
                    //_targetZoom = _bowStats.AimAmount;
                   
                    _currentPull = 0;
                    _currentPullTime = 0;
                }
                break;
            case BowState.Pulling:
                if (Input.GetMouseButton(0))
                {
                    //_currentPull += TEMP_armStrength / _bowStats.PullResistence * Time.deltaTime;
                    _currentPull = TEMP_armStrength / _bowStats.PullResistence * _currentPullTime;
                    _currentPull = Mathf.Clamp(_currentPull, 0, _bowStats.MaxPull_Tension);

                    arrowNotchTransform.localPosition = ogArrowNotchLocalPos + Vector3.back * pullCurve.Evaluate(Mathf.Lerp(0, _bowStats.MaxPull_ArrowDistance, _currentPull / _bowStats.MaxPull_Tension));

                    _currentPullTime += Time.deltaTime; //so we start at 0
                }
                else //assuing now what the KeyUp - since it must be at first, this wont happen again afterwards
                {
                    if (_currentPull <= _bowStats.MinPull_Tension)
                    {
                        _currentBowState = BowState.CancelShot;



                        _canclePosition = arrowNotchTransform.localPosition;
                    }
                    else
                    {
                        _currentBowState = BowState.Empty; //Just fired
                        arrowNotchTransform.localPosition = ogArrowNotchLocalPos;
                        Release();

                        //anim.SetTrigger("ToIdle");
                        anim.SetBool("IsAim", false);
                        SpeedsAndSensitivities.SetPullWeight(0f);
                    }
                    //ZOOM BY SHOOTING!
                    //_targetZoom = SpeedsAndSensitivities.BaseCameraFOV;

                    //_loadedArrow -- shoot!
                }
                break;
            case BowState.CancelShot:
                {
                    //Penalty zone! can't do anything until arrow returns
                    arrowNotchTransform.localPosition = ogArrowNotchLocalPos + Vector3.back * pullCurve.Evaluate(Mathf.Lerp(0, _bowStats.MaxPull_ArrowDistance, _currentPull / _bowStats.MaxPull_Tension));
                    
                    _currentPull -= _cancleShotSpeed * Time.deltaTime;
                    SpeedsAndSensitivities.SetPullWeight(Mathf.Lerp(0f,_bowStats.PullWeight, _currentPull/_bowStats.MaxPull_Tension));


                    if (_currentPull <= 0)
                    {
                        arrowNotchTransform.localPosition = ogArrowNotchLocalPos;
                        _currentBowState = BowState.Loaded;
                        SpeedsAndSensitivities.SetPullWeight(0f);
                        //anim.SetTrigger("ToIdle");
                        anim.SetBool("IsAim", false);

                    }
                }
                break;
            default:
                break;
        }
    }

    private void MovementAnimation()
    {
        if (pc.IsGrounded)
        {
            Vector3 noFallVel = pc.GetVelocity;
            noFallVel.y = 0;
            float current = anim.GetFloat("MoveSpeed");
            float smoothed = Mathf.Lerp(current, noFallVel.magnitude / 10f, .03f);
            anim.SetFloat("MoveSpeed", smoothed);
        }
        else
        {
            //anim set fall speed?
            float current = anim.GetFloat("MoveSpeed");
            if (pc.GetVelocity.y > 0)
            {
                float smoothed = Mathf.Lerp(current, -1, .04f);
                anim.SetFloat("MoveSpeed", smoothed);
            }
            else
            {
                float smoothed = Mathf.Lerp(current, -2, .02f);
                anim.SetFloat("MoveSpeed", smoothed);
            }
        }
        //else
        //    anim.SetTrigger("Aim");

        //if (_currentMoveAnimation != pc.CurrentMoveType)
        //{
        //    switch (pc.CurrentMoveType)
        //    {
        //        case MoveType.Run:
        //            //temp - walk will have it's own animation.
        //            anim.SetTrigger("Run");
        //            break;
        //        //temp - walk will have it's own animation.
        //        case MoveType.Sprint:
        //            anim.SetTrigger("Sprint");
        //            break;
        //        //temp - walk will have it's own animation.
        //        case MoveType.Step:
        //            anim.SetTrigger("ToIdle");
        //            break;
        //        //temp - walk will have it's own animation.
        //        case MoveType.MidAir:
        //            anim.SetTrigger("ToIdle");
        //            break;
        //        //temp - walk will have it's own animation.
        //        case MoveType.Crouch:
        //            anim.SetTrigger("ToIdle");
        //            break;
        //        //temp - walk will have it's own animation.
        //        case MoveType.Prone:
        //            //temp - walk will have it's own animation.
        //            anim.SetTrigger("ToIdle");

        //            break;
        //        default:
        //            break;
        //    }
        //    _currentMoveAnimation = pc.CurrentMoveType;
        //}
    }

    void LoadArrow()
    {
        _currentBowState = BowState.Loaded;
        _loadedArrow = Instantiate(arrowPrefab, arrowNotchTransform);
    }

    void Pull()
    {

    }

    void Release()
    {
       
        if(_bowStats.IsPerfect(_currentPull))
        {
            Debug.Log("PERFECT SHOT!");
            _currentPull += TEMP_perfectShotBonus;
            //perfect shot sound and vfx
        }
        _loadedArrow.transform.GetChild(0).gameObject.layer= layerMask; //temp quick layer fix for cameras
        _loadedArrow.transform.SetParent(null);
        _loadedArrow.GetComponent<Arrow>().ForceMe(arrowNotchTransform.forward * _currentPull * _bowStats.PullFactor);
        _loadedArrow = null;
    }
}
