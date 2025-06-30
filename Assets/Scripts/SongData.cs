using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SongData
{
    public static string videoUrl;
    public static string name;
    public static string artist;
    public static string duration;
    public static int dancerAmount;
    public static int difficulty;
    public static string[] recordings = new string[4];
    public static bool[] recordingsAreImported = new bool[4];
    public static string game;
    public static string imagePath;
    public static bool[] dancerImageSet = new bool[4];
    public static string[] dancerImagePaths = new string[4];
    public static string videoPath;
    public static float startTime;
    public static float endTime;
    public static float audioPreviewStartTime;
}
