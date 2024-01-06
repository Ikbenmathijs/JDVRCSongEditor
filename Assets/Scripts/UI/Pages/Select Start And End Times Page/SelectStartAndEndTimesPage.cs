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
        StartCoroutine(PauseAfterShortTime());
    }
    private IEnumerator PauseAfterShortTime()
    {
        yield return new WaitForSeconds(0.1f);
        videoPlayer.Pause();
    }

    public void PlayPause()
    {
        if (paused)
        {
            videoPlayer.Play();
            pauseIconAnimator.SetBool("Paused", false);
        }
        else
        {
            videoPlayer.Pause();
            pauseIconAnimator.SetBool("Paused", true);
        }

        paused = !paused;
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

    public void OnProgressSliderChanged()
    {
        if (!dragging) return;
        videoPlayer.time = videoPlayer.length * videoProgressSlider.value;
    }


    private void Update()
    {
        if (videoPlayer.isPlaying && !dragging)
        {
            videoProgressSlider.value = (float)(videoPlayer.time / videoPlayer.length);
        }
        else if (!dragging)
        {
            videoPlayer.time = videoPlayer.length * videoProgressSlider.value;
        }
    }
}
