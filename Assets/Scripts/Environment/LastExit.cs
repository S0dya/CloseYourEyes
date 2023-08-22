using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering.Universal;

public class LastExit : MonoBehaviour
{
    Light2D light;

    Coroutine exitingCor;
    Coroutine stopExitingCor;

    void Awake()
    {
        light = GetComponent<Light2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (stopExitingCor != null)
            {
                StopCoroutine(stopExitingCor);
            }
            AudioManager.Instance.PlayExit((int)light.intensity);
            exitingCor = StartCoroutine(Exiting());
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (exitingCor != null)
            {
                StopCoroutine(exitingCor);
            }
            AudioManager.Instance.StopExiting();
            stopExitingCor = StartCoroutine(StopExiting());
        }
    }

    IEnumerator Exiting()
    {
        while (light.intensity < 1)
        {
            light.intensity += Time.deltaTime;

            yield return null;
        }

        Settings.isGameFinished = true;
        GameMenu.Instance.LevelComplete();
        GameManager.Instance.LevelComplete();
    }

    IEnumerator StopExiting()
    {
        while (light.intensity > 0)
        {
            light.intensity -= Time.deltaTime;

            yield return null;
        }
    }
}
