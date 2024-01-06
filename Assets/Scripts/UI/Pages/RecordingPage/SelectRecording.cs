using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectRecording : MonoBehaviour
{
    public int recordingIndex;
    public RecordingPage recordingPage;
    [SerializeField] private TextMeshProUGUI recordingPathText;
    
    public void Select()
    {
        RecordingImportResult? result = recordingPage.SelectRecording(recordingIndex);

        if (result != null)
        {
            recordingPathText.text = $"{result.Value.path} ({result.Value.totalFrames} Frames)";
        }
        else
        {
            recordingPathText.text = "Failed to import recording";
        }
    }

}
