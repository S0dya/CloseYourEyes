using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : SingletonMonobehaviour<LoadingScene>
{
    [SerializeField] GameObject LoadingScreen;
    [SerializeField] Image LoadingBarFill;

    [SerializeField] GameObject inputPlayer;
    GameObject playerObject;

    protected override void Awake()
    {
        base.Awake();

        playerObject = GameObject.FindGameObjectWithTag("Player");
        TogglePlayer();
        StartCoroutine(LoadGame());
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

    public IEnumerator LoadSceneAsync(int sceneId, int sceneToClose)
    {
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneToClose);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Additive);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progression = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingBarFill.fillAmount = progression;

            yield return null;
        }

        TogglePlayer();
        LoadingScreen.SetActive(false);
    }


    public void TogglePlayer()
    {
        playerObject.SetActive(!playerObject.activeSelf);
        inputPlayer.SetActive(!inputPlayer.activeSelf);
    }
}
