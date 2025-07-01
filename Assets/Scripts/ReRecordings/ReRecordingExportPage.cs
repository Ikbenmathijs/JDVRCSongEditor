using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;
using SFB;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReRecordingExportPage : Page
{
    [Header("Dependencies")]
    [SerializeField] private Popup popup;
    
    [Header("UI Elements")]
    [SerializeField] private GameObject exportButton;
    [SerializeField] private GameObject loadingText;
    [SerializeField] private GameObject restartProgramButton;

    private void Start()
    {
        PageName = "Export Re-Recording";
    }

    public override void InitializePage()
    {
        SetNextPageAvailable(false);
    }
    
    
    public void ExportButtonPressed()
    {
        try
        {
            ExportData exportData = new ExportData
            {
                version = 2,
                isRerecording = true,
                originalImageName = ReRecordingsGlobals.rerecodingSong.image,
                name = ReRecordingsGlobals.rerecodingSong.name,
                artist = ReRecordingsGlobals.rerecodingSong.artist,
                videoUrl = ReRecordingsGlobals.rerecodingSong.url,
                dancerAmount = int.Parse(ReRecordingsGlobals.rerecodingSong.amountOfDancers),
                recordings = SongData.recordings
            };
        
            string savePath = StandaloneFileBrowser.SaveFilePanel(
                "Export Song",
                "",
                Util.RemoveCharsFromString(exportData.name, new char[] { '<', '>', ':', '"', '/', '\\', '|', '?', '*' }),
                "jdvrcsongrerecording");

            if (savePath == "")
            {
                popup.ShowPopup("Please select a save location");
                loadingText.SetActive(false);
                exportButton.SetActive(true);
                return;
            }
            
            
            DirectoryInfo directoryInfo = new DirectoryInfo(Config.zippableFolder);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
            
            
            string jsonString = JsonConvert.SerializeObject(exportData);
            File.WriteAllText(Config.zippableFolder + "/data.json", jsonString);
            
            ZipFile.CreateFromDirectory(Config.zippableFolder, savePath, System.IO.Compression.CompressionLevel.Optimal, false);
            popup.ShowPopup("Exported song to " + savePath);
            loadingText.SetActive(false);
            exportButton.SetActive(true);
            restartProgramButton.SetActive(true);
            
            
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to export: " + e.Message);
            popup.ShowPopup("Failed to export: " + e.Message);
            loadingText.SetActive(false);
            exportButton.SetActive(true);
            throw e;
        }
    }
    
    
    public void RestartProgramButtonPressed()
    {
        SceneManager.LoadScene("Init");
    }
}
