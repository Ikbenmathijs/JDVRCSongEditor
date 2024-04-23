using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenVideoPlayer : MonoBehaviour
{
    public Animator fullscreenSwitcherAnimator;
    public GameObject infoPanel;
    public GameObject videoScreen;
    public GameObject previewScreen;
    
    
    public void OpenVideoFullscreen()
    {
        fullscreenSwitcherAnimator.SetBool("Fullscreen", true);
        infoPanel.SetActive(false);
        videoScreen.SetActive(true);
        previewScreen.SetActive(false);
    }

    public void OpenPreviewFullscreen()
    {
        fullscreenSwitcherAnimator.SetBool("Fullscreen", true);
        infoPanel.SetActive(false);
        videoScreen.SetActive(false);
        previewScreen.SetActive(true);
    }
    
    
    public void CloseFullscreen()
    {
        fullscreenSwitcherAnimator.SetBool("Fullscreen", false);
        infoPanel.SetActive(true);
    }
}
