using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveKeyframeMenu : MonoBehaviour
{
    public static SaveKeyframeMenu instance;
    public Image colorPreview;
    public TMP_InputField keyframeNameInputField;
    public Button saveKeyframeButton;
    
    public SaveKeyframeMenu()
    {
        instance = this;
    }

    public void SaveKeyframeButtonPressed()
    {
        KeyframeEditor.instance.keyframe.instruction.SaveInstruction(keyframeNameInputField.text, colorPreview.color);
        EditorUiOrSaveKeyframeSwitcher.instance.SwitchToEditorUi();
    }
    

    public void InitializeSaveKeyframeMenu()
    {
        if (KeyframeEditor.instance.keyframe.instruction.instructionType == InstructionType.SetColors && KeyframeEditor.instance.keyframe.instruction.colors.Count > 0)
        {
            colorPreview.color = KeyframeEditor.instance.keyframe.instruction.colors[0];
        }
        else
        {
            colorPreview.color = Color.black;
        }

        if (!KeyframeEditor.instance.keyframe.instruction.savedInstruction)
        {
            keyframeNameInputField.text = "";
            saveKeyframeButton.interactable = false;
        }
        else
        {
            keyframeNameInputField.text = KeyframeEditor.instance.keyframe.instruction.savedInstructionName;
            colorPreview.color = KeyframeEditor.instance.keyframe.instruction.savedInstructionColor;
            saveKeyframeButton.interactable = false;
        }
    }

    public void OnKeyframeNameInputFieldTextChanged()
    {
        saveKeyframeButton.interactable = keyframeNameInputField.text.Length > 0;
    }
}
