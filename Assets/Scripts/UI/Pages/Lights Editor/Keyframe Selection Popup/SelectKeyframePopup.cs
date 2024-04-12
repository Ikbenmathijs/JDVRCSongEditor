using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectKeyframePopup : MonoBehaviour
{
    public Animator keyframePopupAnimator;
    public Transform keyframeDetailsParent;
    public GameObject keyframeDetailsPrefab;
    public Toggle showAllKeyframesToggle;
    public KeyframesManager keyframesManager;

    private List<Keyframe> currentKeyframeGroup; // The group of keyframes the user clicked on
    
    
    public void OpenKeyframeDetailsListPopup(List<Keyframe> keyframeGroup)
    {
        keyframePopupAnimator.SetBool("Open", true);
        
        
        keyframeGroup.Sort();
        currentKeyframeGroup = keyframeGroup;

        if (showAllKeyframesToggle.isOn)
        {
            keyframeGroup = keyframesManager.keyframes;
            keyframeGroup.Sort();
        }
        
        UpdateKeyframesList(keyframeGroup);
    }


    public void UpdateKeyframesList(List<Keyframe> keyframeGroup)
    {
        keyframeGroup.Sort();
    
        foreach (Transform child in keyframeDetailsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Keyframe keyframe in keyframeGroup)
        {
            GameObject keyframeDetails = Instantiate(keyframeDetailsPrefab, keyframeDetailsParent);
            keyframeDetails.GetComponent<KeyframeDetails>().SetKeyframeDetails(keyframe);
        }
    }
    
    
    public void ShowAllKeyframesToggled()
    {
        if (showAllKeyframesToggle.isOn)
        {
            UpdateKeyframesList(keyframesManager.keyframes);
        }
        else
        {
            UpdateKeyframesList(currentKeyframeGroup);
        }
    }
    
    public void CloseKeyframeDetailsListPopup()
    {
        keyframePopupAnimator.SetBool("Open", false);
        foreach (Transform child in keyframeDetailsParent)
        {
            Destroy(child.gameObject);
        }
    }
    
    
}
