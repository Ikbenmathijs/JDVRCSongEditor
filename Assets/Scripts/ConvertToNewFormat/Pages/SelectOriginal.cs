using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using SFB;
using UnityEngine;
using Newtonsoft.Json;

public class SelectOriginal : Page
{
    [Header("Dependencies")]
    [SerializeField] private Popup popup;

    private bool filePicked = false;


    private void Start()
    {
        PageName = "Select Original Song File";
    }
    
    public override void InitializePage()
    {
        SetNextPageAvailable(filePicked);
    }
    
    
    public void PickFileButtonPressed()
    {
        try
        {
            string[] files = StandaloneFileBrowser.OpenFilePanel("Select Song File", "", "jdvrcsong", false);

            if (files.Length != 1)
            {
                filePicked = false;
                SetNextPageAvailable(false);
                popup.ShowPopup("Please select 1 JDVRCSong file");
                return;
            }
        
            if (!Directory.Exists( Config.tempFolder + "/unzipped"))
            {
                Directory.CreateDirectory(Config.tempFolder + "/unzipped");
            }
            
            DirectoryInfo directoryInfo = new DirectoryInfo(Config.tempFolder);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
            
            DirectoryInfo directoryInfoUnzipped = new DirectoryInfo(Config.tempFolder + "/unzipped");
            foreach (FileInfo file in directoryInfoUnzipped.GetFiles())
            {
                file.Delete();
            }
        
            string path = files[0];
            ZipFile.ExtractToDirectory(path, Config.tempFolder + "/unzipped", true);
        
            if (!File.Exists(Config.tempFolder + "/unzipped/data.json"))
            {
                filePicked = false;
                SetNextPageAvailable(false);
                popup.ShowPopup("This file is not a valid JDVRCSong file (data.json not found)");
                return;
            }
            string jsonDataString = File.ReadAllText(Config.tempFolder + "/unzipped" + "/data.json");

            ExportData songData = JsonConvert.DeserializeObject<ExportData>(jsonDataString);
            
            
            if (songData.version != null && songData.version >= 2)
            {
                Debug.Log(songData.version);
                filePicked = false;
                SetNextPageAvailable(false);
                popup.ShowPopup("This file is already in the new format.");
                return;
            }
            
            
            if (!File.Exists(Config.tempFolder + "/unzipped" + "/image.png"))
            {
                filePicked = false;
                SetNextPageAvailable(false);
                popup.ShowPopup("This file is not a valid JDVRCSong file (image.json not found)");
                return;
            }
            
        
            if (!File.Exists(Config.tempFolder + "/unzipped" + "/video.mp4"))
            {
                filePicked = false;
                SetNextPageAvailable(false);
                popup.ShowPopup("This file is not a valid JDVRCSong file (video.mp4 not found)");
                return;
            }

            
            
            int dancerAmount = songData.dancerAmount;
            if (dancerAmount > 1)
            {
                for (int i = 0; i < dancerAmount; i++)
                {
                    if (!File.Exists(Config.tempFolder + "/unzipped" + $"/dancer{i}.png"))
                    {
                        filePicked = false;
                        SetNextPageAvailable(false);
                        popup.ShowPopup($"This file is not a valid JDVRCSong file (dancer{i}.png not found)");
                        return;
                    }
                }
            }
            
            
            if (!Directory.Exists(Config.videoStoragePath))
            {
                Directory.CreateDirectory(Config.videoStoragePath);
            }
            
            
            if (File.Exists(Config.videoStoragePath + "/video.mp4")) {
                File.Delete(Config.videoStoragePath + "/video.mp4");
            }
            File.Move(Config.tempFolder + "/unzipped" + "/video.mp4", Config.videoStoragePath + "/video.mp4");
            
            FormatConvertionGlobals.exportData = songData;
            
            popup.ShowPopup($"Song \"{songData.name}\" imported successfully!");
            
            SetNextPageAvailable(true);
        } catch (System.Exception e)
        {
            filePicked = false;
            SetNextPageAvailable(false);
            popup.ShowPopup($"An error occurred while reading the file: {e.Message}"); 
            throw e;
        }
    }
}
