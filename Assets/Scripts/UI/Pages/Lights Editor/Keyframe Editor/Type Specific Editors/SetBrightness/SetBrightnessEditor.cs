using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetBrightnessEditor : InstructionSpecificKeyframeEditor
{
    public SetBrightnessEditor instance;
    public Slider brightnessSlider;
    public TMP_InputField brightnessMultiplierInputField;
    
    
    public SetBrightnessEditor()
    {
        instance = this;
        EditorInstructionType = InstructionType.SetBrightness;
    }
    
    public override void UpdateEditor()
    {
        SetBrightnessUI(KeyframeEditor.instance.keyframe.instruction.brightnessMultiplier);
    }

    private void SetBrightnessUI(float brightnessMultiplier)
    {
        brightnessMultiplierInputField.text = brightnessMultiplier.ToString("0.00") + "x";
        brightnessSlider.value = Mathf.InverseLerp(0.5f, 4f, brightnessMultiplier);
    }

    public void OnMultiplierInputFieldChanged()
    {
        string inp = brightnessMultiplierInputField.text;
        
        if (inp.EndsWith("x"))
        {
            inp = inp.Remove(inp.Length - 1);
        }
        
        if (float.TryParse(inp, out float brightness))
        {
            if (brightness < 0.5f)
            {
                brightness = 0.5f;
            } else if (brightness > 4f)
            {
                brightness = 4f;
            }
            KeyframeEditor.instance.keyframe.instruction.brightnessMultiplier = brightness;
            KeyframeEditor.instance.OnKeyframeChanged();
        }
        else
        {
            brightnessMultiplierInputField.text = KeyframeEditor.instance.keyframe.instruction.brightnessMultiplier.ToString("0.00") + "x";
        }
    }

    public void OnBrightnessSliderChanged()
    {
        float brightness = Mathf.Lerp(0.5f, 4f, brightnessSlider.value);
        KeyframeEditor.instance.keyframe.instruction.brightnessMultiplier = brightness;
        KeyframeEditor.instance.OnKeyframeChanged();
    }
    
    
}
