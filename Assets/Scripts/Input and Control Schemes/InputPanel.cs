using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MAy need to be a parent class to inherit from...
public interface InputPanel
{
    //Panel active mode

    //Listen for any amount of closing keys

    //scheme of keys 
    //scheme of actions

    //loop over keys and connect their Button/Down/Up inputs to relevant actions


    void GrabInput();
    bool IsInputPanelEnabled();
}
