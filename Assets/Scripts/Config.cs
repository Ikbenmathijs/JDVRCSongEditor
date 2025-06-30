using UnityEngine;


public static class Config
{
    
    public static bool initialized = false;
    public static string binariesFolder = $"{Application.dataPath}/JDVRCSongEditorData/Bin";
    public static string ytdlpName = "yt-dlp.exe";
    public static string ffmpegName = "ffmpeg.exe";
    public static string videoStoragePath = $"{Application.dataPath}/JDVRCSongEditorData/Temp/Video";
    public static string tempFolder = $"{Application.dataPath}/JDVRCSongEditorData/Temp";
    public static string zippableFolder = $"{Application.dataPath}/JDVRCSongEditorData/Zippable";
    
}
