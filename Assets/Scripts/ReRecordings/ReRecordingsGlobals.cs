using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReRecordingsGlobals
{
    public static SongEntry rerecodingSong;
}


public struct SongEntry
{
    public string name;
    public string artist;
    public string duration;
    public string url;
    public string original_url;
    public string image;
    public string game;
    public string amountOfDancers;
    public int difficulty;
    public int sinceVersion;
    public string searchName;
}