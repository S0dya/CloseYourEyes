using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    [HideInInspector] public bool isMenuOpen;
    

    public void LevelComplete()
    {
        Debug.Log("LVLComplete");
    }
}
