using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum BowState { Empty, Loaded, Pulling, CancelShot}
public class Bow : MonoBehaviour
{
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

    float _currentPull;

    [SerializeField]
    AnimationCurve pullCurve;
    [SerializeField]
    AnimationCurve cancleCurve;

    Vector3 _canclePosition;
    [SerializeField]
    float _cancleShotSpeed;
    private void Awake()
    {
        _currentBowState = BowState.Empty;
        ogArrowNotchLocalPos = arrowNotchTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
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
                    _currentPull = 0;
                }
                break;
            case BowState.Pulling:
                if (Input.GetMouseButton(0))
                {
                    _currentPull += TEMP_armStrength / _bowStats.PullResistence * Time.deltaTime;
                    _currentPull = Mathf.Clamp(_currentPull, 0, _bowStats.MaxPull_Tension);

                    //arrowNotchTransform.localPosition = ogArrowNotchLocalPos + (Vector3.back * _currentPull*Time.deltaTime);
                    arrowNotchTransform.localPosition = ogArrowNotchLocalPos + Vector3.back * pullCurve.Evaluate(Mathf.Lerp(0, _bowStats.MaxPull_ArrowDistance, _currentPull / _bowStats.MaxPull_Tension));
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
                    }
                    //_loadedArrow -- shoot!
                }
                break;
            case BowState.CancelShot:
                {
                    //Penalty zone! can't do anything until arrow returns
                    arrowNotchTransform.localPosition = ogArrowNotchLocalPos + Vector3.back * pullCurve.Evaluate(Mathf.Lerp(0, _bowStats.MaxPull_ArrowDistance, _currentPull / _bowStats.MaxPull_Tension));
                    //arrowNotchTransform.localPosition = Vector3.Lerp( ogArrowNotchLocalPos, _canclePosition, pullCurve.Evaluate(_currentPull/_bowStats.MaxPull_Tension)); //may need to flip this curve
                    _currentPull -= _cancleShotSpeed * Time.deltaTime;

                    if(_currentPull <= 0)
                    {
                        arrowNotchTransform.localPosition = ogArrowNotchLocalPos;
                        _currentBowState = BowState.Loaded;
                    }
                }
                break;
            default:
                break;
        }
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

        _loadedArrow.transform.SetParent(null);
        //_loadedArrow.GetComponent<Arrow>().ForceMeFWD(_currentPull * _bowStats.PullFactor);
        _loadedArrow.GetComponent<Arrow>().ForceMe(arrowNotchTransform.forward * _currentPull * _bowStats.PullFactor);
        _loadedArrow = null;
    }
}
