using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

enum BowState { Empty, Loaded, Pulling, CancelShot}
public enum BowType { Short, Recurve, Long}
public class Bow : MonoBehaviour, InputPanel
{
    
    
    [SerializeField]
    PlayerController pc;

    [SerializeField]
    Animator anim;
    [SerializeField]
    GameObject arrowPrefab;
    [SerializeField]
    Transform arrowNotchTransform;
    [SerializeField]
    Transform shotTransform;
    //Vector3 ogArrowNotchLocalPos;

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
    [SerializeField]
    float TEMP_drawSpeed;
    [SerializeField]
    float TEMP_reloadSpeed;

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


    public UnityEvent OnShoot;


    //temp?
    bool _cancelShot = false;
    bool _canShoot = false;

    private void Awake()
    {
        _currentBowState = BowState.Empty;
        //ogArrowNotchLocalPos = arrowNotchTransform.localPosition;

        _cam = Camera.main; //TEMP AND BADDDD

        if (!anim)
            anim = GetComponent<Animator>();

    }

    private void Start()
    {
        _targetZoom = SpeedsAndSensitivities.BaseCameraFOV;
        SetFov(SpeedsAndSensitivities.BaseCameraFOV);
        //MovementAnimation();
    }

    void SetFov(float zoom)
    {
        _currentZoom = zoom;
        _cam.fieldOfView = _currentZoom;
    }
    private void OnEnable()
    {
        if (SpeedsAndSensitivities.Instance)
            SpeedsAndSensitivities.SetBowWeight(_bowStats.Weight);

        anim.SetFloat("DrawSpeed", TEMP_drawSpeed);
        anim.SetFloat("ReloadSpeed", TEMP_reloadSpeed);
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
            if (pc.CurrentMoveType == MoveType.Sprint)
            {
                anim.SetTrigger("Run");
            }
        }
        GrabInput();
    }

    public void GrabInput()
    {
        if (!IsEnabled())
            return;

        //RIGHT CLICK TO ZOOM!
        if (Input.GetMouseButton(1))
        {
            _targetZoom = _bowStats.AimAmount;
        }
        else
        {
            _targetZoom = SpeedsAndSensitivities.BaseCameraFOV;
        }


        //RIGHT CLICK TO ZOOM!


        if (Mathf.Abs(_currentZoom - _targetZoom) >= .1f)
        {
            float delta = Time.deltaTime * _bowStats.ToAimTime;
            if (_currentZoom > _targetZoom)
            {
                delta *= -1f;
            }
            SetFov(_currentZoom + delta);
        }

        switch (_currentBowState)
        {
            case BowState.Empty:
                //if (Temp_KeyMapper.ToggleOrHold && Input.GetKeyDown(Temp_KeyMapper.GetKeycodeForInputAction(InputActions.Reload))) //Input Version 2, for keys like R
                if (Temp_KeyMapper.ToggleOrHold) //Input Version 2, but for mouse button
                {
                    if(Input.GetMouseButtonDown(0))
                    anim.SetTrigger("Reload");
                    //LoadArrow(); //loads current arrow, assuming the correct one has been preloaded to the prefab? should pull from quiver really
                }
               
                //else
                //{
                //    //_currentBowState = BowState.Loaded;
                //    if (Input.GetMouseButton(0))
                //    {

                //        _currentBowState = BowState.Pulling;
                //        //Apply "Pulling-Weight"
                //        SpeedsAndSensitivities.SetPullWeight(_bowStats.PullWeight);

                //        anim.SetTrigger("Aim");
                //        //anim.SetBool("IsAim", true);
                //        //ZOOM BY SHOOTING!
                //        //_targetZoom = _bowStats.AimAmount;

                //        _currentPull = 0;
                //        _currentPullTime = 0;
                //    }
                //}




                break;
            case BowState.Loaded:
                //if (_cancelShot ? Input.GetMouseButtonDown(0) : Input.GetMouseButton(0))
                if (_canShoot && Input.GetMouseButton(0))
                {
                    _currentBowState = BowState.Pulling;
                    //Apply "Pulling-Weight"
                    SpeedsAndSensitivities.SetPullWeight(_bowStats.PullWeight);

                    anim.SetTrigger("Aim");
                    //anim.SetBool("IsAim", true);
                    //ZOOM BY SHOOTING!
                    //_targetZoom = _bowStats.AimAmount;

                    _currentPull = 0;
                    _currentPullTime = 0;
                }
                break;
            case BowState.Pulling:

                //anim.ResetTrigger("Aim");
                _canShoot = true;
                //_cancelShot = false;

                if (Input.GetMouseButton(0))
                {

                    if(Input.GetKeyDown(Temp_KeyMapper.GetKeycodeForInputAction(InputActions.CancelShot)))
                    {
                        _currentBowState = BowState.CancelShot;
                        //_cancelShot = true;
                        _canShoot = false;

                        return;
                    }

                    //_currentPull += TEMP_armStrength / _bowStats.PullResistence * Time.deltaTime;
                    _currentPull = TEMP_armStrength / _bowStats.PullResistence * _currentPullTime;
                    _currentPull = Mathf.Clamp(_currentPull, 0, _bowStats.MaxPull_Tension);


                    //arrowNotchTransform.localPosition = ogArrowNotchLocalPos + Vector3.back * pullCurve.Evaluate(Mathf.Lerp(0, _bowStats.MaxPull_ArrowDistance, _currentPull / _bowStats.MaxPull_Tension));
                    if(_currentPullTime >= _bowStats.armStats.shakeTime)
                    anim.SetFloat("DrawTime", (_currentPullTime - _bowStats.armStats.shakeTime));
                    else
                    anim.SetFloat("DrawTime", 0);


                    if(_currentPullTime >= _bowStats.armStats.releaseTime)
                    {
                        _currentBowState = BowState.Empty; //Just fired

                        anim.SetTrigger("Release");

                        Release();

                        SpeedsAndSensitivities.SetPullWeight(0f);
                    }

                    _currentPullTime += Time.deltaTime; //so we start at 0
                }
                else //assuing KeyUp - since it must be at first, this wont happen again afterwards
                {
                    if (_currentPull <= _bowStats.MinPull_Tension)
                    {
                        _currentBowState = BowState.CancelShot;



                        _canclePosition = arrowNotchTransform.localPosition;
                    }
                    else
                    {
                        _currentBowState = BowState.Empty; //Just fired
                        //arrowNotchTransform.localPosition = ogArrowNotchLocalPos;

                        anim.SetTrigger("Release");

                        Release();

                        //anim.SetTrigger("ToIdle");
                        //anim.SetBool("IsAim", false);
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
                    //arrowNotchTransform.localPosition = ogArrowNotchLocalPos + Vector3.back * pullCurve.Evaluate(Mathf.Lerp(0, _bowStats.MaxPull_ArrowDistance, _currentPull / _bowStats.MaxPull_Tension));

                    _canShoot = false;


                    _currentPull -= _cancleShotSpeed * Time.deltaTime;
                    SpeedsAndSensitivities.SetPullWeight(Mathf.Lerp(0f, _bowStats.PullWeight, _currentPull / _bowStats.MaxPull_Tension));


                    if (_currentPull <= 0)
                    {
                        //arrowNotchTransform.localPosition = ogArrowNotchLocalPos;
                        _currentBowState = BowState.Loaded;
                        SpeedsAndSensitivities.SetPullWeight(0f);
                        _currentPull = 0;
                        _currentPullTime = 0;

                        anim.SetTrigger("ToIdle");
                        _canShoot = true;

                        //anim.SetBool("IsAim", false);

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
            float smoothed = Mathf.Lerp(current, noFallVel.magnitude / 5f, .05f);
            anim.SetFloat("MoveSpeed", smoothed);
        }
        else
        {
            //anim set fall speed?
            float current = anim.GetFloat("MoveSpeed");
            if (pc.GetVelocity.y > 0)
            {
                float smoothed = Mathf.Lerp(current, -1, .08f);
                anim.SetFloat("MoveSpeed", smoothed);
            }
            else
            {
                float smoothed = Mathf.Lerp(current, -2, .04f);
                anim.SetFloat("MoveSpeed", smoothed);
            }
        }
    }

    public void LoadArrow()
    {
        if(_loadedArrow)
        {
            Debug.LogError("Trying to Double Load arrows - stop this");
            return;
        }
        _currentBowState = BowState.Loaded;
        _loadedArrow = Instantiate(arrowPrefab, arrowNotchTransform);
        _loadedArrow.transform.localEulerAngles = new Vector3(0, -90, 0);

        //_canShoot = true;
    }


    public void CallLoadArrow() //called by animation event
    {
        if (Temp_KeyMapper.ToggleOrHold)
            return;

        LoadArrow();

        //_currentBowState = BowState.Loaded;
        //if (_loadedArrow)
        //{
        //    Destroy(_loadedArrow);
        //}
        //_loadedArrow = Instantiate(arrowPrefab, arrowNotchTransform);
        //_loadedArrow.transform.localEulerAngles = new Vector3(0, -90, 0);
    }

    public void CallReadyToShoot()
    {
        if(_loadedArrow)
        _canShoot = true;
    }

    public void LoadArrow(GameObject newArrowPrefab)
    {
        arrowPrefab = newArrowPrefab;

        LoadArrow();

        //_currentBowState = BowState.Loaded;

        //if (_loadedArrow)
        //{
        //    Destroy(_loadedArrow);
        //}
        //_loadedArrow = Instantiate(arrowPrefab, arrowNotchTransform);
        //_loadedArrow.transform.localEulerAngles = new Vector3(0, -90, 0);

    }

    void Pull()
    {

    }

    public void Release()
    {
        if (_bowStats.IsPerfect(_currentPull))
        {
            Debug.Log("PERFECT SHOT!");
            _currentPull += TEMP_perfectShotBonus;
            //perfect shot sound and vfx
        }
        _loadedArrow.transform.GetChild(0).gameObject.layer= layerMask; //temp quick layer fix for cameras
        _loadedArrow.transform.SetParent(null);


        OnShoot?.Invoke();

        //if(!Temp_KeyMapper.ToggleOrHold)
        //anim.SetTrigger("Reload");
        //Vector3 cleanFwd = arrowNotchTransform.right * -1f;
        //cleanFwd.y = 0;
        //_loadedArrow.transform.forward = shotTransform.forward;
        _loadedArrow.GetComponent<Arrow>().ForceMe(Camera.main.transform.forward * _currentPull * _bowStats.PullFactor);
        _loadedArrow = null;

        _currentBowState = BowState.Empty;
    }


    public bool IsEnabled()
    {
        return PlayerController.ActionInputPanelsEnabled;
    }

    public void DisableMe()
    {
        OnShoot.RemoveAllListeners();
        gameObject.SetActive(false);
    }

    public void TrySetJump()
    {
        if (_currentBowState == BowState.Pulling)
            return;

        anim.SetTrigger("Jump");
    }
    public void Holster()
    {
        //if (_currentBowState == BowState.Pulling)
        //    return;

        anim.SetTrigger("Holster");
    }

    public void SetAnimInAir(bool isInAir)
    {
        anim.SetBool("InAir", isInAir);
    }
    public void ReportLanded()
    {
        pc.LandOnGround();
    }

    public void SetAnimatorController(RuntimeAnimatorController runtimeAnimatorController)
    {
        anim.runtimeAnimatorController = runtimeAnimatorController;
    }
}
