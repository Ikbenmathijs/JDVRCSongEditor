using UnityEngine;


public static class Config
{
    
    public static bool initialized = false;
    public static string binariesFolder = $"{Application.streamingAssetsPath}/Bin";
    public static string ytdlpName = "yt-dlp.exe";
    public static string ffmpegName = "ffmpeg.exe";
    public static string videoStoragePath = $"{Application.streamingAssetsPath}/Temp";
    public static string tempFolder = $"{Application.streamingAssetsPath}/Temp";
    
}
