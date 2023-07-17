using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameMenu : SingletonMonobehaviour<GameMenu>
{
    //delater
    Light2D globalLight;
    [SerializeField] GameObject gameMenu;
    [SerializeField] GameObject pauseBar;
    [SerializeField] GameObject gameOverBar;


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
        pauseBar.SetActive(true);
        OpenGameMenu();
    }

    public void PlayButton()
    {
        CloseGameMenu();
    }

    public void ReplayButton()
    {

    }

    public void HomeButton()
    {
        LoadingScene.Instance.StartCoroutine(LoadingScene.Instance.LoadSceneAsync(1, Settings.curSceneNum));
    }

    public void MusicButton()
    {

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

        pauseBar.SetActive(false);
        gameOverBar.SetActive(false);
    }

    public void GameOver()
    {
        OpenGameMenu();
        gameOverBar.SetActive(true);
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
