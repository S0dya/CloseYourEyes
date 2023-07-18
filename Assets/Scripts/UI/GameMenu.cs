using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameMenu : SingletonMonobehaviour<GameMenu>
{
    //delater
    Light2D globalLight;
    [SerializeField] GameObject gameMenu;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject rePlayButton;
    [SerializeField] GameObject nextLevelButton;


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
        CloseGameMenu();
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
        LoadingScene.Instance.StartCoroutine(LoadingScene.Instance.LoadSceneAsync(Settings.curSceneNum, Settings.curSceneNum));
        LoadingScene.Instance.TogglePlayer();
    }

    public void NextLevelButton()
    {
        LoadingScene.Instance.StartCoroutine(LoadingScene.Instance.LoadSceneAsync(Settings.curSceneNum+1, Settings.curSceneNum));
        LoadingScene.Instance.TogglePlayer();
    }

    public void HomeButton()
    {
        LoadingScene.Instance.StartCoroutine(LoadingScene.Instance.LoadSceneAsync(1, Settings.curSceneNum));
    }

    public void ExitButton()
    {
        Application.Quit();
    }


    //otherMethods
    void OpenGameMenu()
    {
        GameManager.Instance.isMenuOpen = true;
        gameMenu.SetActive(true);
        EnableMoving(false);
    }

    void CloseGameMenu()
    {
        GameManager.Instance.isMenuOpen = false;
        gameMenu.SetActive(false);
        EnableMoving(true);

        playButton.SetActive(false);
        rePlayButton.SetActive(false);
        nextLevelButton.SetActive(false);
    }

    public void LevelComplete()
    {
        OpenGameMenu();
        nextLevelButton.SetActive(true);
    }

    public void GameOver()
    {
        OpenGameMenu();
        rePlayButton.SetActive(true);
    }



    void EnableMoving(bool val)
    {
        foreach (var def in defEnemyArr)
        {
            def.SetActive(val);
        }
     
        foreach (var blind in blindEnemArr)
        {
            blind.SetActive(val);
        }
    }
}
