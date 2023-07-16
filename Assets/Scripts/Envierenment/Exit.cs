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
            stopExitingCor = StartCoroutine(StopExiting());
        }
    }

    IEnumerator Exiting()
    {
        while (light.intensity < 1)
        {
            if (GameManager.Instance.isMenuOpen)
            {
                yield return null;
            }

            light.intensity += Time.deltaTime;

            yield return null;
        }

        GameManager.Instance.LevelComplete();
    }

    IEnumerator StopExiting()
    {
        while (light.intensity > 0)
        {
            if (GameManager.Instance.isMenuOpen)
            {
                yield return null;
            }

            light.intensity -= Time.deltaTime;

            yield return null;
        }
    }
}
