using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeBackgroundColorEditor : MonoBehaviour
{
    public static FadeBackgroundColorEditor instance;
    public Slider fadeSpeedSlider;
    public TMP_InputField fadeSpeedInputField;


    public FadeBackgroundColorEditor()
    {
        instance = this;
    }

    public void SetFadeSpeed(float fadeSpeed)
    {
        float sliderValue = Mathf.InverseLerp(0.05f, 2f, fadeSpeed);
        if (sliderValue > 1f)
        {
            sliderValue = 1f;
        } else if (sliderValue < 0f)
        {
            sliderValue = 0f;
        }
        fadeSpeedSlider.value = sliderValue;
        
        fadeSpeedInputField.text = fadeSpeed.ToString("0.00") + "x";
        
        KeyframeEditor.instance.keyframe.backgroundColorLerpSpeed = fadeSpeed;
    }

    public void OnFadeSliderChanged()
    {
        float fadeSpeed = Mathf.Lerp(0.05f, 2f, fadeSpeedSlider.value);
        SetFadeSpeed(fadeSpeed);
        
    }
    
    
    public void OnFadeSpeedInputFieldEndEdit()
    {
        string inp = fadeSpeedInputField.text;
        
        if (inp.EndsWith("x"))
        {
            inp = inp.Remove(inp.Length - 1);
        }
        
        if (float.TryParse(inp, out float fadeSpeed))
        {
            if (fadeSpeed < 0.05f)
            {
                fadeSpeed = 0.05f;
            } else if (fadeSpeed > 2f)
            {
                fadeSpeed = 2f;
            }
            SetFadeSpeed(fadeSpeed);
            SetColorsEditor.instance.SetBackgroundColorFadeSpeed(fadeSpeed);
        }
        else
        {
            fadeSpeedInputField.text = KeyframeEditor.instance.keyframe.backgroundColorLerpSpeed.ToString("0.00") + "x";
        }
    }
}
