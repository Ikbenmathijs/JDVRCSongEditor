using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewSongButtonPressed()
    {
        SceneManager.LoadScene("Main");
    }
    
    public void ConvertSongButtonPressed()
    {
        SceneManager.LoadScene("ConvertOldFormatToNew");
    }
    
    public void ExitButtonPressed()
    {
        Application.Quit();
    }
}
