using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xenia.ColorPicker;
using UnityEngine.UI;

public class ColorPreview : MonoBehaviour
{
    public ColorPicker colorPicker;
    public Image previewImage;


    private void Update()
    {
        previewImage.color = colorPicker.CurrentColor;
    }

    public void SetColor(Color color)
    {
        previewImage.color = color;
    }
}
