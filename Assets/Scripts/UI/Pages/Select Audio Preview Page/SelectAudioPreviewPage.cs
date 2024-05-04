using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class SelectAudioPreviewPage : Page
{
    public VideoPlayer videoPlayer;
    public Slider videoProgressSlider;
    private bool dragging;
    public RectTransform sliderTransform;
    public RectTransform startMarker;
    public Animator pauseIconAnimator;
    public bool paused;
    

    public SelectAudioPreviewPage()
    {
        PageName = "Set Song Preview";
    }
    
    public override void InitializePage()
    {
        string videoPath = SongData.videoPath;
        videoPlayer.url = $"file://{videoPath}";
        videoPlayer.Play();
        StartCoroutine(FinishPageInitialization());
    }
    private IEnumerator FinishPageInitialization()
    {
        yield return new WaitForSeconds(0.2f);
        videoPlayer.Pause();
    }
    
    
    
    public void PlayPause()
    {
        SetPaused(!paused);
    }


    public void SetPaused(bool value)
    {
        paused = value;
        if (!paused)
        {
            videoPlayer.Play();
            pauseIconAnimator.SetBool("Paused", false);
        }
        else
        {
            videoPlayer.Pause();
            pauseIconAnimator.SetBool("Paused", true);
        }
    }
    
    public void SetAudioPreviewTime()
    {
        SongData.audioPreviewStartTime = (float)videoPlayer.time;
        Rect sliderRect = sliderTransform.rect;
        startMarker.anchoredPosition = new Vector2(sliderRect.x + sliderRect.width * videoProgressSlider.value, startMarker.anchoredPosition.y);
        SetNextPageAvailable(true);
    }
    
    
    public void BeginDrag()
    {
        dragging = true;
    }
    
    public void EndDrag()
    {
        dragging = false;
    }
    


    private void Update()
    {
        if (dragging)
        {
            videoPlayer.time = videoPlayer.length * videoProgressSlider.value;
        }
        else
        {
            float v = (float)(videoPlayer.time / videoPlayer.length);
            if (!float.IsNaN(v))
            {
                videoProgressSlider.value = v;
            }
        }
    }

    public override void OnPageUnfocus()
    {
        SetPaused(true);
    }
    
    
    
}
