using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarScrollCtrl : MonoBehaviour
{
    [SerializeField] private ToolBarUI toolBar;


    private void Update()
    {
        //scroll input
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (scroll < 0)
            {
                toolBar.ChangeCurrentSlotByStep(-1);
            }
            else if (scroll > 0)
            {
                toolBar.ChangeCurrentSlotByStep(1);
            }
        }
    }
}

