using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents Instance;

    //event reference = a reference to the event itself in FMOD for the system to know which event to work with


    //if you have public get and private set then it doesn't show in inspector unless you add the "field:" syntax
    [field: Header("Pickup SFX")]
    [field: SerializeField] public EventReference itemCollected { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }
    [field: SerializeField] public EventReference playerFootstepsSprint { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Too many audiomanager instances!");
        }

        Instance = this;
    }

}
