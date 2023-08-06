using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    /// <summary>
    /// save
    /// </summary>
    public static int complitedLevelsAmount;
    public static int lives;
    public static bool isGameFinished;
    public static bool isJoystickFlexible;

    //sound
    public static float sfxVolume;
    public static float ambienceVolume;
    public static float musicVolume;
    public static float curSfxVolume;

    public static string GameScene = "Menu";

    //game
    public static Vector2 screen { get; private set; }
    public static Vector2 screenHalf { get; private set; }
    public static Vector2 placeForJoystickMovement { get; private set; }


    /// <summary>
    /// inGane
    /// </summary>
    public static int curSceneNum;
    public static int curLvlNum;
    public static int curComplitedLevelsAmount;

    public static float curVisionRadious;

    public static float[] visionRadious = { 6.5f, 3f, 6.3f, 6.2f, 6.1f, 6.5f, 3f, 6.3f, 6.2f, 6.1f, 6.5f, 3f, 6.3f, 6.2f, 6.1f };
    public static float[] visionTime = { 4, 1, 4, 1, 4, 1, 4, 1, 4, 1, 4, 1, 4, 1, 4, 1, 4, 1, 4, 1, };
    public static string[] epigraphs = 
        { 
        ".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",
        "...i dont see...","...i dont hear...","...i dont hear...","...i dont hear...","...i dont hear...","...i dont hear...",
        "...i dont hear...","...i dont hear...","...i dont hear...","...i dont hear...","...i dont hear...","...i dont hear...",
        "...i dont hear...","...i dont hear...","...i dont hear...","...i dont hear...","...i dont hear...","...i dont hear..." 
        };
    public static int[] rainLevels = { 1, 3, 4, 5 };



    public static void Initialize()
    {
        screen = new Vector2(Screen.width, Screen.height);
        screenHalf = screen/2;
        placeForJoystickMovement = new Vector2(Screen.width /1.5f, screenHalf.y);
    }
}
