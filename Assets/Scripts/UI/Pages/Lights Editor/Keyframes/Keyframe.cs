using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;


public class Keyframe : IComparable<Keyframe>
{
    public InstructionType instructionType;
    public float time;
    public GameObject keyframeMarker;
    
    public List<Color> colors = new List<Color>();
    public float fadeTime = 1f; // set if instructionType is LerpDefault or combination
    public bool changeBackgroundColor = false;
    public Color backgroundColor = Color.white; // set if instructionType is SetColors or LerpDefault or combination
    public bool fadeBackgroundColor = false;
    public float backgroundColorLerpSpeed = 1f;
    [CanBeNull] public GameObject keyframeIndicator; // is null if the indicator is tied to a keyframe marker exactly (because it's the first keyframe in a group)

    public int CompareTo(Keyframe other)
    {
        if (other == null)
            return 1;
        
        return time.CompareTo(other.time);
    }

    public Keyframe(InstructionType instructionType, float time)
    {
        this.instructionType = instructionType;
        this.time = time;
    }
}


public enum InstructionType
{
    None,
    SetColors, // this translates to a combi of lerpdefault and setcolors in the 'language' to prevent confusion
}

public static class InstructionTypeExtensions
{
    public static string ToFriendlyString(this InstructionType instructionType)
    {
        switch (instructionType)
        {
            case InstructionType.None:
                return "Choose an instruction";
            case InstructionType.SetColors:
                return "Set Colors";
            default:
                return "Unknown instruction";
        }
    }
    
    public static string GetDescription(this InstructionType instructionType)
    {
        switch (instructionType)
        {
            case InstructionType.None:
                return "This instruction does nothing, please change the instruction type";
            case InstructionType.SetColors:
                return "Sets the background and light colors of the map. You can choose any number of colors, and you can choose to smoothly transition the background color";
            default:
                return "Unknown instruction";
        
        }   
    }
}