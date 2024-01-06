using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class BasicInfoPage : Page
{
    public TMP_InputField songNameInputField;
    public TMP_InputField artistNameInputField;
    public TMP_InputField durationInputField;
    public Button[] amountOfDancersButtons;
    public Button[] difficultyButtons;
    private bool difficultySelected = false;
    private bool amountOfDancersSelected = false;
    
    private void Start()
    {
        PageName = "Song Information";
    }

    public void AmountOfDancersButtonPressed(int amount)
    {
        SongData.dancerAmount = amount;
        for (int i = 0; i < amountOfDancersButtons.Length; i++)
        {
            amountOfDancersButtons[i].GetComponent<Animator>().SetBool("UISelected", false);
        }
        amountOfDancersButtons[amount - 1].GetComponent<Animator>().SetBool("UISelected", true);
        amountOfDancersSelected = true;
        OnFieldEntered();
    }

    public void SelectDifficultyButtonPressed(int difficulty)
    {
        SongData.difficulty = difficulty;
        difficultySelected = true;
        for (int i = 0; i < difficultyButtons.Length; i++)
        {
            difficultyButtons[i].GetComponent<Animator>().SetBool("UISelected", false);
        }
        difficultyButtons[difficulty].GetComponent<Animator>().SetBool("UISelected", true);
        OnFieldEntered();
    }


    public void OnFieldEntered()
    {
        SetNextPageAvailable(songNameInputField.text.Length > 0 && artistNameInputField.text.Length > 0 && ValidateDurationInput(durationInputField.text) && difficultySelected && amountOfDancersSelected);
    }

    private bool ValidateDurationInput(string duration)
    {
        string numbers = "0123456789";
        string numbers05 = "012345";
        if (duration.Length > 5 || duration.Length < 4)
        {
            return false;
        }

        if (duration.Length == 4 && (!numbers.Contains(duration[0]) || !numbers05.Contains(duration[2]) || !numbers.Contains(duration[3])  || duration[1] != ':'))
        {
            return false;
        } else if (duration.Length == 5 && (!numbers.Contains(duration[0]) || !numbers.Contains(duration[1]) || !numbers05.Contains(duration[3]) || !numbers.Contains(duration[4]) || duration[2] != ':'))
        {
            return false;
        }
        return true;
    }
}
