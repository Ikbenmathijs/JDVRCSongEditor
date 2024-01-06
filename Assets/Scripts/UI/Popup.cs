using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Popup : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private Animator popupAnimator;
    
    
    public void ShowPopup(string text)
    {
        popupText.text = text;
        popupAnimator.SetBool("Shown", true);
    }

    public void ClosePopup()
    {
        popupAnimator.SetBool("Shown", false);
    }

    public void ClearPopupText()
    {
        popupText.text = "Placeholder Popup, if you see this then something broke :')";
    }
}
