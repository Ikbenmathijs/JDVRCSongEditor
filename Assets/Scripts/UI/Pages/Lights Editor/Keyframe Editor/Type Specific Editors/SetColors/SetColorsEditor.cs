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
        changeBackgroundColorEditor.SetActive(KeyframeEditor.instance.keyframe.instruction.changeBackgroundColor);
        changeBackgroundColorEditorToggle.isOn = KeyframeEditor.instance.keyframe.instruction.changeBackgroundColor;
        fadeBackgroundColorEditor.SetActive(KeyframeEditor.instance.keyframe.instruction.fadeBackgroundColor);
        fadeBackgroundColorToggle.isOn = KeyframeEditor.instance.keyframe.instruction.fadeBackgroundColor;
        FadeBackgroundColorEditor.instance.SetFadeSpeed(KeyframeEditor.instance.keyframe.instruction.backgroundColorFadeSpeed);
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
        List<Color> primaryColors = keyframe.instruction.colors;

        for (int i = 0; i < primaryColors.Count; i++)
        {
            var color = primaryColors[i];
            GameObject instance = Instantiate(colorButtonPrefab, colorButtonParent);
            ColorButton colorButton = instance.GetComponent<ColorButton>();
            colorButton.index = i;
            colorButton.SetColor(color);
            
            if (KeyframeEditor.instance.keyframe.instruction.usedInInitialKeyframe && i == 0)
            {
                colorButton.removeColorButton.SetActive(false);
            }
            
            plusButton.SetAsLastSibling();
        }

        backgroundColorButton.SetColor(keyframe.instruction.backgroundColor);
        
        noPrimaryColorsText.SetActive(primaryColors.Count == 0);

        colorSelectorPage.SetActive(selectingColor);
        colorsEditorPage.SetActive(!selectingColor);
        
        changeBackgroundColorEditorToggle.interactable = !KeyframeEditor.instance.keyframe.instruction.usedInInitialKeyframe;
    }
    
    public void OnAddColorButtonPressed()
    {
        KeyframeEditor.instance.keyframe.instruction.colors.Add(Color.white);
        KeyframeEditor.instance.OnKeyframeChanged();
    }

    public void ChangeColor(bool isBackgroundColor, int index = 0)
    {
        changingColorIndex = index;
        changingBackgroundColor = isBackgroundColor;
        selectingColor = true;
        Keyframe keyframe = KeyframeEditor.instance.keyframe;
        colorPicker.CurrentColor = isBackgroundColor ? keyframe.instruction.backgroundColor : keyframe.instruction.colors[index];
        
        KeyframeEditor.instance.UpdateEditor();
    }

    public void OnSelectColorButtonPressed()
    {
        selectingColor = false;
        Keyframe keyframe = KeyframeEditor.instance.keyframe;
        if (changingBackgroundColor)
        {
            keyframe.instruction.backgroundColor = colorPicker.CurrentColor;
        }
        else
        {
            keyframe.instruction.colors[changingColorIndex] = colorPicker.CurrentColor;
        }
        KeyframeEditor.instance.OnKeyframeChanged();
    }

    
    public void OnChangeBackgroundColorEditorToggled()
    {
        changeBackgroundColorEditor.SetActive(changeBackgroundColorEditorToggle.isOn);
        KeyframeEditor.instance.keyframe.instruction.changeBackgroundColor = changeBackgroundColorEditorToggle.isOn;
    }

    
    public void OnFadeBackgroundColorEditorToggled()
    {
        fadeBackgroundColorEditor.SetActive(fadeBackgroundColorToggle.isOn);
        KeyframeEditor.instance.keyframe.instruction.fadeBackgroundColor = fadeBackgroundColorToggle.isOn;
    }


    public void SetBackgroundColorFadeSpeed(float fadeSpeed)
    {
        KeyframeEditor.instance.keyframe.instruction.backgroundColorFadeSpeed = fadeSpeed;
        KeyframeEditor.instance.OnKeyframeChanged();
    }


}
