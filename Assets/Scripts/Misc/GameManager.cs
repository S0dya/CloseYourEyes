using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>, ISaveable
{
    [HideInInspector] public bool isMenuOpen;
    [HideInInspector] public bool isInGame;

    public int[] visionTime;


    GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }


    protected override void Awake()
    {
        base.Awake();
        GameObjectSave = new GameObjectSave();

        Settings.Initialize();
        Settings.complitedLevelsAmount = 14;
        Settings.lives = 1;
    }

    void Start()
    {
        SaveManager.Instance.LoadDataFromFile();
    }

    public void LevelComplete()
    {
        Settings.complitedLevelsAmount++;
    }

    public IEnumerator Timer(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (isMenuOpen)
            {
                yield return null;
            }
            else
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }


    
    
    
    
    
    
    
    void OnEnable()
    {
        ISaveableRegister();
    }
    void OnDisable()
    {
        ISaveableDeregister();
    }

    public void ISaveableRegister()
    {
        SaveManager.Instance.iSaveableObjectList.Add(this);
    }

    public void ISaveableDeregister()
    {
        SaveManager.Instance.iSaveableObjectList.Remove(this);
    }

    public GameObjectSave ISaveableSave()
    {
        GameObjectSave.sceneData.Remove(Settings.GameScene);

        SceneSave sceneSave = new SceneSave();

        sceneSave.intDictionary = new Dictionary<string, int>();
        sceneSave.floatDictionary = new Dictionary<string, float>();
        sceneSave.boolDictionary = new Dictionary<string, bool>();

        sceneSave.intDictionary.Add("complitedLevelsAmount", Settings.complitedLevelsAmount);
        sceneSave.intDictionary.Add("lives", Settings.lives);

        sceneSave.floatDictionary.Add("ambienceVolume", Settings.ambienceVolume);
        sceneSave.floatDictionary.Add("sfxVolume", Settings.sfxVolume);
        sceneSave.floatDictionary.Add("musicVolume", Settings.musicVolume);

        sceneSave.boolDictionary.Add("isGameFinished", Settings.isGameFinished);
        sceneSave.boolDictionary.Add("isJoystickFlexible", Settings.isJoystickFlexible);
        
        GameObjectSave.sceneData.Add(Settings.GameScene, sceneSave);
        return GameObjectSave;
    }


    public void ISaveableLoad(GameObjectSave gameObjectSave)
    {
        if (gameObjectSave.sceneData.TryGetValue(Settings.GameScene, out SceneSave sceneSave))
        {
            if (sceneSave.intDictionary != null)
            {
                if (sceneSave.intDictionary.TryGetValue("complitedLevelsAmount", out int complitedLevelsAmount))
                {
                    Settings.complitedLevelsAmount = complitedLevelsAmount;
                }
                if (sceneSave.intDictionary.TryGetValue("lives", out int lives))
                {
                    Settings.lives = lives;
                }
            }
            if (sceneSave.floatDictionary != null)
            {
                if (sceneSave.floatDictionary.TryGetValue("ambienceVolume", out float ambienceVolume))
                {
                    Settings.ambienceVolume = ambienceVolume;
                }
                if (sceneSave.floatDictionary.TryGetValue("sfxVolume", out float sfxVolume))
                {
                    Settings.sfxVolume = sfxVolume;
                }
                if (sceneSave.floatDictionary.TryGetValue("musicVolume", out float musicVolume))
                {
                    Settings.musicVolume = musicVolume;
                }
            }
            if (sceneSave.boolDictionary != null)
            {
                if (sceneSave.boolDictionary.TryGetValue("isGameFinished", out bool isGameFinished))
                {
                    Settings.isGameFinished = isGameFinished;
                }
                if (sceneSave.boolDictionary.TryGetValue("isJoystickFlexible", out bool isJoystickFlexible))
                {
                    Settings.isJoystickFlexible = isJoystickFlexible;
                }
            }
        }
    }

    public void ISaveableStoreScene(string sceneName)
    {
    }


    public void ISaveableRestoreScene(string sceneName)
    {
    }
}
