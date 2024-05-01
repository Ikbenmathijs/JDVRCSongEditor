using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;


public class Keyframe : IComparable<Keyframe>
{
    
    public float time;
    public GameObject keyframeMarker;
    public Instruction instruction;
    [CanBeNull] public GameObject keyframeIndicator; // is null if the indicator is tied to a keyframe marker exactly (because it's the first keyframe in a group)
    public bool initialKeyframe;
    
    public int CompareTo(Keyframe other)
    {
        if (other == null)
            return 1;
        
        return time.CompareTo(other.time);
    }

    public Keyframe(float time, Instruction instruction = null)
    {
        this.time = time;
        if (instruction != null)
        {
            this.instruction = instruction;
        }
        else
        {
            this.instruction = new Instruction(InstructionType.None);
        }
    }
}


public class Instruction
{
    public readonly InstructionType instructionType;

    public bool usedInInitialKeyframe = false;
    
    // instruction saving
    public bool savedInstruction = false;
    public string savedInstructionName;
    public Color savedInstructionColor;
    

    // SetColors
    public List<Color> colors = new List<Color>();
    public float fadeTime = 1f; // set if instructionType is LerpDefault or combination
    public bool changeBackgroundColor = false;
    public Color backgroundColor = Color.white; // set if instructionType is SetColors or LerpDefault or combination
    public bool fadeBackgroundColor = false;
    public float backgroundColorFadeSpeed = 1f;
    
    // SetBrightness
    public float brightnessMultiplier = 1f;
    
    // SetFill
    public Color fillColor = Color.white;
    
    // SolidColors
    public bool solidColorOnBeat = false;
    public bool solidColorsOnInterval = false;
    
    // Disable
    public bool disableBeat = false;
    public bool disableInterval = false;
    
    
    public Instruction(InstructionType instructionType)
    {
        this.instructionType = instructionType;
    }

    public void SaveInstruction(string name, Color color)
    {
        savedInstruction = true;
        savedInstructionName = name;
        savedInstructionColor = color;
        if (!KeyframesManager.instance.savedInstructions.Contains(this))
        {
            KeyframesManager.instance.savedInstructions.Add(this);
        }
    }
}


public enum InstructionType
{
    None,
    SetColors, // this translates to a combi of lerpdefault and setcolors in the 'language' to prevent confusion
    SetBrightness,
    SetFill,
    DisableFill,
    SolidColors,
    Disable,
}

public static class InstructionTypeExtensions
{
    public static bool IsAdvancedInstruction(this InstructionType instructionType)
    {
        switch (instructionType)
        {
            case InstructionType.SetColors:
                return false;
            case InstructionType.SetBrightness:
                return false;
            case InstructionType.SetFill:
                return true;
            case InstructionType.DisableFill:
                return true;
            case InstructionType.SolidColors:
                return true;
            case InstructionType.Disable:
                return true;
            default:
                return true;
        }
    }

    public static string ToFriendlyString(this InstructionType instructionType)
    {
        switch (instructionType)
        {
            case InstructionType.None:
                return "Choose an instruction";
            case InstructionType.SetColors:
                return "Set Colors";
            case InstructionType.SetBrightness:
                return "Set Brightness";
            case InstructionType.SetFill:
                return "Set Fill Color";
            case InstructionType.DisableFill:
                return "Disable Fill Color";
            case InstructionType.SolidColors:
                return "Solid Colors";
            case InstructionType.Disable:
                return "Disable Lights";
            default:
                return instructionType.ToString();
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
            case InstructionType.SetBrightness:
                return "Changes the brightness of the primary colors by a certain multiplier. For example, you could make the colors 2x as bright";
            case InstructionType.SetFill:
                return "Sets the fill color of the map. This color will be displayed on all tiles no matter the loudness of the music. You should use a Disable Fill Color instruction afterwards to disable the fill color again.";
            case InstructionType.DisableFill:
                return "Disables the fill color. This means the tiles will return back to normal and the fill color will no longer be displayed. You should use this after a Set Fill Color keyframe.";
            case InstructionType.SolidColors:
                return "When turned on, colors triggered by music will always go to the maximum brightness regardless of the volume of the music. So a very soft beat can produce the same color intensity as a very loud beat.";
            case InstructionType.Disable:
                return "Allows you to disable parts of the lights, or all of them so it only shows the background color.";
            default:
                return "Unknown instruction";
        }   
    }
}