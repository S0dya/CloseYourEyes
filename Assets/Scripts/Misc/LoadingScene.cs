using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class LoadingScene : SingletonMonobehaviour<LoadingScene>
{
    [SerializeField] GameObject LoadingScreen;
    [SerializeField] Image LoadingBarFill;
    [SerializeField] TextMeshProUGUI Epigraph;

    [SerializeField] GameObject inputPlayer;
    GameObject playerObject;
    Player player;

    Coroutine epigraphFadeOut;

    protected override void Awake()
    {
        base.Awake();

        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();
        TogglePlayer(false);
        StartCoroutine(LoadGame());
        Epigraph.enabled = false;
    }

    public IEnumerator LoadGame()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progression = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingBarFill.fillAmount = progression;

            yield return null;
        }

        LoadingScreen.SetActive(false);
    }

    public IEnumerator LoadMenuAsync(int sceneToClose)
    {
        AudioManager.Instance.ToggleRandomSFX(false);
        AudioManager.Instance.ToggleAmbience(false);
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneToClose);
        AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progression = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingBarFill.fillAmount = progression;

            yield return null;
        }
        Settings.curSceneNum = 1;

        TogglePlayer(false);
        LoadingScreen.SetActive(false);
    }

    public IEnumerator LoadSceneAsync(int sceneId, int sceneToClose)
    {
        AudioManager.Instance.ToggleRandomSFX(false);
        AudioManager.Instance.StopAllEmitters();
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneToClose);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Additive);

        if (!Settings.rainLevels.Contains(sceneToClose - 2))
        {
            AudioManager.Instance.ToggleAmbience(false);
        }
        else if (sceneToClose > 1)
        {
            Debug.Log("as");
            AudioManager.Instance.ToggleMusicRain(false);
        }

        if (epigraphFadeOut != null)
        {
            StopCoroutine(epigraphFadeOut);
            Epigraph.color = new Color(Epigraph.color.r, Epigraph.color.g, Epigraph.color.b, 1);
        }
        Epigraph.text = Settings.epigraphs[sceneId - 2];
        Epigraph.enabled = true;
        LoadingScreen.SetActive(true);

        while (!unloadOperation.isDone)
        {
            yield return null;
        }

        while (!operation.isDone)
        {
            float progression = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBarFill.fillAmount = progression;
            yield return null;
        }
        
        if (!Settings.rainLevels.Contains(sceneId-2))
        {
            AudioManager.Instance.ToggleAmbience(true);
            player.isThunderLevel = false;
            AudioManager.Instance.ToggleRandomSFX(true);
        }
        else
        {
            Debug.Log("QWE");
            AudioManager.Instance.ToggleMusicRain(true);
            player.isThunderLevel = true;
        }
        ClearForNewScene();
        Settings.curSceneNum = sceneId;
    }

    IEnumerator FadeOutEpigraph()
    {
        Color curC = Epigraph.color;
        float newAlpha = curC.a;
        while (newAlpha > 0)
        {
            newAlpha = Mathf.Lerp(newAlpha, -0.3f, 0.009f);
            Epigraph.color = new Color(curC.r, curC.g, curC.b, newAlpha);

            yield return null;
        }
        
        Epigraph.enabled = false;
        Epigraph.color = new Color(curC.r, curC.g, curC.b, 1);
    }

    void ClearForNewScene()
    {
        GameManager.Instance.isBlindFollowingPlayer = false;
        GameManager.Instance.isDefFollowingPlayer = false;
        GameManager.Instance.isMenuOpen = false;
        TogglePlayer(true);

        epigraphFadeOut = StartCoroutine(FadeOutEpigraph());
        LoadingScreen.SetActive(false);
    }

    public void TogglePlayer(bool val)
    {
        GameManager.Instance.isInGame = val;
        playerObject.SetActive(val);
        inputPlayer.SetActive(val);
    }
}
