using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    List<EventInstance> eventInstances;
    List<StudioEventEmitter> eventEmitters;

    [HideInInspector] public Dictionary<string, EventInstance> EventInstancesDict;

    bool calmMusicIsCurrentlyPlaying;

    Coroutine fadeOutCoroutine;

    protected override void Awake()
    {
        base.Awake();

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
        EventInstancesDict = new Dictionary<string, EventInstance>();

    }

    void Start()
    {
        EventInstancesDict.Add("Music", CreateInstance(FMODManager.Instance.Music));
        EventInstancesDict.Add("Amnbience", CreateInstance(FMODManager.Instance.Amnbience));

        //EventInstancesDict.Add("ButtonPress", CreateInstance(FMODManager.Instance.ButtonPress));
        //EventInstancesDict.Add("PlaySound", CreateInstance(FMODManager.Instance.PlaySound));
        //EventInstancesDict.Add("GameOverSound", CreateInstance(FMODManager.Instance.GameOverSound));

        EventInstancesDict.Add("PlayerStepSound", CreateInstance(FMODManager.Instance.PlayerStepSound));
        /*
        EventInstancesDict.Add("PlayerStepSoundOnWater", CreateInstance(FMODManager.Instance.PlayerStepSoundOnWater));

        EventInstancesDict.Add("DefIdle", CreateInstance(FMODManager.Instance.DefIdle));
        EventInstancesDict.Add("DefStepSound", CreateInstance(FMODManager.Instance.DefStepSound));
        EventInstancesDict.Add("DefStepSoundOnWater", CreateInstance(FMODManager.Instance.DefStepSoundOnWater));
        EventInstancesDict.Add("BlindIdle", CreateInstance(FMODManager.Instance.BlindIdle));
        EventInstancesDict.Add("BlindStepSound", CreateInstance(FMODManager.Instance.BlindStepSound));
        EventInstancesDict.Add("BlindStepSoundOnWater", CreateInstance(FMODManager.Instance.BlindStepSoundOnWater));
        */
        
        EventInstancesDict["Amnbience"].start();
    }

    public void SetParameter(string instanceName, string parameterName, float value)
    {
        EventInstancesDict[instanceName].setParameterByName(parameterName, value);
    }
    public void SetParameterWithCheck(string instanceName, string parameterName, float newValue)
    {
        float currentParameterValue;
        EventInstancesDict[parameterName].getParameterByName(parameterName, out currentParameterValue);

        if (currentParameterValue != newValue)
        {
            EventInstancesDict[parameterName].setParameterByName(parameterName, newValue);
        }
    }


    public void PlayOneShot(EventReference sound, Vector2 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }

    public void PlayOneShot(EventReference sound, Vector2 position, float volume)
    {
        FMOD.Studio.EventInstance soundEvent = FMODUnity.RuntimeManager.CreateInstance(sound);
        soundEvent.setVolume(volume);
        soundEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));

        soundEvent.start();
        soundEvent.release();
    }


    public EventInstance CreateInstance(EventReference sound)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        eventInstances.Add(eventInstance);

        return eventInstance;
    }

    public StudioEventEmitter initializeEventEmitter(EventReference eventReference, GameObject emitterGameO)
    {
        StudioEventEmitter emitter = emitterGameO.GetComponent<StudioEventEmitter>();

        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);

        return emitter;
    }

    public void SetSFXVolume(float volume)
    {
        RuntimeManager.GetBus("bus:/SFX").setVolume(volume);
        Settings.sfxVolume = volume;
    }

    public void SetAmbienceVolume(float volume)
    {
        RuntimeManager.GetBus("bus:/AMBIENCE").setVolume(volume);
        Settings.ambienceVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        RuntimeManager.GetBus("bus:/MUSIC").setVolume(volume);
        Settings.musicVolume = volume;
    }

    IEnumerator FadeOutMusic(EventInstance music)
    {
        float timer = 1f;

        while (timer > 0)
        {
            music.setVolume(timer);
            timer -= Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator FadeInMusic(EventInstance music)
    {
        float timer = 0;

        while (timer < 1)
        {
            music.setVolume(timer);
            timer += Time.deltaTime;

            yield return null;
        }
    }
}