using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ColorIndicator : MonoBehaviour
{
    public Image borderImage;
    public Image fillerImage;
    public Sprite backgroundColorSprite;
    public Sprite nonBackgroundColorSprite;
    
    
    public void SetIsBackgroundColor(bool isBackgroundColor)
    {
        borderImage.sprite = isBackgroundColor ? backgroundColorSprite : nonBackgroundColorSprite;
    }
    
    public void SetColor(Color color)
    {
        fillerImage.color = color;
    }

}
