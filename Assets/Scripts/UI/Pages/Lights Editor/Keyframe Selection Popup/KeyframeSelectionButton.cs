using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyframeDetails : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI instructionTypeText;
    public GameObject colorIndicatorPrefab;
    public Transform colorIndicatorParent;
    public Image timeIndicatorBackground;
    public Keyframe keyframe;
    



    public void OnClicked()
    {
        KeyframeEditor.instance.OpenKeyframeEditor(keyframe);
    }

    public void SetKeyframeDetails(Keyframe keyframe)
    {
        this.keyframe = keyframe;
        
        ResetKeyframeDetails();
        timeText.text = Util.TimeToString(keyframe.time);


        if (keyframe.instruction.savedInstruction)
        {
            instructionTypeText.text = keyframe.instruction.savedInstructionName;
            timeIndicatorBackground.color = keyframe.instruction.savedInstructionColor;
        }
        else
        {
            instructionTypeText.text = keyframe.instruction.instructionType.ToFriendlyString();
        }
        
        
        
        if (keyframe.instruction.instructionType == InstructionType.SetColors)
        {
            if (keyframe.instruction.changeBackgroundColor)
            {
                GameObject instance = Instantiate(colorIndicatorPrefab, colorIndicatorParent);
                ColorIndicator colorIndicator = instance.GetComponent<ColorIndicator>();
                colorIndicator.SetIsBackgroundColor(true);
                colorIndicator.SetColor(keyframe.instruction.backgroundColor);
            }


            if (keyframe.instruction.colors != null)
            {
                foreach (Color color in keyframe.instruction.colors)
                {
                    GameObject instance = Instantiate(colorIndicatorPrefab, colorIndicatorParent);
                    ColorIndicator colorIndicator = instance.GetComponent<ColorIndicator>();
                    colorIndicator.SetIsBackgroundColor(false);
                    colorIndicator.SetColor(color);
                }
            }
        }
    }


    private void ResetKeyframeDetails()
    {
        for (int i = 0; i < colorIndicatorParent.childCount; i++)
        {
            Destroy(colorIndicatorParent.GetChild(i).gameObject);
        }
    }
}
