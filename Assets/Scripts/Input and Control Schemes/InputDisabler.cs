using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDisabler : MonoBehaviour
{
   
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerController.ActionInputPanelsEnabled = !PlayerController.ActionInputPanelsEnabled;
        }
    }
}
