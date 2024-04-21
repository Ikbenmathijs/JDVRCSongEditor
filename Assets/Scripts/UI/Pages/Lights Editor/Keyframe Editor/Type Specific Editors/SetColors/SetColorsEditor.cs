using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xenia.ColorPicker;

public class SetColorsEditor : InstructionSpecificKeyframeEditor
{
    
    public static SetColorsEditor instance;
    public GameObject colorButtonPrefab;
    public Transform colorButtonParent;
    public Transform plusButton;
    public GameObject noPrimaryColorsText;
    public GameObject colorSelectorPage;
    public GameObject colorsEditorPage;
    public ColorPicker colorPicker;
    public BackgroundColorButton backgroundColorButton;
    private bool selectingColor;
    private int changingColorIndex;
    private bool changingBackgroundColor;
    public GameObject changeBackgroundColorEditor;
    public Toggle changeBackgroundColorEditorToggle;
    public GameObject fadeBackgroundColorEditor;
    public Toggle fadeBackgroundColorToggle;
    
    
    
    
    
    
    public SetColorsEditor()
    {
        EditorInstructionType = InstructionType.SetColors;
        instance = this;
    }
    
    public override void InitializeEditor()
    {
        selectingColor = false;
        changeBackgroundColorEditor.SetActive(KeyframeEditor.instance.keyframe.changeBackgroundColor);
        changeBackgroundColorEditorToggle.isOn = KeyframeEditor.instance.keyframe.changeBackgroundColor;
        fadeBackgroundColorEditor.SetActive(KeyframeEditor.instance.keyframe.fadeBackgroundColor);
        fadeBackgroundColorToggle.isOn = KeyframeEditor.instance.keyframe.fadeBackgroundColor;
        FadeBackgroundColorEditor.instance.SetFadeSpeed(KeyframeEditor.instance.keyframe.backgroundColorLerpSpeed);
    }

    public override void UpdateEditor()
    {
        for (int i = 0; i < colorButtonParent.childCount; i++)
        {
            if (colorButtonParent.GetChild(i).GetComponent<ColorButton>() != null)
            {
                Destroy(colorButtonParent.GetChild(i).gameObject);
            }
        }
        Keyframe keyframe = KeyframeEditor.instance.keyframe;
        List<Color> primaryColors = keyframe.colors;

        for (int i = 0; i < primaryColors.Count; i++)
        {
            var color = primaryColors[i];
            GameObject instance = Instantiate(colorButtonPrefab, colorButtonParent);
            ColorButton colorButton = instance.GetComponent<ColorButton>();
            colorButton.index = i;
            colorButton.SetColor(color);
            plusButton.SetAsLastSibling();
        }

        backgroundColorButton.SetColor(keyframe.backgroundColor);
        
        noPrimaryColorsText.SetActive(primaryColors.Count == 0);

        colorSelectorPage.SetActive(selectingColor);
        colorsEditorPage.SetActive(!selectingColor);
    }
    
    public void OnAddColorButtonPressed()
    {
        KeyframeEditor.instance.keyframe.colors.Add(Color.white);
        UpdateEditor();
    }

    public void ChangeColor(bool isBackgroundColor, int index = 0)
    {
        changingColorIndex = index;
        changingBackgroundColor = isBackgroundColor;
        selectingColor = true;
        Keyframe keyframe = KeyframeEditor.instance.keyframe;
        colorPicker.CurrentColor = isBackgroundColor ? keyframe.backgroundColor : keyframe.colors[index];
        
        KeyframeEditor.instance.UpdateEditor();
    }

    public void OnSelectColorButtonPressed()
    {
        selectingColor = false;
        Keyframe keyframe = KeyframeEditor.instance.keyframe;
        if (changingBackgroundColor)
        {
            keyframe.backgroundColor = colorPicker.CurrentColor;
        }
        else
        {
            keyframe.colors[changingColorIndex] = colorPicker.CurrentColor;
        }
        
        KeyframeEditor.instance.UpdateEditor();
    }

    
    public void OnChangeBackgroundColorEditorToggled()
    {
        changeBackgroundColorEditor.SetActive(changeBackgroundColorEditorToggle.isOn);
        KeyframeEditor.instance.keyframe.changeBackgroundColor = changeBackgroundColorEditorToggle.isOn;
    }

    
    public void OnFadeBackgroundColorEditorToggled()
    {
        fadeBackgroundColorEditor.SetActive(fadeBackgroundColorToggle.isOn);
        KeyframeEditor.instance.keyframe.fadeBackgroundColor = fadeBackgroundColorToggle.isOn;
    }


    public void SetBackgroundColorFadeSpeed(float fadeSpeed)
    {
        KeyframeEditor.instance.keyframe.backgroundColorLerpSpeed = fadeSpeed;
        KeyframeEditor.instance.UpdateEditor();
    }


}
