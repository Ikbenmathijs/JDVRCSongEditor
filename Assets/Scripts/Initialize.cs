using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using File = UnityEngine.Windows.File;


public class Initialize : MonoBehaviour
{
    public TextMeshProUGUI currentTaskText;
    public TextMeshProUGUI consoleOutput;
    public ErrorScreen errorScreen;
    
    
    
    void Start()
    {
        InitializeProgram();
    }
    
    
    private async void InitializeProgram()
    {
        
     
        await ClearTempFolder();
        await InitializeYTDLP();
    }

    private async Task ClearTempFolder()
    {
        currentTaskText.text = "Clearing temp folder...";
        
        DirectoryInfo directoryInfo = new DirectoryInfo(Config.tempFolder);
        foreach (FileInfo file in directoryInfo.GetFiles())
        {
            consoleOutput.text += $"Deleting {file.FullName}";
            await Task.Run(() => file.Delete());
        }
    }

    private async Task InitializeYTDLP()
    {
        currentTaskText.text = "Downloading YT-DLP...";
        if (!File.Exists(Config.binariesFolder + "/" + Config.ytdlpName) || !File.Exists(Config.binariesFolder + "/" + Config.ffmpegName))
        {
            await YoutubeDLSharp.Utils.DownloadYtDlp(Config.binariesFolder);
            await YoutubeDLSharp.Utils.DownloadFFmpeg(Config.binariesFolder);
        }

        YoutubeDLSharp.YoutubeDL ytdlp = new YoutubeDLSharp.YoutubeDL();

        ytdlp.YoutubeDLPath = Config.binariesFolder + "/" + Config.ytdlpName;
        ytdlp.FFmpegPath = Config.binariesFolder + "/" + Config.ffmpegName;

        string output = await ytdlp.RunUpdate();
        
        if (output.Contains("ERROR"))
        {
            errorScreen.SetErrorScreen(output);
            return;
        }

        consoleOutput.text += "\n" + output;
        
        Config.initialized = true;
        
        SceneManager.LoadScene("Main");

    }
    
}