using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsPreviewInterpreter : MonoBehaviour
{
    public LightingEditorVideoPlayer videoPlayer;
    private int executedKeyframeIndex = -1;
    public static LightsPreviewInterpreter instance;
    private Color fadingFrom;
    private Color fadingTo;
    private float fadeSpeed;
    private float fadingTime;
    private bool isFading;
    
    public LightsPreviewInterpreter()
    {
        instance = this;
    }
    
    private void Update()
    {
        InstructionExecutionUpdate();
        FadeUpdate();
    }

    public void RefreshExecutedKeyframes(bool useCustomTime = false, float customTime = 0f)
    {
        int index = -1;
        for (int i = 0; i < KeyframesManager.instance.sortedKeyframes.Count; i++)
        {
            if (KeyframesManager.instance.sortedKeyframes[i].time <= (useCustomTime ? customTime : videoPlayer.GetVideoTime()))
            {
                index = i;
            }
        }
    
        for (int i = 0; i < index + 1; i++)
        {
            ExecuteInstruction(KeyframesManager.instance.sortedKeyframes[i].instruction);
        }
        executedKeyframeIndex = index;
    }

    private void FadeUpdate()
    {
        if (!isFading) return;
        fadingTime += Time.deltaTime * fadeSpeed;
        if (fadingTime >= 1f)
        {
            isFading = false;
            AudioVisualiser.instance.defaultColor = fadingTo;
        }
        else
        {
            AudioVisualiser.instance.defaultColor = Color.Lerp(fadingFrom, fadingTo, fadingTime);
        }
    }

    private void InstructionExecutionUpdate()
    {
        if (!videoPlayer.IsPlaying()) return;

        int nextToExecuteKeyframeIndex = executedKeyframeIndex + 1;
        if (nextToExecuteKeyframeIndex >= KeyframesManager.instance.sortedKeyframes.Count) return;

        
        if (videoPlayer.GetVideoTime() >= KeyframesManager.instance.sortedKeyframes[nextToExecuteKeyframeIndex].time)
        {
            ExecuteInstruction(KeyframesManager.instance.sortedKeyframes[nextToExecuteKeyframeIndex].instruction);
            executedKeyframeIndex = nextToExecuteKeyframeIndex;
        }
    }


    private void ExecuteInstruction(Instruction instruction)
    {
        if (instruction.instructionType == InstructionType.SetColors)
        {
            if (instruction.colors.Count > 0)
            {
                AudioVisualiser.instance.colorList = instruction.colors.ToArray();
            }

            if (instruction.changeBackgroundColor)
            {
                if (instruction.fadeBackgroundColor)
                {
                    fadingFrom = AudioVisualiser.instance.defaultColor;
                    fadingTo = instruction.backgroundColor;
                    fadeSpeed = instruction.backgroundColorFadeSpeed;
                    fadingTime = 0f;
                    isFading = true;
                }
                else
                {
                    AudioVisualiser.instance.defaultColor = instruction.backgroundColor;
                }
            }
            
        }
    }
}
