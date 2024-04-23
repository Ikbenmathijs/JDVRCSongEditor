using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InstructionConfirmationScreen : MonoBehaviour
{
    public TextMeshProUGUI instructionName;
    public TextMeshProUGUI instructionTypeDescription;
    public Image instructionNameBackgroundColor;
    public Color defaultInstructionNameBackgroundColor;
    public InstructionType instructionType;
    private Instruction savedInstructionToSelect;
    private bool selectingSavedInstruction;

    public void InitializeInstructionConfirmationScreenWithInstructionType(InstructionType instructionType)
    {
        this.instructionType = instructionType;
        instructionTypeDescription.text = instructionType.GetDescription();
        instructionName.text = instructionType.ToFriendlyString();
        instructionNameBackgroundColor.color = defaultInstructionNameBackgroundColor;
        selectingSavedInstruction = false;
    }
    
    public void InitializeInstructionConfirmationScreenWithSavedInstruction(Instruction instruction)
    {
        instructionType = instruction.instructionType;
        instructionTypeDescription.text = "Saved keyframe\nKeyframe type: " + instruction.instructionType.ToFriendlyString() + "\n Keyframe type description: " + instruction.instructionType.GetDescription();
        instructionName.text = instruction.savedInstructionName;
        instructionNameBackgroundColor.color = instruction.savedInstructionColor;
        selectingSavedInstruction = true;
        savedInstructionToSelect = instruction;
    }
    
    public void OnConfirmButtonClicked()
    {
        if (!selectingSavedInstruction)
        {
            KeyframeEditor.instance.keyframe.instruction = new Instruction(instructionType);
        }
        else
        {
            KeyframeEditor.instance.keyframe.instruction = savedInstructionToSelect;
        }
        InstructionSelectionEditor.instance.instructionConfirmationScreenActive = false; 
        KeyframeEditor.instance.OnKeyframeChanged();
    }

    public void OnGoBackButtonClicked()
    {
        InstructionSelectionEditor.instance.instructionConfirmationScreenActive = false;
        KeyframeEditor.instance.UpdateEditor();
    }
    
    
}
