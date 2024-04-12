using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstructionTypeButton : MonoBehaviour
{
    public TextMeshProUGUI instructionTypeText;
    public InstructionType instructionType;
    
    
    public void SetInstructionType(InstructionType instructionType)
    {
        this.instructionType = instructionType;
        instructionTypeText.text = instructionType.ToFriendlyString();
    }
    
    public void OnInstructionTypeButtonClicked()
    {
        InstructionSelectionEditor.instance.OnInstructionSelected(instructionType);
    }

    
    
    
}
