using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class GameMenu : SingletonMonobehaviour<GameMenu>
{
    //delater
    Light2D globalLight;
    [SerializeField] GameObject gameMenu;
    [SerializeField] GameObject inGameMenu;
    [SerializeField] GameObject rewardedAdBar;
    [SerializeField] TextMeshProUGUI mainButtonText;

    bool canReplay;

    bool inMenu;
    bool inGameOver;

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

        inMenu = false;
        inGameOver = false;
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
        canReplay = true;
        //SaveManager.Instance.SaveDataToFile();
        ToggleRewardedAds(false);
    }

    public void LevelComplete()
    {
        mainButtonText.text = "NEXT LEVEL";
        //SaveManager.Instance.SaveDataToFile();
        OpenGameMenu();
    }

    public void GameOver()
    {
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
        //SaveManager.Instance.SaveDataToFile();
    }

    public void ToggleUnputUI(bool val)
    {
        inGameMenu.SetActive(val);
        GameManager.Instance.inputUI.SetActive(val);
    }

    public void OpenGameOver()
    {
        OpenGameMenu();
    }
}
