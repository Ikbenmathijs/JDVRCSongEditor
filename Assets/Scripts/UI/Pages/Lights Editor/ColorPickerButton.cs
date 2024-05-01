using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xenia.ColorPicker;

public class ColorPickerButton : MonoBehaviour
{
    public ColorPicker colorPicker;


    public void OnColorPickerButtonClicked()
    {
        colorPicker.Open();
        colorPicker.transform.Find("Parts").localPosition = Vector3.zero;
    }
}
