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
    public Sprite whiteKeyframeHeadSprite;
    public Sprite normalKeyframeHeadSprite;
    public Sprite hoverKeyframeSprite;
    public Image keyframeHead;
    public Image mainKeyframeIndicator;
    
    public void AddKeyframe(Keyframe keyframe)
    {
        holdingKeyframes.Add(keyframe);
        amountOfKeyframesText.text = holdingKeyframes.Count.ToString();
        if (holdingKeyframes.Count > 1)
        {
            amountOfKeyframesText.gameObject.SetActive(true);
        }
        UpdateKeyframeMenuObject();
    }


    private void UpdateKeyframeMenuObject()
    {
        if (holdingKeyframes[0].instruction.savedInstruction)
        {
            mainKeyframeIndicator.color = holdingKeyframes[0].instruction.savedInstructionColor;
        }
        
        keyframeHead.sprite = whiteKeyframeHeadSprite;
        keyframeHead.color = holdingKeyframes[0].instruction.savedInstructionColor;
        
        foreach (Keyframe keyframe in holdingKeyframes)
        {
            

            if (keyframe.keyframeIndicator != null && keyframe.instruction.savedInstruction)
            {
                keyframe.keyframeIndicator.GetComponent<Image>().color = keyframe.instruction.savedInstructionColor;
            }
        }
    }

    public void OnKeyframeHeadPressed()
    {
        if (holdingKeyframes.Count == 1)
        {
            KeyframeEditor.instance.OpenKeyframeEditor(holdingKeyframes[0]);
            return;
        }
        SelectKeyframePopup.instance.OpenSelectKeyframePopup(this);
    }

    
    // triggered by animation event
    public void KeyframeHoverStart()
    {
        foreach (Keyframe keyframe in holdingKeyframes)
        {
            if (keyframe.keyframeIndicator == null) continue;
            keyframe.keyframeIndicator.GetComponent<Image>().color = selectedColor;
        }
        mainKeyframeIndicator.color = selectedColor;
        keyframeHead.sprite = hoverKeyframeSprite;
        keyframeHead.color = Color.white;
    }
    
    // triggered by animation event
    public void KeyframeHoverEnd()
    {
        foreach (Keyframe keyframe in holdingKeyframes)
        {
            if (keyframe.keyframeIndicator == null) continue;
            keyframe.keyframeIndicator.GetComponent<Image>().color = keyframe.instruction.savedInstruction ? keyframe.instruction.savedInstructionColor : unselectedColor;
        }
        mainKeyframeIndicator.color = holdingKeyframes[0].instruction.savedInstruction ? holdingKeyframes[0].instruction.savedInstructionColor : unselectedColor;

        if (holdingKeyframes.Count == 1 && holdingKeyframes[0].instruction.savedInstruction)
        {
            keyframeHead.sprite = whiteKeyframeHeadSprite;
            keyframeHead.color = holdingKeyframes[0].instruction.savedInstructionColor;
        }
        else
        {
            keyframeHead.sprite = normalKeyframeHeadSprite;
            keyframeHead.color = Color.white;
        }
    }
}
