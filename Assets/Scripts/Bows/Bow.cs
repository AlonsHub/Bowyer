using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum BowState { Empty, Loaded, Pulling, CancelShot}
public class Bow : MonoBehaviour
{
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

   
    Camera _cam; //TEMP AND BAD!
    private void Awake()
    {
        _currentBowState = BowState.Empty;
        ogArrowNotchLocalPos = arrowNotchTransform.localPosition;

        _cam = Camera.main; //TEMP AND BADDDD

        if (!anim)
            anim = GetComponent<Animator>();

        _targetZoom = SpeedsAndSensitivities.BaseCameraFOV;
        SetFov(SpeedsAndSensitivities.BaseCameraFOV);
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
                    //if(_releaseCoro != null)
                    //{
                    //    StopCoroutine(_releaseCoro);
                    //    _releaseCoro = null;
                    //    if(_currentZoom != SpeedsAndSensitivities.BaseCameraFOV)
                    //    {
                    //        _tempZoom = _currentZoom;
                    //    }
                    //}
                    //else
                    //{
                    //    _tempZoom = SpeedsAndSensitivities.BaseCameraFOV;
                    //}
                    anim.SetTrigger("Aim");
                    _targetZoom = _bowStats.AimAmount;
                    //if (_takeCoro != null)
                    //{
                    //    StopCoroutine(_takeCoro);
                    //    _takeCoro = null;
                    //}
                    //if(_releaseCoro!= null)
                    //{
                    //    StopCoroutine(_releaseCoro);
                    //    _releaseCoro = null;
                    //}
                    //_takeCoro = StartCoroutine(TakeAim());
                    //Set to aiming anim here? pretty sure... also need to do camera stuff, consider something else here

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

                    //if (_currentPullTime <= _bowStats.ToAimTime)
                    //{
                    //    //_currentZoom = Mathf.Lerp(SpeedsAndSensitivities.BaseCameraFOV, _bowStats.AimAmount, _currentPullTime / _bowStats.ToAimTime);
                    //    _currentZoom = Mathf.Lerp(_tempZoom, _bowStats.AimAmount, _currentPullTime / _bowStats.ToAimTime);
                    //    _cam.fieldOfView = _currentZoom;
                    //}

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
                        //if (_takeCoro != null)
                        //{
                        //    StopCoroutine(_takeCoro);
                        //    _takeCoro = null;
                        //}
                        //if (_releaseCoro != null)
                        //{
                        //    StopCoroutine(_releaseCoro);
                        //    _releaseCoro = null;
                        //}
                        //_releaseCoro = StartCoroutine(ReleaseAim());
                        
                        anim.SetTrigger("ToIdle");
                        SpeedsAndSensitivities.SetPullWeight(0f);
                    }
                    _targetZoom = SpeedsAndSensitivities.BaseCameraFOV;

                    //_loadedArrow -- shoot!
                }
                break;
            case BowState.CancelShot:
                {
                    //Penalty zone! can't do anything until arrow returns
                    arrowNotchTransform.localPosition = ogArrowNotchLocalPos + Vector3.back * pullCurve.Evaluate(Mathf.Lerp(0, _bowStats.MaxPull_ArrowDistance, _currentPull / _bowStats.MaxPull_Tension));
                    //arrowNotchTransform.localPosition = Vector3.Lerp( ogArrowNotchLocalPos, _canclePosition, pullCurve.Evaluate(_currentPull/_bowStats.MaxPull_Tension)); //may need to flip this curve
                    _currentPull -= _cancleShotSpeed * Time.deltaTime;
                    SpeedsAndSensitivities.SetPullWeight(Mathf.Lerp(0f,_bowStats.PullWeight, _currentPull/_bowStats.MaxPull_Tension));


                        ////_currentZoom = Mathf.Lerp(SpeedsAndSensitivities.BaseCameraFOV, _bowStats.AimAmount, _currentPull / _bowStats.MaxPull_Tension);
                        //_currentZoom = Mathf.Lerp(SpeedsAndSensitivities.BaseCameraFOV, _tempZoom, _currentPull / _bowStats.MaxPull_Tension);
                        //_cam.fieldOfView = _currentZoom;
                    

                    if (_currentPull <= 0)
                    {
                        arrowNotchTransform.localPosition = ogArrowNotchLocalPos;
                        _currentBowState = BowState.Loaded;
                        SpeedsAndSensitivities.SetPullWeight(0f);
                        anim.SetTrigger("ToIdle");
                    }
                }
                break;
            default:
                break;
        }
    }

    //IEnumerator ReleaseAim()
    //{
    //    float t = 0;
    //    _tempZoom = _currentZoom;
    //    t = (SpeedsAndSensitivities.BaseCameraFOV-_tempZoom) / ( SpeedsAndSensitivities.BaseCameraFOV - _bowStats.AimAmount);
    //    while (t <= _bowStats.FromAimTime)
    //    {
    //        _currentZoom = Mathf.Lerp(_tempZoom, SpeedsAndSensitivities.BaseCameraFOV, t / _bowStats.FromAimTime);
    //        _cam.fieldOfView = _currentZoom;
    //        yield return null;
    //        t+= Time.deltaTime; //so we start at 0
    //    }
    //}
    //IEnumerator TakeAim()
    //{
    //    float t = 0;
    //    //_tempZoom = _currentZoom;
    //    t = (SpeedsAndSensitivities.BaseCameraFOV - _tempZoom) / (SpeedsAndSensitivities.BaseCameraFOV - _bowStats.AimAmount);
    //    while (t <= _bowStats.FromAimTime)
    //    {
    //        _currentZoom = Mathf.Lerp(_tempZoom, _bowStats.AimAmount, t / _bowStats.FromAimTime);
    //        _cam.fieldOfView = _currentZoom;
    //        yield return null;
    //        t+= Time.deltaTime; //so we start at 0
    //    }
    //}


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

        _loadedArrow.transform.SetParent(null);
        //_loadedArrow.GetComponent<Arrow>().ForceMeFWD(_currentPull * _bowStats.PullFactor);
        _loadedArrow.GetComponent<Arrow>().ForceMe(arrowNotchTransform.forward * _currentPull * _bowStats.PullFactor);
        _loadedArrow = null;
    }
}
