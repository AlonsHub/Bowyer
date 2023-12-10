using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeymapSetter : MonoBehaviour
{
    //Runs when game starts and whenever settings are changed and saved

    [SerializeField]
    KeyCode[] movementKeys = new KeyCode[4]; //WASD
    [SerializeField]
    KeyCode jump;
    [SerializeField]
    KeyCode draw;
    [SerializeField]
    KeyCode aim; //focus/zoom
    [SerializeField]
    KeyCode sprint;
    [SerializeField]
    KeyCode crouch;
    [SerializeField]
    KeyCode interact;



    //ControlSchemeKeySetup
}
