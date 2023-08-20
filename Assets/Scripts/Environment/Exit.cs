using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering.Universal;

public class Exit : MonoBehaviour
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
            light.intensity += 0.5f * Time.deltaTime;

            yield return null;
        }

        GameMenu.Instance.LevelComplete();
        GameManager.Instance.LevelComplete();
        AudioManager.Instance.StopExiting();
    }

    IEnumerator StopExiting()
    {
        while (light.intensity > 0)
        {
            light.intensity -= 0.7f * Time.deltaTime;

            yield return null;
        }
    }
}
