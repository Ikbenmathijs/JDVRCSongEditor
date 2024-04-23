using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionTypeButton : MonoBehaviour
{
    public TextMeshProUGUI instructionTypeText;
    public InstructionType instructionType;

    public bool isSavedInstructionButton;
    public Instruction savedInstruction;
    
    
    
    public void SetInstructionType(InstructionType instructionType)
    {
        this.instructionType = instructionType;
        instructionTypeText.text = instructionType.ToFriendlyString();
        isSavedInstructionButton = false;
    }

    public void SetSavedInstruction(Instruction instruction)
    {
        isSavedInstructionButton = true;
        savedInstruction = instruction;
        GetComponent<Image>().color = instruction.savedInstructionColor;
        instructionTypeText.text = instruction.savedInstructionName;
    }
    
    
    
    public void OnInstructionTypeButtonClicked()
    {
        if (isSavedInstructionButton)
        {
            InstructionSelectionEditor.instance.OnSavedInstructionSelected(savedInstruction);
        }
        else
        {
            InstructionSelectionEditor.instance.OnInstructionTypeSelected(instructionType);
        }
    }

    
    
    
}
