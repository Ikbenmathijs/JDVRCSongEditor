using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class KeyframeDetails : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI instructionTypeText;
    public GameObject colorIndicatorPrefab;
    public Transform colorIndicatorParent;
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
        
        if (keyframe.instructionType == InstructionType.SetColors)
        {
            instructionTypeText.text = "Set Colors";
            if (keyframe.changeBackgroundColor)
            {
                GameObject instance = Instantiate(colorIndicatorPrefab, colorIndicatorParent);
                ColorIndicator colorIndicator = instance.GetComponent<ColorIndicator>();
                colorIndicator.SetIsBackgroundColor(true);
                colorIndicator.SetColor(keyframe.backgroundColor);
            }

            foreach (Color color in keyframe.colors)
            {
                GameObject instance = Instantiate(colorIndicatorPrefab, colorIndicatorParent);
                ColorIndicator colorIndicator = instance.GetComponent<ColorIndicator>();
                colorIndicator.SetIsBackgroundColor(false);
                colorIndicator.SetColor(color);
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
