using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;
using SFB;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExportConvertedPage : Page
{
    [Header("Dependencies")]
    [SerializeField] private Popup popup;
    
    [Header("UI Elements")]
    [SerializeField] private GameObject exportButton;
    [SerializeField] private GameObject loadingText;
    [SerializeField] private GameObject restartProgramButton;

    private void Start()
    {
        PageName = "Export converted data";
    }

    public override void InitializePage()
    {
        SetNextPageAvailable(false);
    }
    
    
    public void ExportButtonPressed()
    {
        try
        {
            FormatConvertionGlobals.exportData.version = 2;
        
            string savePath = StandaloneFileBrowser.SaveFilePanel(
                "Export Song",
                "",
                Util.RemoveCharsFromString(FormatConvertionGlobals.exportData.name + "v2", new char[] { '<', '>', ':', '"', '/', '\\', '|', '?', '*' }),
                "jdvrcsong");

            if (savePath == "")
            {
                popup.ShowPopup("Please select a save location");
                loadingText.SetActive(false);
                exportButton.SetActive(true);
                return;
            }
            
            
            string jsonString = JsonConvert.SerializeObject(FormatConvertionGlobals.exportData);
            File.WriteAllText(Config.tempFolder + "/unzipped/data.json", jsonString);
            
            ZipFile.CreateFromDirectory(Config.tempFolder + "/unzipped", savePath, System.IO.Compression.CompressionLevel.Optimal, false);
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
