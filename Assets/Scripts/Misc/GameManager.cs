using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    [HideInInspector] public bool isMenuOpen;

    public int[] visionTime;

    protected override void Awake()
    {
        base.Awake();

        Settings.complitedLevelsAmount = 14;
    }

    public void LevelComplete()
    {
        Settings.complitedLevelsAmount++;
        Debug.Log("LVLComplete");
    }

    public IEnumerator Timer(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (isMenuOpen)
            {
                yield return null;
            }
            else
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
