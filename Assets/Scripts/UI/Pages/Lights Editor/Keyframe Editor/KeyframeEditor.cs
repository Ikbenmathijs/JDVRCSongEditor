using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyframeEditor : MonoBehaviour
{
    public static KeyframeEditor instance;
    public Animator keyframeEditorPopupAnimator;
    public InstructionSpecificKeyframeEditor currentEditor;
    public InstructionSpecificKeyframeEditor[] editors;
    public Keyframe keyframe;
    
    public KeyframeEditor()
    {
        instance = this;
    }
    
    public void OpenKeyframeEditor(Keyframe keyframe)
    {
        keyframeEditorPopupAnimator.SetBool("Open", true);
        SetKeyframe(keyframe);
        
    }

    public void CloseKeyframeEditor()
    {
        keyframeEditorPopupAnimator.SetBool("Open", false);
    }

    public void SetKeyframe(Keyframe keyframe)
    {
        this.keyframe = keyframe;
        UpdateEditor();
    }
    
    

    public void UpdateEditor()
    {
        CloseAllEditors();
        
        currentEditor = GetEditorByInstructionType(keyframe.instructionType);
        currentEditor.gameObject.SetActive(true);
        currentEditor.UpdateEditor();
    }
    
    
    public void ChangeInstructionTypeButtonPressed()
    {
        keyframe.instructionType = InstructionType.None;
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
