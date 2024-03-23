using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xenia.ColorPicker;

public class ColorPickerButton : MonoBehaviour
{
    public ColorPicker colorPicker;
    private bool _open;
    
    public void OnColorPickerButtonClicked()
    {
        if (_open)
        {
            colorPicker.Close();
        }
        else
        {
            colorPicker.Open();
        }
    }
}
