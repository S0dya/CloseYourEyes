using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : SingletonMonobehaviour<Menu>
{
    [SerializeField] GameObject levelsBar;
    [SerializeField] GameObject settingsBar;
    [SerializeField] GameObject[] levels;


    protected override void Awake()
    {
        base.Awake();


    }

    void Start()
    {
        
    }

    public void PlayButton()
    {
        LevelButton(2);
        levelsBar.SetActive(!levelsBar.activeSelf);
        if (settingsBar.activeSelf)
        {
            settingsBar.SetActive(!settingsBar.activeSelf);
        }
    }

    public void SettingsButton()
    {
        settingsBar.SetActive(!settingsBar.activeSelf);
        if (levelsBar.activeSelf)
        {
            levelsBar.SetActive(!levelsBar.activeSelf);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void LevelButton(int index)
    {
        LoadingScene.Instance.StartCoroutine(LoadingScene.Instance.LoadSceneAsync(index, 1));
        Settings.curSceneNum = index;
    }
}
