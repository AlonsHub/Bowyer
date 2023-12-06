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


    //StudioEventEmitter allows us to change the data of emitters through code.
    //Emitters allow us to change data such as when to play and stop the sound + what the radius of the sound is (3D)
    private List<StudioEventEmitter> eventEmitters;


    //Create EventInstance for the ambient music of the world so that we can start + stop it AND change it's parameters
    private EventInstance monsterHunterAmbientInstance;

    private EventInstance worldMusic;

    [Header("Volume")]
    [Range(0, 1)]
    public float masterVol = 1;

    [Range(0, 1)]
    public float musicVol = 1;

    [Range(0, 1)]
    public float ambieceVol = 1;

    [Range(0, 1)]
    public float SFXVol = 1;

    //Bus is what it's called on FMOD for the "holder" of all the types of sounds for that BUS
    private Bus masterBus;
    private Bus musicBus;
    private Bus ambientBus;
    private Bus sfxBus;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Too many audiomanager instances!");
        }

        Instance = this;

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        //This is how we populate the refrences to the bus's in FMOD
        // master bus is empty after /
        // every other bus needs it's name after the /
        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/music");
        ambientBus = RuntimeManager.GetBus("bus:/ambience");
        sfxBus = RuntimeManager.GetBus("bus:/sfx");
    }

    private void Update()
    {
        //This is how we change the value of a bus on FMOD
        masterBus.setVolume(masterVol);
        musicBus.setVolume(musicVol);
        ambientBus.setVolume(ambieceVol);
        sfxBus.setVolume(SFXVol);
    }

    private void Start()
    {
        InitializeAmbience(FMODEvents.Instance.monsterHunterAmbience);
        InitializeMusic(FMODEvents.Instance.worldMusic);
    }
    private void InitializeAmbience(EventReference eventReference)
    {
        //create instance of ambient so that we can start and stop it when we want to
        monsterHunterAmbientInstance = CreateEventIsntace(eventReference);
        monsterHunterAmbientInstance.start();
    }

    private void InitializeMusic(EventReference eventReference)
    {
        //create instance of ambient so that we can start and stop it when we want to
        worldMusic = CreateEventIsntace(eventReference);
        worldMusic.start();
    }

    public void SetAmbienceParameter(string paramName, float paramValue)
    {
        //This is how we can change the parameters of sound on the EventInstance
        monsterHunterAmbientInstance.setParameterByName(paramName, paramValue);
    }

    public void SetMusicArea(MusicZones zone)
    {
        //This is how we can change the parameters of sound on the EventInstance
        worldMusic.setParameterByName("Area", (float)zone);
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

    public StudioEventEmitter CreateEventEmitter(EventReference eventReference, GameObject emitterGameObject, float minDistance, float maxDistance)
    {
        //This creates a "StudioEventEmitter" object that provides us with more control over the sound emitter
        //FMOD Studio Event Emitter is a component you can add to objects in the unity editor.
        //This takes an EventReference (Reference to the event in FMOD) in order to create the StudioEventEmitter
        //This function also takes a gameobject to get the GO the emitter is attached to.

        StudioEventEmitter emitter;

        emitterGameObject.TryGetComponent<StudioEventEmitter>(out emitter);

        if(!emitter)
        {
            Debug.LogError("No emitter component found on GO!");
        }

        //change the event reference to link to FMOD
        emitter.EventReference = eventReference;

        //This allows us to change the radius in which the sound work in
        emitter.OverrideAttenuation = true;
        emitter.OverrideMinDistance = minDistance;
        emitter.OverrideMaxDistance = maxDistance;

        eventEmitters.Add(emitter);
        return emitter;
    }

    private void CleanUp()
    {
        //EventInstances REQUIRE a cleanup on scene transition or on end of usage.
        //That is because they are persistant and will cause a memory leak if left alone.
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        //StudioEventEmitter REQUIRE a cleanup on scene transition or on end of usage.
        //That is because they are persistant and will continue playing unless they are stopped.
        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
