using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using SFB;
using System.IO;

public static class RecordingImporter
{

    

    public static RecordingImportResult ImportRecording(int dancerIndex)
    {
        string[] files = StandaloneFileBrowser.OpenFilePanel("Select Recording File", "", "txt", false);

        if (files.Length != 1)
        {
            SongData.recordingsAreImported[dancerIndex] = false;
            throw new Exception("Please select 1 file containing the recording");
        }

        string path = files[0];

        string rawRecordingFile;
        try
        {
            var sr = new StreamReader(path);
            rawRecordingFile = sr.ReadToEnd();
        }
        catch (IOException e)
        {
            SongData.recordingsAreImported[dancerIndex] = false;
            throw new Exception($"Failed to read contents of file {path}.");
        }

        string[] rawRecordingFileLines = rawRecordingFile.Split('\n');
        string recordingLine = "";
        foreach (var line in rawRecordingFileLines)
        {
            if (line.Contains("2|"))
            {
                recordingLine = line;
            }
        }

        if (recordingLine == "")
        {
            SongData.recordingsAreImported[dancerIndex] = false;
            throw new Exception("No recording found in this file.");
        }

        recordingLine = recordingLine.Substring(recordingLine.IndexOf("2|", StringComparison.Ordinal));

        try
        {
            Debug.Log("Verifying recording...");
            VerifyRecording(recordingLine);
        }
        catch (Exception e)
        {
            SongData.recordingsAreImported[dancerIndex] = false;
            throw new Exception($"Invalid recording: {e.Message}");
        }

        Debug.Log("Valid Recording!");
        SongData.recordings[dancerIndex] = recordingLine;
        SongData.recordingsAreImported[dancerIndex] = true;
        return new RecordingImportResult { path = path, totalFrames = recordingLine.Split(':').Length - 1};
    }


    private static void VerifyRecording(string recording)
    {
        string[] splitRecording = recording.Split('~');
        // get header info
        string[] header = splitRecording[0].Split('|');
        
        
        var recordingVersion = int.Parse(header[0]);
        if (recordingVersion != 2)
        {
            throw new Exception($"Recording version {recordingVersion} not supported");
        }
        
        int playbackRecordingType = int.Parse(header[1]);
        float playbackCalibration = float.Parse(header[2], CultureInfo.InvariantCulture);
        Vector3[] recordingReference = new Vector3[5];
        
        string[] splitReference = header[3].Split('_');
        // get reference
        for (int i = 0; i < recordingReference.Length; i++)
        { 
            recordingReference[i] = getVector3FromString(splitReference[i]);
        }
        
        
        var splitFrames = splitRecording[1].Split(':');
        var splitPositions = new string[splitFrames.Length];
        var timings = new float[splitFrames.Length];
        var points = new Vector3[splitPositions.Length][];
        
        for (int i = 0; i < splitFrames.Length; i++)
        {
            string[] splitFrame = splitFrames[i].Split(';');
            splitPositions[i] = splitFrame[0];
            timings[i] = float.Parse(splitFrame[1], CultureInfo.InvariantCulture);
            
            string[] pointsInFrame = splitPositions[i].Split('/');
            Vector3[] frameArray = new Vector3[5];
            for (int i2 = 0; i2 < pointsInFrame.Length; i2++)
            {
                frameArray[i2] = getVector3FromString(pointsInFrame[i2]);
            }

            points[i] = frameArray;
        }
    }
    
    
    
    
    
    private static Vector3 getVector3FromString(string input)
    {
        string[] split = input.Split(',');
        return new Vector3(float.Parse(split[0], CultureInfo.InvariantCulture), float.Parse(split[1], CultureInfo.InvariantCulture), float.Parse(split[2], CultureInfo.InvariantCulture));
    }
}


public struct RecordingImportResult
{
    public string path;
    public int totalFrames;
    
}