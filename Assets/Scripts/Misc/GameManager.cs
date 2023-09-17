using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameManager : SingletonMonobehaviour<GameManager>
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

    protected override void Awake()
    {
        base.Awake();

        Settings.Initialize();
    }
    
    void Start()
    {
        LoadData();
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
            SaveData();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveData();
        }
    }

    void OnApplicationQuit()
    {
        SaveData();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("complitedLevelsAmount", Settings.complitedLevelsAmount);
        PlayerPrefs.SetInt("lives", Settings.lives);

        PlayerPrefs.SetFloat("ambienceVolume", Settings.ambienceVolume);
        PlayerPrefs.SetFloat("sfxVolume", Settings.sfxVolume);
        PlayerPrefs.SetFloat("musicVolume", Settings.musicVolume);

        PlayerPrefs.SetInt("isFirstOpen", (Settings.isFirstOpen ? 1 : 0));
        PlayerPrefs.SetInt("isGameFinished", (Settings.isGameFinished ? 1 : 0));
        PlayerPrefs.SetInt("isJoystickFlexible", (Settings.isJoystickFlexible ? 1 : 0));
    }

    public void LoadData()
    {
        Settings.isFirstOpen = (PlayerPrefs.GetFloat("isFirstOpen") == 1);
        if (Settings.isFirstOpen)
        {
            Settings.isFirstOpen = false;
            return;
        }
        
        Settings.complitedLevelsAmount = PlayerPrefs.GetInt("complitedLevelsAmount");
        Settings.lives = PlayerPrefs.GetInt("lives");

        Settings.ambienceVolume = PlayerPrefs.GetFloat("ambienceVolume");
        Settings.sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        Settings.musicVolume = PlayerPrefs.GetFloat("musicVolume");
        


        Settings.isGameFinished = (PlayerPrefs.GetFloat("isGameFinished") == 1);
        Settings.isJoystickFlexible = (PlayerPrefs.GetFloat("isJoystickFlexible") == 1);
    }
}
