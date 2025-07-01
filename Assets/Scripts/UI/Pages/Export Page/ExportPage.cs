using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using SFB;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class ExportPage : Page
{
    public Popup popup;
    public GameObject ExportButton;
    public GameObject LoadingText;
    [FormerlySerializedAs("RestartButton")] public GameObject RestartProgramButton;
    
    public ExportPage()
    {
        PageName = "Export Song";
    }
    
    
    public void ExportSongButtonPressed()
    {
        StartCoroutine(ExportSongButtonPressedCoRoutine());
    }
    private IEnumerator ExportSongButtonPressedCoRoutine()
    {
        LoadingText.SetActive(true);
        ExportButton.SetActive(false);
        yield return null;
        ExportSong();
    }

    private async Task ExportSong()
    {
        try
        {
            string savePath = StandaloneFileBrowser.SaveFilePanel(
                "Export Song",
                "",
                Util.RemoveCharsFromString(SongData.name, new char[] { '<', '>', ':', '"', '/', '\\', '|', '?', '*' }),
                "jdvrcsong");

            if (savePath == "")
            {
                popup.ShowPopup("Please select a save location");
                LoadingText.SetActive(false);
                ExportButton.SetActive(true);
                return;
            }


            DirectoryInfo directoryInfo = new DirectoryInfo(Config.zippableFolder);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                await Task.Run(() => file.Delete());
            }



            ExportData exportData = new ExportData
            {
                version = 2,
                isRerecording = false,
                originalImageName = null,
                videoUrl = SongData.videoUrl,
                name = SongData.name,
                artist = SongData.artist,
                duration = SongData.duration,
                dancerAmount = SongData.dancerAmount,
                difficulty = SongData.difficulty,
                recordings = SongData.recordings,
                game = SongData.game,
                dancerImageSet = SongData.dancerImageSet,
                dancerImages = new string[SongData.dancerImagePaths.Length],
                startTime = SongData.startTime,
                endTime = SongData.endTime,
                audioPreviewStartTime = SongData.audioPreviewStartTime,
                lightingData = LightingCodeGen.GenerateCode()
            };

            string jsonString = JsonConvert.SerializeObject(exportData);

            File.WriteAllText($"{Config.zippableFolder}/data.json", jsonString);
            
            //File.Copy(SongData.videoPath, $"{Config.zippableFolder}/video.mp4");
            File.Copy(SongData.imagePath, $"{Config.zippableFolder}/image.png");
            for (int i = 0; i < SongData.dancerAmount; i++)
            {
                if (SongData.dancerImagePaths[i] == "" || !File.Exists(SongData.dancerImagePaths[i])) continue;
                File.Copy(SongData.dancerImagePaths[i], $"{Config.zippableFolder}/dancer{i}.png");
            }
            
            ZipFile.CreateFromDirectory(Config.zippableFolder, savePath, System.IO.Compression.CompressionLevel.Optimal, false);
            popup.ShowPopup("Exported song to " + savePath);
            LoadingText.SetActive(false);
            ExportButton.SetActive(true);
            RestartProgramButton.SetActive(true);
        }
        catch (System.Exception e)
        {
            popup.ShowPopup("An error occurred: " + e.Message);
            LoadingText.SetActive(false);
            ExportButton.SetActive(true);
        }
    }


    public void RestartProgramButtonPressed()
    {
        SceneManager.LoadScene("Init");
    }
}



public class ExportData
{
    public int? version;
    public bool isRerecording;
    [CanBeNull] public string originalImageName;
    [CanBeNull] public string videoUrl;
    public string name;
    public string artist;
    public string duration;
    public int dancerAmount;
    public int difficulty;
    public string game;
    public bool[] dancerImageSet;
    public string[] dancerImages;
    public float startTime;
    public float endTime;
    public float audioPreviewStartTime;
    public string lightingData;
    public string[] recordings;
}