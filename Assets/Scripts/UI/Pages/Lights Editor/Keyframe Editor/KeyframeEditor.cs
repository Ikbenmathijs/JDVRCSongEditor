using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyframeEditor : MonoBehaviour
{
    public static KeyframeEditor instance;
    public Animator keyframeEditorPopupAnimator;
    public InstructionSpecificKeyframeEditor currentEditor;
    public InstructionSpecificKeyframeEditor[] editors;
    public Keyframe keyframe;
    public TextMeshProUGUI keyframeNameText;
    public TextMeshProUGUI keyframeTimeText;



    private InstructionSpecificKeyframeEditor previousEditor;
    private Keyframe previousKeyframe;
    
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
        UpdateEditor(forceInitializeEditor: true);
    }
    
    

    public void UpdateEditor(bool forceInitializeEditor = false)
    {
        keyframeNameText.text = keyframe.instructionType.ToFriendlyString();
        keyframeTimeText.text = Util.TimeToString(keyframe.time);
    
        CloseAllEditors();

        if (keyframe == null)
        {
            CloseKeyframeEditor();
            return;
        }
        currentEditor = GetEditorByInstructionType(keyframe.instructionType);
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
                
                keyframe = null;
                UpdateEditor();
                
                SelectKeyframePopup.instance.UpdateKeyframesList();

                v.keyframeMarker.GetComponent<KeyframeMenuObject>().holdingKeyframes.Remove(v);
                
                break;
            }
        }
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
