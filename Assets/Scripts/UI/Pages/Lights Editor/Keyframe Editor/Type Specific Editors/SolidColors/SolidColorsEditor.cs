using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SolidColorsEditor : InstructionSpecificKeyframeEditor
{
    public static SolidColorsEditor instance;
    public Toggle solidColorsOnBeatToggle;
    public Toggle solidColorsOnIntervalToggle;
    
    public SolidColorsEditor()
    {
        instance = this;
        EditorInstructionType = InstructionType.SolidColors;
    }
    
    public override void UpdateEditor()
    {
        solidColorsOnBeatToggle.isOn = KeyframeEditor.instance.keyframe.instruction.solidColorOnBeat;
        solidColorsOnIntervalToggle.isOn = KeyframeEditor.instance.keyframe.instruction.solidColorsOnInterval;
    }
    
    
    public void OnSolidColorsOnBeatToggleChanged()
    {
        KeyframeEditor.instance.keyframe.instruction.solidColorOnBeat = solidColorsOnBeatToggle.isOn;
        KeyframeEditor.instance.OnKeyframeChanged();
    }
    
    public void OnSolidColorsOnIntervalToggleChanged()
    {
        KeyframeEditor.instance.keyframe.instruction.solidColorsOnInterval = solidColorsOnIntervalToggle.isOn;
        KeyframeEditor.instance.OnKeyframeChanged();
    }
}
