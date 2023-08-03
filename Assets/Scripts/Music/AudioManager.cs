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
    Coroutine randomSFXCor;

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
        EventInstancesDict.Add("RandomSFX", CreateInstance(FMODManager.Instance.RandomSFX));
        EventInstancesDict.Add("Ambience", CreateInstance(FMODManager.Instance.Ambience));
        EventInstancesDict.Add("Rain", CreateInstance(FMODManager.Instance.Rain));

        EventInstancesDict.Add("ButtonPress", CreateInstance(FMODManager.Instance.ButtonPress));
        //EventInstancesDict.Add("PlaySound", CreateInstance(FMODManager.Instance.PlaySound));
        //EventInstancesDict.Add("GameOverSound", CreateInstance(FMODManager.Instance.GameOverSound));

        EventInstancesDict.Add("PlayerStepSound", CreateInstance(FMODManager.Instance.PlayerStepSound));
        EventInstancesDict.Add("DieSound", CreateInstance(FMODManager.Instance.DieSound));

        /*
        EventInstancesDict.Add("PlayerStepSoundOnWater", CreateInstance(FMODManager.Instance.PlayerStepSoundOnWater));
        */
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


    public void PlayOneShot(string sound)
    {
        EventInstancesDict[sound].start();
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

    public void StopAllEmitters()
    {
        foreach (var emitter in eventEmitters)
        {
            if (emitter != null && emitter.IsPlaying())
            {
                emitter.Stop();
            }
        }
    }

    public void SetCurSFX()
    {
        float volume = 0f;
        RuntimeManager.GetBus("bus:/SFX").getVolume(out volume);
        Settings.curSfxVolume = volume;
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


    IEnumerator RandomSFXCor()
    {
        while (true)
        {
            yield return GameManager.Instance.StartCoroutine(GameManager.Instance.Timer(Random.Range(70, 150)));

            EventInstancesDict["RandomSFX"].start();
        }
    }
    

    public void ToggleSFX(bool val)
    {
        if (val)
        {
            RuntimeManager.GetBus("bus:/SFX").setVolume(Settings.curSfxVolume);
        }
        else
        {
            float volume = 0f;
            RuntimeManager.GetBus("bus:/SFX").getVolume(out volume);
            Settings.curSfxVolume = volume;
            RuntimeManager.GetBus("bus:/SFX").setVolume(0);
        }
    }

    public void ToggleRandomSFX(bool val)
    {
        if (randomSFXCor != null)
        {
            StopCoroutine(randomSFXCor);
        }

        if (val)
        {
            randomSFXCor = StartCoroutine(RandomSFXCor());
        }
    }

    public void ToggleAmbience(bool val)
    {
        if (val)
        {
            EventInstancesDict["Ambience"].start();
        }
        else
        {
            EventInstancesDict["Ambience"].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
    public void ToggleMusic(bool val)
    {
        if (val)
        {
            EventInstancesDict["Music"].start();
        }
        else
        {
            EventInstancesDict["Music"].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
    public void ToggleMusicWithRain(bool val)
    {
        if (val)
        {
            EventInstancesDict["Music"].start();
        }
        else
        {
            EventInstancesDict["Music"].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}