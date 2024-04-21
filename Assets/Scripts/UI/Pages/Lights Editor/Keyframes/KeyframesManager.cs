using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class KeyframesManager : MonoBehaviour
{
    public static KeyframesManager instance;
    public LightingEditorVideoPlayer videoPlayer;
    public GameObject keyframePrefab;
    public GameObject keyframeIndicatorPrefab;
    public Transform keyframesParent;
    public RectTransform keyframeObjectStartPosition;
    public RectTransform keyframeObjectEndPosition;
    
    public List<Keyframe> keyframes = new List<Keyframe>();
    
    public KeyframesManager()
    {
        instance = this;
    }
    
    


    public void AddKeyFrameButtonPressed()
    {
        float time = videoPlayer.GetVideoTime();
        Keyframe keyframe = new Keyframe(InstructionType.None, time);
        keyframes.Add(keyframe);
        UpdateKeyframeMarkers();
        KeyframeEditor.instance.OpenKeyframeEditor(keyframe);
        SelectKeyframePopup.instance.UpdateKeyframesList();
    }

    public void UpdateKeyframeMarkers()
    {
        Debug.Log("Amount of keyframes: " + keyframes.Count);
        for (int i = 0; i < keyframesParent.childCount; i++)
        {
            Destroy(keyframesParent.GetChild(i).gameObject);
        }
        
        for (int i = 0; i < keyframes.Count; i++)
        {
            keyframes[i].keyframeMarker = null;
            // find a keyframe nearby, if there is one combine them into 1 marker and increase the number on the marker
            bool foundNearbyKeyframe = false;
            // loop through all previously created keyframes
            for (int j = 0; j < i; j++)
            {
                
                if (Mathf.Abs(keyframes[i].time - keyframes[j].time) < videoPlayer.GetVideoLength() / 35f // are the keyframe times close enough?
                && keyframes[j].keyframeMarker != null 
                && keyframes[j].keyframeMarker.GetComponent<KeyframeMenuObject>().holdingKeyframes[0] == keyframes[j]) // is this the keyframe which originally created the marker?
                {
                    Debug.Log("Found nearby keyframe");
                    keyframes[i].keyframeMarker = keyframes[j].keyframeMarker;
                    // keyframe menu object handles updating the text
                    keyframes[i].keyframeMarker.GetComponent<KeyframeMenuObject>().AddKeyframe(keyframes[i]);
                    foundNearbyKeyframe = true;
                    break;
                }
            }
            
            
            if (!foundNearbyKeyframe)
            {
                // create new keyframe menu object (marker head)
                GameObject keyframeInstance = Instantiate(keyframePrefab, keyframesParent);
                RectTransform rectTransform = keyframeInstance.GetComponent<RectTransform>();
                float time = keyframes[i].time;
                float posX = Mathf.Lerp(keyframeObjectStartPosition.position.x, keyframeObjectEndPosition.position.x, time / (float)videoPlayer.videoPlayer.length);
                rectTransform.position = new Vector3(posX, keyframeObjectStartPosition.position.y, 0f);
                keyframes[i].keyframeMarker = keyframeInstance;
                KeyframeMenuObject keyframeMenuObject = keyframeInstance.GetComponent<KeyframeMenuObject>();
                keyframeMenuObject.AddKeyframe(keyframes[i]);
            }
            else
            {
                // create new line
                GameObject indicatorInstance = Instantiate(keyframeIndicatorPrefab, keyframesParent);
                RectTransform rectTransform = indicatorInstance.GetComponent<RectTransform>();
                float time = keyframes[i].time;
                float posX = Mathf.Lerp(keyframeObjectStartPosition.position.x, keyframeObjectEndPosition.position.x, time / (float)videoPlayer.videoPlayer.length);
                rectTransform.position = new Vector3(posX, keyframeObjectStartPosition.position.y, 0f);
                keyframes[i].keyframeIndicator = indicatorInstance;
            }
            
        }
    }
}


