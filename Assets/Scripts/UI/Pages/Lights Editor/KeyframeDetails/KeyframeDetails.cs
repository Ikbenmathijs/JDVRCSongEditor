using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyframeDetails : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI keyframeText;
    public GameObject colorIndicatorPrefab;
    public Transform colorIndicatorParent;


    public void SetKeyframeDetails(Keyframe keyframe)
    {
        ResetKeyframeDetails();
        if (keyframe.instructionType == InstructionType.SetColors)
        {
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
