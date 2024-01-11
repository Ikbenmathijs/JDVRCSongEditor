using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LightingEditorVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private bool paused = true;
    public Animator pauseIconAnimator;
    public Slider videoProgressSlider;
    private bool dragging = false;
    public void SetUrl(string url)
    {
        videoPlayer.url = url;
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
        if (dragging && videoPlayer.isPlaying)
        {
            videoPlayer.time = videoPlayer.length * videoProgressSlider.value;
        } else
        {
            float v = (float)(videoPlayer.time / videoPlayer.length);
            if (!float.IsNaN(v))
            {
                videoProgressSlider.value = v;
            }
        }
    }
}
