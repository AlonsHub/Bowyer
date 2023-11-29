using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    //EventInstance allows us to start and stop events of sound as needed.
    //requires using FMOD.Studio;
    private List<EventInstance> eventInstances;

    private void Awake()
    {
        if(Instance!=null)
        {
            Debug.LogError("Too many audiomanager instances!");
        }

        Instance = this;

        eventInstances = new List<EventInstance>();
    }


    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        //PlayOneShot means the sound event is triggired until it is finished automatically.
        //We have no control here on when to stop the sound - as that requires an EventInstance
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventIsntace(EventReference eventReference)
    {
        //This creates an "EventInstance" object that provides us with more control over the sound.
        //This takes an EventReference (Reference to the event in FMOD) in order to create the instance
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp()
    {
        //EventInstances REQUIRE a cleanup on scene transition or on end of usage.
        //That is because they are persistant and a
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
