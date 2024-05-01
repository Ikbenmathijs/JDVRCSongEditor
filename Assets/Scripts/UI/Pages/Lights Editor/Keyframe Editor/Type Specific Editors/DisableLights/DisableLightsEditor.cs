using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableLightsEditor : InstructionSpecificKeyframeEditor
{
    public Toggle disableOnBeatToggle;
    public Toggle disableOnIntervalToggle;
    
    public DisableLightsEditor()
    {
        EditorInstructionType = InstructionType.Disable;
    }
    
    public override void UpdateEditor()
    {
        disableOnBeatToggle.isOn = KeyframeEditor.instance.keyframe.instruction.disableBeat;
        disableOnIntervalToggle.isOn = KeyframeEditor.instance.keyframe.instruction.disableInterval;
    }
    
    
    public void disableColorsOnBeatToggleChanged()
    {
        KeyframeEditor.instance.keyframe.instruction.disableBeat = disableOnBeatToggle.isOn;
        KeyframeEditor.instance.OnKeyframeChanged();
    }
    
    public void disableColorsOnIntervalToggleChanged()
    {
        KeyframeEditor.instance.keyframe.instruction.disableInterval = disableOnIntervalToggle.isOn;
        KeyframeEditor.instance.OnKeyframeChanged();
    }
}
