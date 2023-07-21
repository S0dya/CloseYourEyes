using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : SingletonMonobehaviour<Menu>
{
    [SerializeField] GameObject levelsBar;
    [SerializeField] GameObject settingsBar;
    [SerializeField] Level[] levels;

    bool isLevelBarSet;

    protected override void Awake()
    {
        base.Awake();


    }

    public void PlayButton()
    {

        levelsBar.SetActive(!levelsBar.activeSelf);
        if (settingsBar.activeSelf)
        {
            settingsBar.SetActive(!settingsBar.activeSelf);
        }
        if (levelsBar.activeSelf && !isLevelBarSet)
        {
            StartCoroutine(OpenLevels());
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
        if (Settings.complitedLevelsAmount >= index)
        {
            index += 2;
            LoadingScene.Instance.StartCoroutine(LoadingScene.Instance.LoadSceneAsync(index, 1));
        }
    }

    IEnumerator OpenLevels()
    {
        for (int i = levels.Length - 1; i >= 0; i--)
        {
            if (i > Settings.complitedLevelsAmount)
            {
                levels[i].Lock();
            }
            else if (i < Settings.complitedLevelsAmount)
            {
                levels[i].SetRune();
            }
            else
            {
                levels[i].UnLock();
            }

            yield return new WaitForSeconds(0.025f);
        }
        isLevelBarSet = true;
    }

}
