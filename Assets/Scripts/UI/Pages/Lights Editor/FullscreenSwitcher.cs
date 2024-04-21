using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenVideoPlayer : MonoBehaviour
{
    public Animator fullscreenSwitcherAnimator;
    public GameObject infoPanel;
    
    
    public void OpenFullscreen()
    {
        fullscreenSwitcherAnimator.SetBool("Fullscreen", true);
        infoPanel.SetActive(false);
    }
    
    
    public void CloseFullscreen()
    {
        fullscreenSwitcherAnimator.SetBool("Fullscreen", false);
        infoPanel.SetActive(true);
    }
}
