using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xenia.ColorPicker;
using UnityEngine.UI;

public class SetFillEditor : InstructionSpecificKeyframeEditor
{
    public static SetFillEditor instance;
    public ColorPicker colorPicker;
    public Image currentColorPreview;
    public Image newColorPreview;
    
    public SetFillEditor()
    {
        instance = this;
        EditorInstructionType = InstructionType.SetFill;
    }


    public override void UpdateEditor()
    {
        currentColorPreview.color = KeyframeEditor.instance.keyframe.instruction.fillColor;
    }

    private void Update()
    {
        newColorPreview.color = colorPicker.CurrentColor;
    }

    public void OnChangeColorButtonPressed()
    {
        KeyframeEditor.instance.keyframe.instruction.fillColor = colorPicker.CurrentColor;
        KeyframeEditor.instance.OnKeyframeChanged();
    }
    
}
