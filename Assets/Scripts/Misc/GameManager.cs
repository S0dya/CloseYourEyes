using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    [HideInInspector] public bool isMenuOpen;

    protected override void Awake()
    {
        base.Awake();

    }

    public void LevelComplete()
    {
        Debug.Log("LVLComplete");
    }
}
