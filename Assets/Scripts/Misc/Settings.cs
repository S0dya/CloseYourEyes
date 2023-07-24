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

    public static string GameScene = "Menu";
    /// <summary>
    /// inGane
    /// </summary>
    public static int curSceneNum;
    public static int curComplitedLevelsAmount;

    public static float[] visionTime = { 4, 1 };
    public static string[] epigraphs = { "...i dont see...", "...i dont hear..." };
}
