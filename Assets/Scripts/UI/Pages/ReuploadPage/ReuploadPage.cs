using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class ReuploadPage : Page
{
    // if I would rewrite this program I'd use dependency injection instead of this shit
    [SerializeField] private bool convertingFromOldFormat = false;

    [SerializeField] private GameObject[] instructionPages;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button previousPageButton;
    [SerializeField] private TMP_InputField urlField;
    
    private int currentPageIndex = 0;




    public void NextInstructionPage()
    {
        if (currentPageIndex >= instructionPages.Length - 1) return;
        instructionPages[currentPageIndex].SetActive(false);
        currentPageIndex++;
        instructionPages[currentPageIndex].SetActive(true);
        
        previousPageButton.interactable = currentPageIndex > 0;
        nextPageButton.interactable = currentPageIndex < instructionPages.Length - 1;
    }
    
    public void PreviousInstructionPage()
    {
        if (currentPageIndex <= 0) return;
        instructionPages[currentPageIndex].SetActive(false);
        currentPageIndex--;
        instructionPages[currentPageIndex].SetActive(true);
        
        previousPageButton.interactable = currentPageIndex > 0;
        nextPageButton.interactable = currentPageIndex < instructionPages.Length - 1;
    }


    public void ShowVideoFileButtonPressed()
    {
        if (convertingFromOldFormat)
        {
            ExploreFile(Config.videoStoragePath + "/video.mp4");
        }
        else
        {
            ExploreFile(SongData.videoPath);
        }
    }

    public void UrlInputFieldChanged()
    {
        SetNextPageAvailable(urlField.text.StartsWith("https://www.youtube.com/watch?v=") || urlField.text.StartsWith("https://youtube.com/watch?v=") ||urlField.text.StartsWith("https://www.youtu.be/") || urlField.text.StartsWith("https://youtu.be/"));
        previousPageButton.interactable = currentPageIndex > 0;
        nextPageButton.interactable = currentPageIndex < instructionPages.Length - 1;


        if (convertingFromOldFormat)
        {
            FormatConvertionGlobals.exportData.videoUrl = urlField.text;
        }
        else
        {
            SongData.videoUrl = urlField.text;
        }
    }
    
    
    public override void InitializePage()
    {
        SetNextPageAvailable(urlField.text.StartsWith("https://www.youtube.com/watch?v=") || urlField.text.StartsWith("https://youtube.com/watch?v=") ||urlField.text.StartsWith("https://www.youtu.be/") || urlField.text.StartsWith("https://youtu.be/"));
    }
    
    private void Start()
    {
        PageName = "Re-Upload Video To Youtube";
    }
    
    
    
    
    
    private bool ExploreFile(string filePath) {
        if (!System.IO.File.Exists(filePath)) {
            return false;
        }
        //Clean up file path so it can be navigated OK
        filePath = System.IO.Path.GetFullPath(filePath);
        System.Diagnostics.Process.Start("explorer.exe", string.Format("/select,\"{0}\"", filePath));
        return true;
    }
}
