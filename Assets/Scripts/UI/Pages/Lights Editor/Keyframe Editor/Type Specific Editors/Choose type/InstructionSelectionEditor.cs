using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class InstructionSelectionEditor : InstructionSpecificKeyframeEditor
{
    public static InstructionSelectionEditor instance;
    public GameObject instructionTypeButtonPrefab;
    public Transform instructionTypeButtonParent;
    public GameObject instructionSelectionScreen;
    public InstructionConfirmationScreen instructionConfirmationScreen;
    public bool instructionConfirmationScreenActive;

    public InstructionSelectionEditor()
    {
        instance = this;
        EditorInstructionType = InstructionType.None;
    }

    public override void UpdateEditor()
    {
        instructionSelectionScreen.SetActive(!instructionConfirmationScreenActive);
        instructionConfirmationScreen.gameObject.SetActive(instructionConfirmationScreenActive);
        if (!instructionConfirmationScreenActive)
        {
            for (int i = 0; i < instructionTypeButtonParent.childCount; i++)
            {
                Destroy(instructionTypeButtonParent.GetChild(i).gameObject);
            }
    
    
            var types = Enum.GetValues(typeof(InstructionType)).Cast<InstructionType>();

            foreach (InstructionType type in types)
            {
                if (type == InstructionType.None) continue;
                InstructionTypeButton button = Instantiate(instructionTypeButtonPrefab, instructionTypeButtonParent).GetComponent<InstructionTypeButton>();
                button.SetInstructionType(type);
            }

            foreach (Instruction instruction in KeyframesManager.instance.savedInstructions)
            {
                InstructionTypeButton button = Instantiate(instructionTypeButtonPrefab, instructionTypeButtonParent).GetComponent<InstructionTypeButton>();
                button.SetSavedInstruction(instruction);
            }
        }
    }
    
    public void OnInstructionTypeSelected(InstructionType instructionType)
    {
        instructionSelectionScreen.SetActive(false);
        instructionConfirmationScreen.gameObject.SetActive(true);
        instructionConfirmationScreen.InitializeInstructionConfirmationScreenWithInstructionType(instructionType);
    }
    
    public void OnSavedInstructionSelected(Instruction instruction)
    {
        instructionSelectionScreen.SetActive(false);
        instructionConfirmationScreen.gameObject.SetActive(true);
        instructionConfirmationScreen.InitializeInstructionConfirmationScreenWithSavedInstruction(instruction);
    }
}
