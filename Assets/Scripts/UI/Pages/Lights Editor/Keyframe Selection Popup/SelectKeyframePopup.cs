using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectKeyframePopup : MonoBehaviour
{
    public static SelectKeyframePopup instance;
    public Animator keyframePopupAnimator;
    public Transform keyframeDetailsParent;
    public GameObject keyframeDetailsPrefab;
    public KeyframesManager keyframesManager;
    private KeyframeMenuObject clickedKeyframeMenuObject;

    private bool showAllKeyframes;
    
    public SelectKeyframePopup()
    {
        instance = this;
    }


    public void OpenKeyframesListButtonPressed()
    {
        OpenSelectKeyframePopup(null);
    }
    
    public void OpenSelectKeyframePopup(KeyframeMenuObject keyframeMenuObject)
    {
        keyframePopupAnimator.SetBool("Open", true);

        showAllKeyframes = keyframeMenuObject == null;
        clickedKeyframeMenuObject = keyframeMenuObject;
        
        UpdateKeyframesList();
    }
    
    
    public void UpdateKeyframesList()
    {
        List<Keyframe> currentKeyframeGroup = null;
        if (!showAllKeyframes && clickedKeyframeMenuObject != null)
        {
            currentKeyframeGroup = clickedKeyframeMenuObject.holdingKeyframes;
        }
        else
        {
            currentKeyframeGroup = keyframesManager.keyframes;
        }
        
        if (currentKeyframeGroup == null || currentKeyframeGroup.Count == 0)
        {
            currentKeyframeGroup = keyframesManager.keyframes;
        }
        
        currentKeyframeGroup.Sort();
        
    
        foreach (Transform child in keyframeDetailsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Keyframe keyframe in currentKeyframeGroup)
        {
            GameObject keyframeDetails = Instantiate(keyframeDetailsPrefab, keyframeDetailsParent);
            keyframeDetails.GetComponent<KeyframeDetails>().SetKeyframeDetails(keyframe);
        }
    }
    
    
    public void CloseSelectKeyframePopup()
    {
        keyframePopupAnimator.SetBool("Open", false);
        foreach (Transform child in keyframeDetailsParent)
        {
            Destroy(child.gameObject);
        }
    }
    
    
}
