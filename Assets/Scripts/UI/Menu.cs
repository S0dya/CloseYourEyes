using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : SingletonMonobehaviour<Menu>
{
    [SerializeField] GameObject levelsBar;
    [SerializeField] GameObject settingsBar;
    [SerializeField] Level[] levels;


    protected override void Awake()
    {
        base.Awake();


    }

    void Start()
    {
        for (int i = 0; i < Settings.CemplitedLevelsAmount; i++)
        {
            levels[i].SetRune();
        }
        levels[Settings.CemplitedLevelsAmount].UnLock();

    }

    public void PlayButton()
    {
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
        if (Settings.CemplitedLevelsAmount >= index)
        {
            index += 2;
            LoadingScene.Instance.StartCoroutine(LoadingScene.Instance.LoadSceneAsync(index, 1));
        }
    }
}
