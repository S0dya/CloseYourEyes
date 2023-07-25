using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : SingletonMonobehaviour<Menu>
{
    [SerializeField] GameObject levelsBar;
    [SerializeField] GameObject settingsBar;
    [SerializeField] GameObject newGameBar;
    [SerializeField] Level[] levels;

    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider ambienceSlider;
    [SerializeField] Slider musicSlider;

    [SerializeField] Image flexibleJoystick;
    [SerializeField] Image fixedJoystick;

    bool isLevelBarSet;

    protected override void Awake()
    {
        base.Awake();

    }

    void Start()
    {
        sfxSlider.value = Settings.sfxVolume;
        ambienceSlider.value = Settings.ambienceVolume;
        musicSlider.value = Settings.musicVolume;
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);
        AudioManager.Instance.SetAmbienceVolume(ambienceSlider.value);
        AudioManager.Instance.SetMusicVolume(musicSlider.value);

        ToggleFlexibilityOfJoystick(Settings.isJoystickFlexible);
    }

    //Buttons
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

    public void CloseNewGameBar()
    {
        newGameBar.SetActive(false);
    }

    public void NewGameButtonInSettings()
    {
        newGameBar.SetActive(true);
    }

    public void NewGameButton()
    {
        Settings.complitedLevelsAmount = 0;
        Settings.curComplitedLevelsAmount = 0;
        LoadingScene.Instance.StartCoroutine(LoadingScene.Instance.LoadMenuAsync(1));
    }

    public void JoystickButton(bool val)
    {
        ToggleFlexibilityOfJoystick(val);
    }

    public void SFXVolumeChange()
    {
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);
    }

    public void AmbienceVolumeChange()
    {
        AudioManager.Instance.SetAmbienceVolume(ambienceSlider.value);
    }

    public void MusicVolumeChange()
    {
        AudioManager.Instance.SetMusicVolume(musicSlider.value);
    }

    //OtherMethods
    public void ToggleFlexibilityOfJoystick(bool val)
    {
        Settings.isJoystickFlexible = val;
        if (val)
        {
            flexibleJoystick.color = new(255, 255, 255, 1f);
            fixedJoystick.color = new(255, 255, 255, 0.3f);
            FlexibleJoystick.Instance.Toggle(true);
        }
        else
        {
            fixedJoystick.color = new(255, 255, 255, 1);
            flexibleJoystick.color = new(255, 255, 255, 0.3f);
            FlexibleJoystick.Instance.Toggle(false);
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
