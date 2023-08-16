using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameManager : SingletonMonobehaviour<GameManager>, ISaveable
{
    [HideInInspector] public bool isMenuOpen;
    [HideInInspector] public bool isInGame;
    [HideInInspector] public bool inMenu;
    [HideInInspector] public bool inLevels;
    [HideInInspector] public bool inSettings;
    public GameObject inputUI;

    

    [HideInInspector] public bool isBlindFollowingPlayer;
    [HideInInspector] public bool isDefFollowingPlayer;

    public int[] visionTime;


    GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }


    protected override void Awake()
    {
        base.Awake();
        GameObjectSave = new GameObjectSave();

        Settings.Initialize();
    }
    
    void Start()
    {
        SaveManager.Instance.LoadDataFromFile();
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (inMenu)
            {
                Menu.Instance.ExitButton();
            }
            else if (inLevels)
            {
                Menu.Instance.PlayButton();
            }
            else if (inSettings)
            {
                Menu.Instance.SettingsButton();
            }
            else if (GameMenu.Instance.inGame)
            {
                GameMenu.Instance.PauseButton();
            }
            else if (GameMenu.Instance.inMenu)
            {
                GameMenu.Instance.CloseGameMenu();
            }
            else if (GameMenu.Instance.inRewardedAd)
            {
                GameMenu.Instance.CloseRewardAdButton();
            }
            else if (GameMenu.Instance.inGameOver)
            {
                GameMenu.Instance.HomeButton();
            }
        }
            
    }

    public void LevelComplete()
    {
        if (Settings.curSceneNum > Settings.complitedLevelsAmount)
        {
            Settings.lives = Math.Min(Settings.lives+1, 3);
            Settings.complitedLevelsAmount = Settings.curSceneNum-1;
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveManager.Instance.SaveDataToFile();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveManager.Instance.SaveDataToFile();
        }
    }

    void OnApplicationQuit()
    {
        SaveManager.Instance.SaveDataToFile();
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
}
