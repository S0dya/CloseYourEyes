using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameMenu : SingletonMonobehaviour<GameMenu>
{
    [SerializeField] GameObject gameMenu;
    [SerializeField] GameObject inGameMenu;
    [SerializeField] GameObject rewardedAdBar;
    [SerializeField] TextMeshProUGUI mainButtonText;

    bool canReplay;

    [HideInInspector] public bool inGame;
    [HideInInspector] public bool inMenu;
    [HideInInspector] public bool inBeforeGameOver;
    [HideInInspector] public bool inGameOver;
    [HideInInspector] public bool inRewardedAd;

    GameObject[] defEnemyArr;
    GameObject[] blindEnemArr;

    protected override void Awake()
    {
        base.Awake();

        defEnemyArr = GameObject.FindGameObjectsWithTag("DefEnemy");
        blindEnemArr = GameObject.FindGameObjectsWithTag("BlindEnemy");

    }

    void Start()
    {
        Time.timeScale = 1;
        inGame = true;
    }

    //Buttons
    public void PauseButton()
    {
        inMenu = true;
        mainButtonText.text = "PLAY";
        OpenGameMenu();
    }

    public void MainButton()
    {
        if (inMenu)
        {
            CloseGameMenu();
        }
        else if (inGameOver)
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
        else
        {
            LoadingScene.Instance.StartCoroutine(LoadingScene.Instance.LoadSceneAsync(Settings.curSceneNum+1, Settings.curSceneNum));
            LoadingScene.Instance.TogglePlayer(false);
            AudioManager.Instance.StopAllEmitters();
        }
    }

    public void HomeButton()
    {
        AudioManager.Instance.StopAllEmitters();
        AudioManager.Instance.ToggleSFX(true);
        LoadingScene.Instance.StartCoroutine(LoadingScene.Instance.LoadMenuAsync(Settings.curSceneNum));
    }

    public void ExitButton()
    {
        SaveManager.Instance.SaveDataToFile();
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

    public void PlayButtonSound()
    {
        AudioManager.Instance.PlayOneShot("ButtonPress");
    }


    //otherMethods
    public void OpenGameMenu()
    {
        Time.timeScale = 0;
        inGame = false;
        AudioManager.Instance.ToggleSFX(false);
        GameManager.Instance.isMenuOpen = true;
        gameMenu.SetActive(true);
    }

    public void CloseGameMenu()
    {
        inGame = true;
        GameManager.Instance.isMenuOpen = false;
        gameMenu.SetActive(false);

        inMenu = false;
        inGameOver = false;
        rewardedAdBar.SetActive(false);
        canReplay = false;
        AudioManager.Instance.ToggleSFX(true);
        Time.timeScale = 1;
    }

    void ToggleRewardedAds(bool val)
    {
        inGameOver = !val;
        inRewardedAd = val;
        rewardedAdBar.SetActive(val);
    }

    public void RewardPlayer()
    {
        Settings.complitedLevelsAmount = Settings.curComplitedLevelsAmount;
        canReplay = true;
        ToggleRewardedAds(false);
    }

    public void LevelComplete()
    {
        mainButtonText.text = "NEXT LEVEL";
        OpenGameMenu();
    }

    public void GameOver()
    {
        inGame = false;
        inGameOver = true;
        mainButtonText.text = "REPLAY";
        GameManager.Instance.isMenuOpen = true;
        Settings.lives--;
        if (Settings.lives <= 0) 
        {
            canReplay = false;
            Settings.curComplitedLevelsAmount = Settings.complitedLevelsAmount;
            Settings.complitedLevelsAmount = 0;
            Settings.lives = 3;
        }
        else
        {
            canReplay = true;
        }
    }

    public void ToggleUnputUI(bool val)
    {
        inGameMenu.SetActive(val);
        GameManager.Instance.inputUI.SetActive(val);
    }
}
