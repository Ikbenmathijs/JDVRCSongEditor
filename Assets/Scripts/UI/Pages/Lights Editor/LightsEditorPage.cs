using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsEditorPage : Page
{
    public LightingEditorVideoPlayer videoPlayer;
    void Start()
    {
        PageName = "Light Show Editor";
    }


    public override void InitializePage()
    {
        string videoPath = SongData.videoPath;
        videoPlayer.SetUrl($"file://{videoPath}");
        SetNextPageAvailable(true);
    }

    public override void OnPageUnfocus()
    {
        videoPlayer.SetPaused(true);
    }
    
    
}
