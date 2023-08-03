using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameMenu : SingletonMonobehaviour<GameMenu>
{
    //delater
    Light2D globalLight;
    [SerializeField] GameObject gameMenu;
    [SerializeField] GameObject inGameMenu;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject rePlayButton;
    [SerializeField] GameObject nextLevelButton;
    [SerializeField] GameObject rewardedAdBar;

    bool canReplay;

    GameObject[] defEnemyArr;
    GameObject[] blindEnemArr;

    protected override void Awake()
    {
        base.Awake();

        defEnemyArr = GameObject.FindGameObjectsWithTag("DefEnemy");
        blindEnemArr = GameObject.FindGameObjectsWithTag("BlindEnemy");

        globalLight = GameObject.FindGameObjectWithTag("REMOVELATER").GetComponent<Light2D>();
        globalLight.intensity = 0f;
    }

    void Start()
    {
        AudioManager.Instance.ToggleSFX(true);
    }

    //Buttons
    public void PauseButton()
    {
        OpenGameMenu();
        playButton.SetActive(true);
    }

    public void PlayButton()
    {
        CloseGameMenu();
    }

    public void ReplayButton()
    {
        if (canReplay || Settings.isGameFinished)
        {
            AudioManager.Instance.StopAllEmitters();
            LoadingScene.Instance.TogglePlayer(false);
            LoadingScene.Instance.StartCoroutine(LoadingScene.Instance.LoadSceneAsync(Settings.curSceneNum, Settings.curSceneNum));
        }
        else
        {
            ToggleRewardedAds(true);
        }

    }

    public void NextLevelButton()
    {
        LoadingScene.Instance.StartCoroutine(LoadingScene.Instance.LoadSceneAsync(Settings.curSceneNum+1, Settings.curSceneNum));
        LoadingScene.Instance.TogglePlayer(false);
        AudioManager.Instance.StopAllEmitters();
    }

    public void HomeButton()
    {
        AudioManager.Instance.StopAllEmitters();
        AudioManager.Instance.ToggleSFX(true);
        LoadingScene.Instance.StartCoroutine(LoadingScene.Instance.LoadMenuAsync(Settings.curSceneNum));
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void PlayRewardAdButton()
    {
        AdsManager.Instance.ShowRewardedAd();
    }

    public void CloseRewardAdButton()
    {
        ToggleRewardedAds(false);
    }


    //otherMethods
    void OpenGameMenu()
    {
        AudioManager.Instance.ToggleSFX(false);
        GameManager.Instance.isMenuOpen = true;
        gameMenu.SetActive(true);
    }

    void CloseGameMenu()
    {
        GameManager.Instance.isMenuOpen = false;
        gameMenu.SetActive(false);

        playButton.SetActive(false);
        rePlayButton.SetActive(false);
        nextLevelButton.SetActive(false);
        rewardedAdBar.SetActive(false);
        canReplay = false;
        AudioManager.Instance.ToggleSFX(true);
    }

    void ToggleRewardedAds(bool val)
    {
        rewardedAdBar.SetActive(val);
    }


    public void RewardPlayer()
    {
        Settings.complitedLevelsAmount = Settings.curComplitedLevelsAmount;
        Settings.lives = 3;
        canReplay = true;
        SaveManager.Instance.SaveDataToFile();
        ToggleRewardedAds(false);
    }

    public void LevelComplete()
    {
        SaveManager.Instance.SaveDataToFile();
        OpenGameMenu();
        nextLevelButton.SetActive(true);
    }

    public void GameOver()
    {

        GameManager.Instance.isMenuOpen = true;
        Settings.lives--;
        if (Settings.lives <= 0) 
        {
            canReplay = false;
            Settings.curComplitedLevelsAmount = Settings.complitedLevelsAmount;
            Settings.complitedLevelsAmount = 0;
        }
        else
        {
            canReplay = true;
        }
        SaveManager.Instance.SaveDataToFile();
    }

    public void ToggleUnputUI(bool val)
    {
        inGameMenu.SetActive(val);
        GameManager.Instance.inputUI.SetActive(val);
    }

    public void OpenGameOver()
    {
        OpenGameMenu();
        rePlayButton.SetActive(true);
    }
}
