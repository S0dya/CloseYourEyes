using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    /// <summary>
    /// save
    /// </summary>
    public static int complitedLevelsAmount;
    public static int lives = 3;
    public static bool isGameFinished;
    public static bool isJoystickFlexible;

    //sound
    public static float sfxVolume = 1;
    public static float ambienceVolume = 1;
    public static float musicVolume = 1;
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

    public static float[] visionRadious = { 6.5f, 3f, 6.3f, 6.1f, 3f, 5.9f, 5.7f, 3f, 5.5f, 5.3f, 5.1f, 3f, 4.9f, 4.7f, 4.5f, 3f, 4.3f, 4.1f, 3.9f };
    public static float[] visionTime = { 6, 5, 4, 3, 2, 1, 6, 5, 4, 3, 2, 1, 5, 4, 3, 2, 1, 0.5f};
    public static string[] epigraphs = 
        { "closed eyes", "despair linger", "through darkness", "shadows persist", "light elusive", "a futile struggle",
        "closed ears", "silence reigns", "unveil sorrow"," shattered hope", "silence drowns hope","...i dont hear...",
        "void","steps echo futility","labyrinth of despair","racing heart","haunting realization","chasing light "
        };
    public static int[] rainLevels = { 1, 4, 7, 8, 11, 15};



    public static void Initialize()
    {
        screen = new Vector2(Screen.width, Screen.height);
        screenHalf = screen/2;
        placeForJoystickMovement = new Vector2(Screen.width /1.5f, screenHalf.y);

    }
}
