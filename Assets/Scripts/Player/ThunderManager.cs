using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering.Universal;

public class ThunderManager : SingletonMonobehaviour<ThunderManager>
{
    [SerializeField] Light2D[] thunders;
    [SerializeField] Transform[] thundersTransform;

    Coroutine thunderCor;

    protected override void Awake()
    {
        base.Awake();


    }

    public void StartThunder()
    {
        thunderCor = StartCoroutine(ThunderCoroutine());
    }

    void OnDisable()
    {
        if (thunderCor != null)
        {
            StopCoroutine(thunderCor);
        }
        DisableThunders();
    }


    void DisableThunders()
    {
        foreach (Light2D thunder in thunders)
        {
            thunder.enabled = false;
        }
    }

    IEnumerator ThunderCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4f, 24f));

            int index = Random.Range(0, thunders.Length);
            thunders[index].pointLightOuterRadius = Random.Range(13f, 20f);

            thunders[index].enabled = true;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.3f));
            AudioManager.Instance.PlayOneShot(FMODManager.Instance.Thunder, thundersTransform[index].position);
            thunders[index].enabled = false;

            yield return null;
        }
    }
}
