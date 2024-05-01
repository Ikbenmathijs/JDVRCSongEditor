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
    public float currentBrightness = 1f;
    
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

        Reset();
        
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

    private void Reset()
    {
        currentBrightness = 1f;
        AudioVisualiser.instance.filled = false;
        AudioVisualiser.instance.beatSolidColors = false;
        AudioVisualiser.instance.intervalSolidColors = false;
        AudioVisualiser.instance.disableBeat = false;
        AudioVisualiser.instance.disableInterval = false;
    }


    private void ExecuteInstruction(Instruction instruction)
    {
        if (instruction.instructionType == InstructionType.SetColors)
        {
            if (instruction.colors.Count > 0)
            {
                Color[] colors = new Color[instruction.colors.Count];
                for (int i = 0; i < instruction.colors.Count; i++)
                {
                    colors[i] = instruction.colors[i] * currentBrightness;
                }
                AudioVisualiser.instance.colorList = colors;
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
                    isFading = false;
                }
            }
        } else if (instruction.instructionType == InstructionType.SetBrightness)
        {
            float brightness = instruction.brightnessMultiplier;
            for (int i = 0; i < AudioVisualiser.instance.colorList.Length; i++)
            {
                AudioVisualiser.instance.colorList[i] *= brightness;
            }
            currentBrightness = brightness;
        } else if (instruction.instructionType == InstructionType.SetFill)
        {
            AudioVisualiser.instance.fillColor = instruction.fillColor;
            AudioVisualiser.instance.filled = true;
        } else if (instruction.instructionType == InstructionType.DisableFill)
        {
            AudioVisualiser.instance.filled = false;
        } else if (instruction.instructionType == InstructionType.SolidColors)
        {
            AudioVisualiser.instance.beatSolidColors = instruction.solidColorOnBeat;
            AudioVisualiser.instance.intervalSolidColors = instruction.solidColorsOnInterval;
        } else if (instruction.instructionType == InstructionType.Disable)
        {
            AudioVisualiser.instance.disableBeat = instruction.disableBeat;
            AudioVisualiser.instance.disableInterval = instruction.disableInterval;
        }
    }
}
