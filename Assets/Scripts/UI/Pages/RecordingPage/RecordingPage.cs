using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RecordingPage : Page
{
    [SerializeField] private bool reRecordingPage;
    [SerializeField] private Popup popup;
    [SerializeField] private Transform selectRecordingButtonsParent;

    private void Start()
    {
        PageName = "Select recording(s)";
    }
    
    public RecordingImportResult? SelectRecording(int recordingIndex)
    {
        RecordingImportResult result;
        
        
        try
        {
            result = RecordingImporter.ImportRecording(recordingIndex);
        }
        catch (Exception e)
        {
            SetNextPageAvailable(AllRecordingsAreImported());
            popup.ShowPopup("Failed to import recording: " + e.Message);
            throw;
        }
        
        SetNextPageAvailable(AllRecordingsAreImported());
        
        return result;
    }

    public override void InitializePage()
    {
        
        for (int i = 0; i < selectRecordingButtonsParent.childCount; i++)
        {
            selectRecordingButtonsParent.GetChild(i).gameObject.SetActive(i < getDancerAmount());
        }
        SetNextPageAvailable(AllRecordingsAreImported());
    }
    
    private bool AllRecordingsAreImported()
    {
        for (int i = 0; i < getDancerAmount(); i++)
        {
            if (!SongData.recordingsAreImported[i])
            {
                return false;
            }
        }
        return true;
    }


    private int getDancerAmount()
    {
        return !reRecordingPage ? SongData.dancerAmount : int.Parse(ReRecordingsGlobals.rerecodingSong.amountOfDancers);
    }
}
