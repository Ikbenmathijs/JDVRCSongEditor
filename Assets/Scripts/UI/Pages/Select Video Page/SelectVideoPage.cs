using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using TMPro;
using UnityEngine;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using YoutubeDLSharp;
using YoutubeDLSharp.Options;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SFB;

public class SelectVideoPage : Page
{
    public bool skipVideoProcessing = false;
    public TMP_InputField youtubeUrlInputField;

    public Popup popup;

    private YoutubeDL ytdlp;


    public Transform progressBar;
    
    private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    
    public Button downloadFromYoutubeUrlButton;

    public GameObject selectVideoScreen;
    public GameObject loadingScreen;

    public TextMeshProUGUI currentTaskText;
    public TextMeshProUGUI consolePreviewText;

    private int videoDuration = -1;
    private float loadingBarProgress = 0f;
    private string currentConsolePreviewString = "";
    
    private List<Process> runningProcesses = new List<Process>();
    public GameObject alreadySelected;
    
    private bool videoFromYoutube = false;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        PageName = "Select Video";
        selectVideoScreen.SetActive(true);
        loadingScreen.SetActive(false);
        
        
        
    }
    
    // this function is just here to call the async function
    public void DownloadFromYoutubeUrlButtonPressed()
    {
        DownloadFromYoutubeUrlButton();
    }

    private async void DownloadFromYoutubeUrlButton()
    {
        videoFromYoutube = true;
        string url = youtubeUrlInputField.text;

        if (!ValidateURL(url))
        {
            popup.ShowPopup("Please enter a valid URL!");
            return;
        }
        
        ytdlp = new YoutubeDL
        {
            YoutubeDLPath = Config.binariesFolder + "/" + Config.ytdlpName,
            FFmpegPath = Config.binariesFolder + "/" + Config.ffmpegName,
            OutputFolder = Config.videoStoragePath
        };


        Progress<DownloadProgress> progressCallback = new Progress<DownloadProgress>(p =>
        {
            loadingBarProgress = p.Progress;
        });
        selectVideoScreen.SetActive(false);
        loadingScreen.SetActive(true);
        
        downloadFromYoutubeUrlButton.interactable = false;

        string downloadedVideoPath = await DownloadYoutubeVideo(url, progressCallback);
        string processedVideoPath = await ProcessVideo(downloadedVideoPath);
        
        
        SongData.videoPath = processedVideoPath;

        FinishVideoProcessing();
        downloadFromYoutubeUrlButton.interactable = true;
    }

    private void FinishVideoProcessing()
    {
        loadingScreen.SetActive(false);
        selectVideoScreen.SetActive(true);
        SetNextPageAvailable(true);
        NextPage();
        alreadySelected.SetActive(true);
        
    }
    
    

    public void SelectVideoFromFileButtonPressed()
    {
        string[] files = StandaloneFileBrowser.OpenFilePanel("Select Recording File", "", "mp4", false);
        
        if (files.Length != 1)
        {
            popup.ShowPopup("Please select 1 video file");
            return;
        }
        
        string initialPath = files[0];
        
        SelectVideoFromFileAsync(initialPath);
    }

    private async void SelectVideoFromFileAsync(string initialPath)
    {
        videoFromYoutube = false;
        loadingScreen.SetActive(true);
        selectVideoScreen.SetActive(false);
        string processedVideoPath = await ProcessVideo(initialPath);
        
        SongData.videoPath = processedVideoPath;
        
        FinishVideoProcessing();
    }


    private async Task<string> ProcessVideo(string initialPath)
    {
#if UNITY_EDITOR
        if (skipVideoProcessing) return initialPath;
#endif
        
        string adjustedAudioVideoPath = await AdjustAudio(initialPath);
        /*if (new FileInfo(initialPath).Length > 95 * 1024 * 1024 || !initialPath.EndsWith(".mp4"))
        { 
            processedVideoPath = await CompressVideo(adjustedAudioVideoPath);
        }*/

        if (videoFromYoutube)
        {
            await Task.Run(() => { File.Delete(initialPath) ;});
        }

        return adjustedAudioVideoPath;
    }

    


    [ItemCanBeNull]
    private async Task<string> DownloadYoutubeVideo(string url, Progress<DownloadProgress> progressCallback = null)
    {
        currentTaskText.text = "Downloading video...";
        
        OptionSet options = new OptionSet
        {
            Output = "DownloadedVideo.%(ext)s",
            ForceOverwrites = true,
            Format = "bestvideo[ext=mp4][height<=1080]+bestaudio[ext=m4a]/best[ext=mp4]/best\"",
            RecodeVideo = VideoRecodeFormat.Mp4,
            MergeOutputFormat = DownloadMergeFormat.Mp4,
            Paths = Config.videoStoragePath
        };
        
        RunResult<string> res = await ytdlp.RunVideoDownload(
        url, 
        progress: progressCallback, 
        ct: cancellationTokenSource.Token, 
        recodeFormat: VideoRecodeFormat.Mp4, 
        mergeFormat: DownloadMergeFormat.Mp4,
        overrideOptions: options
         );
        
        foreach (string error in res.ErrorOutput)
        {
            Debug.LogError(error + "\n");
            currentConsolePreviewString += error + "\n";
        }
        
        if (!res.Success)
        {
            popup.ShowPopup($"Failed to download video, see log for more details");
            Debug.LogError($"Failed to download video: {res.ErrorOutput[0]}");
            return null;
        }
        
        
        
        string path = res.Data;
        return path;
    }
    
    
    private async Task<string> AdjustAudio(string inputPath)
    {
        Debug.Log("Adjusting audio... with path " + inputPath);
        videoDuration = -1;
        float? volumeOffset = await GetRequiredAudioOffset(inputPath);
        
        
        if (!Directory.Exists(Config.videoStoragePath))
        {
            Directory.CreateDirectory(Config.videoStoragePath);
        }
        
        
        currentTaskText.text = "Adjusting audio volume...";

        
        if (volumeOffset == null)
        {
            return inputPath;
        }
        
        string outputFile = $"{Config.videoStoragePath}/Video.mp4";
        
        Process ffmpegProcess = new Process
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = $"{Config.binariesFolder}/{Config.ffmpegName}",
                Arguments = $"-i \"{inputPath}\" -y -af \"volume={volumeOffset}dB\" -movflags +faststart \"{outputFile}\"",
                UseShellExecute = false,
                RedirectStandardOutput = false,                    
                CreateNoWindow = true,
                RedirectStandardError = true
            },
            EnableRaisingEvents = true
        };

        currentConsolePreviewString += $"ffmpeg {ffmpegProcess.StartInfo.Arguments}\n";
        Debug.Log(ffmpegProcess.StartInfo.Arguments);
        
        
        // for some reason ffmpeg only logs in standard error, instead of standard output
        ffmpegProcess.ErrorDataReceived += (sender, e) =>
        {
            Debug.Log(e.Data);
            Interlocked.Exchange(ref currentConsolePreviewString, currentConsolePreviewString + e.Data + "\n");
            if (videoDuration < 0 && e.Data != null && e.Data.Contains("Duration:"))
            {
                Interlocked.Exchange(ref videoDuration, GetDurationFromLogMessage(e.Data));
                Debug.Log("Obtained video duration: " + videoDuration);
            } else if (videoDuration > 0 && e.Data != null && e.Data.Contains("time="))
            {
                float progress = GetProgressFromLogMessage(e.Data, videoDuration);
                Interlocked.Exchange(ref loadingBarProgress, progress);
                Debug.Log("Progress: " + progress);
            }
        };
        
        ffmpegProcess.Start();
        runningProcesses.Add(ffmpegProcess);
        ffmpegProcess.BeginErrorReadLine();
        
        await Task.Run(() => ffmpegProcess.WaitForExit());
        
        
        if (!File.Exists(outputFile))
        {
            popup.ShowPopup($"Failed to adjust volume, see log for more details. The original video without adjusted volume will be used instead");
            return inputPath;
        }

        return outputFile;
    }

    
    private float maxVolume = float.NegativeInfinity;
    private async Task<float?> GetRequiredAudioOffset(string videoPath)
    {
        currentTaskText.text = "Getting audio volume offset...";
        maxVolume = float.NegativeInfinity;
        videoDuration = -1;


        Process ffmpegProcess = new Process
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = $"{Config.binariesFolder}/{Config.ffmpegName}",
                Arguments = $"-i \"{videoPath}\" -filter:a volumedetect -y -f null -",
                UseShellExecute = false,
                RedirectStandardOutput = false,                    
                CreateNoWindow = true,
                RedirectStandardError = true
            },
            EnableRaisingEvents = true
        };
        
        currentConsolePreviewString += $"ffmpeg {ffmpegProcess.StartInfo.Arguments}\n";
        Debug.Log(ffmpegProcess.StartInfo.Arguments);
        
        // for some reason ffmpeg only logs in standard error, instead of standard output
        ffmpegProcess.ErrorDataReceived += (sender, e) =>
        {
            Debug.Log(e.Data);
            Interlocked.Exchange(ref currentConsolePreviewString, currentConsolePreviewString + e.Data + "\n");
            if (e.Data != null && e.Data.Contains("max_volume:"))
            {
                string split = e.Data.Split(':')[1];
                string maxVolumeString = split.Substring(1,split.Length - 3);
                float parsed;
                if (float.TryParse(maxVolumeString, out parsed))
                {
                    Interlocked.Exchange(ref maxVolume, parsed);
                    Debug.Log("Obtained max volume: " + maxVolume);
                }
                else
                {
                    Debug.LogError($"Failed to parse max volume: {maxVolumeString}");
                }
            }
            
            if (videoDuration < 0 && e.Data != null && e.Data.Contains("Duration:"))
            {
                Interlocked.Exchange(ref videoDuration, GetDurationFromLogMessage(e.Data));
                Debug.Log("Obtained video duration: " + videoDuration);
            } else if (videoDuration > 0 && e.Data != null && e.Data.Contains("time="))
            {
                float progress = GetProgressFromLogMessage(e.Data, videoDuration);
                Interlocked.Exchange(ref loadingBarProgress, progress);
                Debug.Log("Progress: " + progress);
            }
        };

        ffmpegProcess.Start();
        runningProcesses.Add(ffmpegProcess);
        
        ffmpegProcess.BeginErrorReadLine();

        await Task.Run(() => { ffmpegProcess.WaitForExit(); });
        
        Debug.Log("Process exited");
        Debug.Log("Max volume is: " + maxVolume);
        
        
        if (float.IsNegativeInfinity(maxVolume))
        {
            popup.ShowPopup("Failed to get max volume, check log for more details. The original video without adjusted volume will be used instead");
            return null;
        }
        
        
        float requiredVolumeAdjustment = -3.0f - maxVolume;

        return requiredVolumeAdjustment;
    }
    
    /*private async Task<string> CompressVideo(string videoPath)
    {
        currentTaskText.text = "Compressing video...";
        videoDuration = -1;
        loadingBarProgress = 0f;
        
        string outputFile = $"{Config.videoStoragePath}/CompressedVideo.mp4";


        Process ffmpegProcess = new Process
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = $"{Config.binariesFolder}/{Config.ffmpegName}",
                Arguments = $"-i \"{videoPath}\" -y -vcodec libx264 -crf 15 \"{outputFile}\"",
                UseShellExecute = false,
                RedirectStandardOutput = false,                    
                CreateNoWindow = true,
                RedirectStandardError = true
            },
            EnableRaisingEvents = true
        };
        
        currentConsolePreviewString += $"ffmpeg {ffmpegProcess.StartInfo.Arguments}\n";
        Debug.Log(ffmpegProcess.StartInfo.Arguments);
        
        
        // for some reason ffmpeg only logs in standard error, instead of standard output
        ffmpegProcess.ErrorDataReceived += (sender, e) =>
        {
            Debug.Log(e.Data);
            Interlocked.Exchange(ref currentConsolePreviewString, currentConsolePreviewString + e.Data + "\n");
            if (videoDuration < 0 && e.Data != null && e.Data.Contains("Duration:"))
            {
                Interlocked.Exchange(ref videoDuration, GetDurationFromLogMessage(e.Data));
                Debug.Log("Obtained video duration: " + videoDuration);
            } else if (videoDuration > 0 && e.Data != null && e.Data.Contains("time="))
            {
                float progress = GetProgressFromLogMessage(e.Data, videoDuration);
                Interlocked.Exchange(ref loadingBarProgress, progress);
                Debug.Log("Progress: " + progress);
            }
        };
        
        ffmpegProcess.Start();
        runningProcesses.Add(ffmpegProcess);
        ffmpegProcess.BeginErrorReadLine();
        
        await Task.Run(() => ffmpegProcess.WaitForExit());
        
        
        if (!File.Exists(outputFile))
        {
            popup.ShowPopup($"Failed to compress video, see log for more details. The uncompressed video will be used instead");
            return videoPath;
        }

        return outputFile;
    }*/

    private void OnApplicationQuit()
    {
        cancellationTokenSource.Cancel();
        foreach (Process process in runningProcesses)
        {
            if (!process.HasExited)
                process.Kill();
        }
    }

    private float GetProgressFromLogMessage(string logMessage, int duration)
    {
        if (!logMessage.Contains("time="))
        {
            return -1;
        }

        string split = logMessage.Split("time=")[1];
        string timestampString = split.Substring(0, 11);
        
        int seconds = GetSecondsFromTimestamp(timestampString);
        
        return (float)seconds / duration;
    }
    

    private int GetDurationFromLogMessage(string logMessage)
    {
        if (!logMessage.Contains("Duration:"))
        {
            return -1;
        }

        string split = logMessage.Split("Duration:")[1];
        string durationStringWithLeadingSpace = split.Split(',')[0];
        string durationString = durationStringWithLeadingSpace.Substring(1, durationStringWithLeadingSpace.Length - 1);

        int seconds = GetSecondsFromTimestamp(durationString);
        
        return seconds;
    }

    private int GetSecondsFromTimestamp(string timestamp)
    {
        string[] split= timestamp.Split(':');
        int hours = int.Parse(split[0]);
        int minutes = int.Parse(split[1]);
        int seconds = Mathf.RoundToInt(float.Parse(split[2], CultureInfo.InvariantCulture));
        
        return hours * 3600 + minutes * 60 + seconds;
    }
    
    

    private void Update()
    {
        if (loadingScreen.activeSelf)
        {
            progressBar.localScale = new Vector3(loadingBarProgress, 1, 1);
            consolePreviewText.text = currentConsolePreviewString;
        }
    }

    // From https://github.com/MerlinVR/USharpVideo/blob/d91aa97a3a89aa844ae186405b244d754775808b/Assets/USharpVideo/Scripts/USharpVideoPlayer.cs#L1236
    /// <summary>
    /// Checks for URL sanity and throws warnings if it's not nice.
    /// </summary>
    /// <param name="url"></param>
    private bool ValidateURL(string url)
    {
        
        if (string.IsNullOrWhiteSpace(url)) // Don't do anything if the player entered an empty URL by accident
            return false;

        //if (!url.StartsWith("https://", System.StringComparison.OrdinalIgnoreCase) &&
        //    !url.StartsWith("http://", System.StringComparison.OrdinalIgnoreCase) &&
        //    !IsRTSPURL(url))
        int idx = url.IndexOf("://", System.StringComparison.Ordinal);
        if (idx < 1 || idx > 8) // I'm not sure exactly what rule VRC uses so just check for the :// in an expected spot since it seems like VRC checks that it has a protocol at least.
        {
            UnityEngine.Debug.LogError($"Invalid URL '{url}' provided");
            return false;
        }

        // Longer than most browsers support, see: https://stackoverflow.com/questions/417142/what-is-the-maximum-length-of-a-url-in-different-browsers. I'm not sure if this length will even play in the video player.
        // Most CDN's keep their URLs under 1000 characters so this should be more than reasonable
        // Prevents people from pasting a book and breaking sync on the video player xd
        if (url.Length > 4096)
        {
            UnityEngine.Debug.LogError($"Video URL is too long! url: '{url}'");
            return false;
        }

        return true;
    }
}
