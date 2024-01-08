using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarNumCtrl : MonoBehaviour
{
    [SerializeField] private ToolBarUI toolBar;


    private void Update()
    {
        //numerical input
        switch (Input.inputString)
        {
            case "1":
                Debug.Log("Tried to change to arrow 1");
                toolBar.ChangeCurrentSlotByIndex(0);
                break;
            case "2":
                Debug.Log("Tried to change to arrow 2");
                toolBar.ChangeCurrentSlotByIndex(1);
                break;
            case "3":
                Debug.Log("Tried to change to arrow 3");
                toolBar.ChangeCurrentSlotByIndex(2);
                break;
            case "4":
                Debug.Log("Tried to change to arrow 4");
                toolBar.ChangeCurrentSlotByIndex(3);
                break;
            case "5":
                Debug.Log("Tried to change to arrow 5");
                toolBar.ChangeCurrentSlotByIndex(4);
                break;
            case "6":
                Debug.Log("Tried to change to arrow 6");
                toolBar.ChangeCurrentSlotByIndex(5);
                break;
            case "7":
                Debug.Log("Tried to change to arrow 7");
                toolBar.ChangeCurrentSlotByIndex(6);
                break;
            case "8":
                Debug.Log("Tried to change to arrow 8");
                toolBar.ChangeCurrentSlotByIndex(7);
                break;
            case "9":
                Debug.Log("Tried to change to arrow 9");
                toolBar.ChangeCurrentSlotByIndex(8);
                break;
            case "0":
                Debug.Log("Tried to change to arrow 0");
                toolBar.ChangeCurrentSlotByIndex(9);
                break;
            default: // do something for other inputs
                break;
        }
    }
}
