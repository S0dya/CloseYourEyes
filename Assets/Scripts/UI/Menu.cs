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
    [SerializeField] Heart[] hearts;

    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider ambienceSlider;
    [SerializeField] Slider musicSlider;

    [SerializeField] Image flexibleJoystick;
    [SerializeField] Image fixedJoystick;

    [SerializeField] GameObject MobileUISettings;

    bool isLevelBarSet;

    

    protected override void Awake()
    {
        base.Awake();

#if UNITY_ANDROID || UNITY_IOS
        MobileUISettings.SetActive(true);
#else
        MobileUISettings.SetActive(false);
#endif
    }

    void Start()
    {
        Time.timeScale = 1;
        isLevelBarSet = false;
        AudioManager.Instance.ToggleMusic(true);
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
        if (settingsBar.activeSelf)
        {
            ToggleSettings(false);
        }

        bool active = !levelsBar.activeSelf;
        ToggleLevels(active);
        GameManager.Instance.inMenu = !active;
    }

    public void SettingsButton()
    {
        if (levelsBar.activeSelf)
        {
            ToggleLevels(false);
        }

        bool active = !settingsBar.activeSelf;
        ToggleSettings(active);
        GameManager.Instance.inMenu = !active;
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void LevelButton(int index)
    {
        if (Settings.complitedLevelsAmount >= index)
        {
            GameManager.Instance.inLevels = false;
            AudioManager.Instance.ToggleMusic(false);
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
    void ToggleLevels(bool val)
    {
        GameManager.Instance.inLevels = val;
        levelsBar.SetActive(val);

        if (val && !isLevelBarSet)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i].ToggleHeart(Settings.lives <= i);
            }
            StartCoroutine(OpenLevels());
        }
    }
    void ToggleSettings(bool val)
    {
        GameManager.Instance.inSettings = val;
        settingsBar.SetActive(val);
    }

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

    public void PlayButtonSound()
    {
        AudioManager.Instance.PlayOneShot("ButtonPress");
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
