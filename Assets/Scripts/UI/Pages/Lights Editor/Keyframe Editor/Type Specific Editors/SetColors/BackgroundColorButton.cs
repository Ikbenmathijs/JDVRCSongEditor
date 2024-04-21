using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackgroundColorButton : MonoBehaviour
{
    public Image colorFill;
    public void SetColor(Color color)
    {
        colorFill.color = color;
    }
    
    
    public void OnColorButtonPressed()
    {
        
        SetColorsEditor.instance.ChangeColor(isBackgroundColor: true);
    }
    
    
}
