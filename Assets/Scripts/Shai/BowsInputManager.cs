using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowsInputManager : MonoBehaviour
{
    [SerializeField] private BowsLogic bowsLogic;

    // Update is called once per frame
    void Update()
    {
        ArrowUiInputDetection();
        ScrollSetChange();
    }

    private void ArrowUiInputDetection()
    {
        NumericalArrowChange();
        ScrollArrowChange();
    }

    private void NumericalArrowChange()
    {
        switch (Input.inputString)
        {
            case "1":
                Debug.Log("Tried to change to arrow 1");
                bowsLogic.ChangeCurrentArrowIndexByindex(0);
                break;
            case "2":
                Debug.Log("Tried to change to arrow 2");
                bowsLogic.ChangeCurrentArrowIndexByindex(1);
                break;
            case "3":
                Debug.Log("Tried to change to arrow 3");
                bowsLogic.ChangeCurrentArrowIndexByindex(2);
                break;
            case "4":
                Debug.Log("Tried to change to arrow 4");
                bowsLogic.ChangeCurrentArrowIndexByindex(3);
                break;
            case "5":
                Debug.Log("Tried to change to arrow 5");
                bowsLogic.ChangeCurrentArrowIndexByindex(4);
                break;
            case "6":
                Debug.Log("Tried to change to arrow 6");
                bowsLogic.ChangeCurrentArrowIndexByindex(5);
                break;
            case "7":
                Debug.Log("Tried to change to arrow 7");
                bowsLogic.ChangeCurrentArrowIndexByindex(6);
                break;
            case "8":
                Debug.Log("Tried to change to arrow 8");
                bowsLogic.ChangeCurrentArrowIndexByindex(7);
                break;
            case "9":
                Debug.Log("Tried to change to arrow 9");
                bowsLogic.ChangeCurrentArrowIndexByindex(8);
                break;
            case "0":
                Debug.Log("Tried to change to arrow 0");
                bowsLogic.ChangeCurrentArrowIndexByindex(9);
                break;
            default: // do something for other inputs
                break;
        }
    }

    private void ScrollArrowChange()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (scroll < 0)
            {
                Debug.Log("Tried to change to arrow by -1");
                bowsLogic.ChangeCurrentArrowIndexByStep(-1);
            }
            else if (scroll > 0)
            {
                Debug.Log("Tried to change to arrow by 1");
                bowsLogic.ChangeCurrentArrowIndexByStep(1);
            }
        }
    }

    private void ScrollSetChange()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (!Input.GetKey(KeyCode.LeftAlt))
        {
            if (scroll < 0)
            {
                Debug.Log("Tried to change set by -1");
                bowsLogic.ChangeCurrentSetIndexByStep(-1);
            }
            else if (scroll > 0)
            {
                Debug.Log("Tried to change set by 1");
                bowsLogic.ChangeCurrentSetIndexByStep(1);
            }
        }
    }
}
