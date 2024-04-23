using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyframeEditor : MonoBehaviour
{
    public static KeyframeEditor instance;
    public Animator keyframeEditorPopupAnimator;
    public InstructionSpecificKeyframeEditor currentEditor;
    public InstructionSpecificKeyframeEditor[] editors;
    public Keyframe keyframe;
    public TextMeshProUGUI keyframeNameText;
    public TextMeshProUGUI keyframeTimeText;
    public Color defaultTimeIndicatorBackground;
    public Image timeIndicatorBackground;



    private InstructionSpecificKeyframeEditor previousEditor;
    private Keyframe previousKeyframe;
    
    public KeyframeEditor()
    {
        instance = this;
    }
    
    public void OnKeyframeChanged()
    {
        LightsPreviewInterpreter.instance.RefreshExecutedKeyframes();
        UpdateEditor();
        KeyframesManager.instance.UpdateKeyframeMarkers();
    }
    
    public void OpenKeyframeEditor(Keyframe keyframe)
    {
        keyframeEditorPopupAnimator.SetBool("Open", true);
        EditorUiOrSaveKeyframeSwitcher.instance.SwitchToEditorUi();
        SetKeyframe(keyframe);
        
    }

    public void CloseKeyframeEditor()
    {
        keyframeEditorPopupAnimator.SetBool("Open", false);
    }

    public void SetKeyframe(Keyframe keyframe)
    {
        this.keyframe = keyframe;
        UpdateEditor(forceInitializeEditor: true);
    }
    
    

    public void UpdateEditor(bool forceInitializeEditor = false)
    {
        if (keyframe == null)
        {
            CloseKeyframeEditor();
            return;
        }

        if (keyframe.instruction.savedInstruction)
        {
            keyframeNameText.text = keyframe.instruction.savedInstructionName;
            timeIndicatorBackground.color = keyframe.instruction.savedInstructionColor;
        }
        else
        {
            keyframeNameText.text = keyframe.instruction.instructionType.ToFriendlyString();
            timeIndicatorBackground.color = defaultTimeIndicatorBackground;
        }
        keyframeTimeText.text = Util.TimeToString(keyframe.time);
    
        CloseAllEditors();

        
        currentEditor = GetEditorByInstructionType(keyframe.instruction.instructionType);
        currentEditor.gameObject.SetActive(true);


        if (currentEditor != previousEditor || keyframe != previousKeyframe || forceInitializeEditor)
        {
            currentEditor.InitializeEditor();
        }
        
        currentEditor.UpdateEditor();
        
        SelectKeyframePopup.instance.UpdateKeyframesList();
        
        
        previousEditor = currentEditor;
        previousKeyframe = keyframe;
    }

    public void DeleteKeyframeButtonPressed()
    {
        foreach (Keyframe v in KeyframesManager.instance.keyframes)
        {
            if (v == keyframe)
            {
                KeyframesManager.instance.keyframes.Remove(v);
                KeyframesManager.instance.UpdateKeyframeMarkers();
                LightsPreviewInterpreter.instance.RefreshExecutedKeyframes();
                
                keyframe = null;
                UpdateEditor();
                
                
                SelectKeyframePopup.instance.UpdateKeyframesList();

                
                break;
            }
        }
    }
    
    
    public void ChangeInstructionTypeButtonPressed()
    {
        keyframe.instruction = new Instruction(InstructionType.None);
        UpdateEditor();
    }
    
    private void CloseAllEditors()
    {
        foreach (InstructionSpecificKeyframeEditor editor in editors)
        {
            editor.gameObject.SetActive(false);
        }
    }

    private InstructionSpecificKeyframeEditor GetEditorByInstructionType(InstructionType instructionType)
    {
        for (int i = 0; i < editors.Length; i++)
        {
            if (editors[i].EditorInstructionType == instructionType)
            {
                return editors[i];
            }
        }

        return null;
    }
}
