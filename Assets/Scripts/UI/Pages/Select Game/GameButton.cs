using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameButton : MonoBehaviour
{
    public string gameCodeName;
    public SelectGamePage selectGamePage;
    public Image gameImage;
    public TextMeshProUGUI gameNameText;
    
    public void SelectGame()
    {
        selectGamePage.SelectGame(gameCodeName);
    }
}
