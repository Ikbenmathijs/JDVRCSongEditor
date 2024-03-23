using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;
public class SelectStartAndEndTimesPage : Page
{
    public VideoPlayer videoPlayer;
    public Slider videoProgressSlider;
    private bool dragging = false;
    public RectTransform sliderTransform;
    public RectTransform startMarker;
    public RectTransform endMarker;
    public Animator pauseIconAnimator;
    public bool paused = true;
    private void Start()
    {
        PageName = "Select start and end times";
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
        yield return new WaitForSeconds(0.1f);
        videoPlayer.Pause();

        SongData.startTime = 0f;
        SongData.endTime = (float)videoPlayer.length - 1f;
        SetNextPageAvailable(true);
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
    
    public void SetStartTime()
    {
        SongData.startTime = (float)videoPlayer.time;
        Rect sliderRect = sliderTransform.rect;
        startMarker.anchoredPosition = new Vector2(sliderRect.x + sliderRect.width * videoProgressSlider.value, startMarker.anchoredPosition.y);
    }
    
    public void SetEndTime()
    {
        SongData.endTime = (float)videoPlayer.time;
        Rect sliderRect = sliderTransform.rect;
        endMarker.anchoredPosition = new Vector2(sliderRect.x + sliderRect.width * videoProgressSlider.value, endMarker.anchoredPosition.y);
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
