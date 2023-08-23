using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum BowState { Empty, Loaded, Pulling}
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
    float pullForceSpeed;
    [SerializeField]
    float maxPullForceAmount;

    float _currentPull;
    [SerializeField]
    float pullFactor;
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
                if(Input.GetMouseButton(0))
                {
                    _currentPull += pullForceSpeed * Time.deltaTime;
                    _currentPull = Mathf.Clamp(_currentPull, 0, maxPullForceAmount);

                    arrowNotchTransform.localPosition = ogArrowNotchLocalPos + (Vector3.back * _currentPull*Time.deltaTime);

                    //_loadedArrow.transform.position = arrowNotchTransform.position + (arrowNotchTransform.forward * -1f * _currentPull*Time.deltaTime);
                }
                else //assuing now what the KeyUp - since it must be at first, this wont happen again afterwards
                {
                    _currentBowState = BowState.Empty; //Just fired
                    arrowNotchTransform.localPosition = ogArrowNotchLocalPos;
                    Release();
                    //_loadedArrow -- shoot!
                }
                break;
            default:
                break;
        }
    }

    void LoadArrow()
    {
        _currentBowState = BowState.Loaded;
        //_loadedArrow = Instantiate(arrowPrefab, arrowNotchTransform.position, arrowNotchTransform.rotation);
        _loadedArrow = Instantiate(arrowPrefab, arrowNotchTransform);
    }

    void Pull()
    {

    }

    void Release()
    {
        _loadedArrow.transform.SetParent(null);
        _loadedArrow.GetComponent<Arrow>().ForceMeFWD(_currentPull * pullFactor);
        _loadedArrow = null;
    }
}
