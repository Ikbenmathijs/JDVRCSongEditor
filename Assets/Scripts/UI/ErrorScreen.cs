using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ErrorScreen : MonoBehaviour
{
    public TextMeshProUGUI errorDetailsText;
    public GameObject errorScreen;
    public void SetErrorScreen(string errorDetails)
    {
        errorDetailsText.text = errorDetails;
        errorScreen.SetActive(true);
    }
}
