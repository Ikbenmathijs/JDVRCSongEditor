using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectDancerImageButton : MonoBehaviour
{
    public int dancerIndex;
    public Image dancerImage;
    public TextMeshProUGUI selectionButtonText;
    public SelectDancerImagesPage selectDancerImagesPage;
    
    public void SelectDancerImage()
    {
        selectDancerImagesPage.SelectDancerImageButtonPressed(dancerIndex);
    }
}
