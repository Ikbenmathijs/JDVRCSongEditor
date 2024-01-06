using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SongData
{
    public static int dancerAmount;
    public static int difficulty;
    public static string[] recordings = new string[4];
    public static bool[] recordingsAreImported = new bool[4];
    public static string game;
    public static Texture2D image;
    public static bool[] dancerImageSet = new bool[4];
    public static Texture2D[] dancerImages = new Texture2D[4];
    public static string videoPath;
    public static float startTime;
    public static float endTime;
}
