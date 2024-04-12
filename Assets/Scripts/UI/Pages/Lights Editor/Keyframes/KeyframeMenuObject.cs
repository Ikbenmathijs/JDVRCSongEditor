using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyframeMenuObject : MonoBehaviour
{
    public TextMeshProUGUI amountOfKeyframesText;
    
    public List<Keyframe> holdingKeyframes = new List<Keyframe>();
    public Color selectedColor;
    public Color unselectedColor;
    public SelectKeyframePopup selectKeyframePopup;
    
    public void AddKeyframe(Keyframe keyframe)
    {
        holdingKeyframes.Add(keyframe);
        amountOfKeyframesText.text = holdingKeyframes.Count.ToString();
        if (holdingKeyframes.Count > 1)
        {
            amountOfKeyframesText.gameObject.SetActive(true);
        }
    }

    public void OnKeyframeHeadPressed()
    {
        if (selectKeyframePopup == null)
        {
            Debug.LogError("SelectKeyframePopup is null");
            return;
        }
        
        selectKeyframePopup.OpenKeyframeDetailsListPopup(holdingKeyframes);
    }

    
    // triggered by animation event
    public void KeyframeHoverStart()
    {
        foreach (Keyframe keyframe in holdingKeyframes)
        {
            if (keyframe.keyframeIndicator == null) continue;
            keyframe.keyframeIndicator.GetComponent<Image>().color = selectedColor;
        }
    }
    
    // triggered by animation event
    public void KeyframeHoverEnd()
    {
        foreach (Keyframe keyframe in holdingKeyframes)
        {
            if (keyframe.keyframeIndicator == null) continue;
            keyframe.keyframeIndicator.GetComponent<Image>().color = unselectedColor;
        }
    }
}
