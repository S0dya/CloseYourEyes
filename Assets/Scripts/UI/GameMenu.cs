using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : SingletonMonobehaviour<GameMenu>
{
    [SerializeField] GameObject gameMenu;

    [SerializeField] GameObject pauseBar;
    [SerializeField] GameObject gameOverBar;

    [SerializeField] List<DefEnemy> defEnemyList;
    [SerializeField] List<BlindEnemy> blindEnemyList;

    protected override void Awake()
    {
        base.Awake();

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
        foreach (var def in defEnemyList)
        {
            def.ai.canMove = val;
        }
     
        foreach (var blind in blindEnemyList)
        {
            if (blind.isWatched && val)
            {
                continue;
            }
            blind.ai.canMove = val;
        }
    }
}
