using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldMoveEditor : InstructionSpecificKeyframeEditor
{
    public Button[] goldMoveButtons;
    public Button allGoldMoveButton;
    public GameObject[] multipleDancerObjects;
    public GameObject[] singleDancerObjects;



    public override void UpdateEditor()
    {
        foreach (GameObject obj in multipleDancerObjects)
        {
            obj.SetActive(SongData.dancerAmount > 1);
        }
        foreach (GameObject obj in singleDancerObjects)
        {
            obj.SetActive(SongData.dancerAmount == 1);
        }
         SetAllGoldMoveButtonsToUnselected();
         Instruction instruction = KeyframeEditor.instance.keyframe.instruction;
         if (instruction.goldMoveType == GoldMoveType.All)
         {
             allGoldMoveButton.GetComponent<Animator>().SetBool("UISelected", true);
         } else if (instruction.goldMoveType == GoldMoveType.Dancer0)
         {
             goldMoveButtons[0].GetComponent<Animator>().SetBool("UISelected", true);
         } else if (instruction.goldMoveType == GoldMoveType.Dancer1)
         {
             goldMoveButtons[1].GetComponent<Animator>().SetBool("UISelected", true);
         } else if (instruction.goldMoveType == GoldMoveType.Dancer2)
         {
             goldMoveButtons[2].GetComponent<Animator>().SetBool("UISelected", true);
         } else if (instruction.goldMoveType == GoldMoveType.Dancer3)
         {
             goldMoveButtons[3].GetComponent<Animator>().SetBool("UISelected", true);
         }
    }


    public void OnGoldMoveButtonPressed(int dancerIndex)
    {
        Instruction instruction = KeyframeEditor.instance.keyframe.instruction;
        if (dancerIndex == 0)
        {
            instruction.goldMoveType = GoldMoveType.Dancer0;
        } else if (dancerIndex == 1)
        {
            instruction.goldMoveType = GoldMoveType.Dancer1;
        } else if (dancerIndex == 2)
        {
            instruction.goldMoveType = GoldMoveType.Dancer2;
        } else if (dancerIndex == 3)
        {
            instruction.goldMoveType = GoldMoveType.Dancer3;
        }
        KeyframeEditor.instance.OnKeyframeChanged();
    }
    
    public void OnAllGoldMoveButtonPressed()
    {
        KeyframeEditor.instance.keyframe.instruction.goldMoveType = GoldMoveType.All;
        KeyframeEditor.instance.OnKeyframeChanged();
    }


    public GoldMoveEditor()
    {
        EditorInstructionType = InstructionType.GoldMove;
    }

    private void SetAllGoldMoveButtonsToUnselected()
    {
        allGoldMoveButton.GetComponent<Animator>().SetBool("UISelected", false);
        for (int i = 0; i < goldMoveButtons.Length; i++)
        {
            goldMoveButtons[i].GetComponent<Animator>().SetBool("UISelected", false);
        }
    }
}
