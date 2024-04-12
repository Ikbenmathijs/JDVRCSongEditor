using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstructionConfirmationScreen : MonoBehaviour
{
    public TextMeshProUGUI instructionTypeText;
    public TextMeshProUGUI instructionTypeDescription;
    public InstructionType instructionType;

    public void InitializeInstructionConfirmationScreen(InstructionType instructionType)
    {
        this.instructionType = instructionType;
        instructionTypeDescription.text = instructionType.GetDescription();
        instructionTypeText.text = instructionType.ToFriendlyString();
        
    }
    
    public void OnConfirmButtonClicked()
    {
        KeyframeEditor.instance.keyframe.instructionType = instructionType;
        KeyframeEditor.instance.UpdateEditor();
        InstructionSelectionEditor.instance.instructionConfirmationScreenActive = false;
    }

    public void OnGoBackButtonClicked()
    {
        InstructionSelectionEditor.instance.instructionConfirmationScreenActive = false;
        KeyframeEditor.instance.UpdateEditor();
    }
    
    
}
