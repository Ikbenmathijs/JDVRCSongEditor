using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongListing : MonoBehaviour
{
    private SelectSongToReRecord selectSongToReRecord;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI titleText;

    [SerializeField] private GameObject checkmark;
    
    private SongEntry songEntry;



    public void InjectDependencies(SelectSongToReRecord selectSongToReRecord)
    {
        this.selectSongToReRecord = selectSongToReRecord;
    }

    public void SetSong(SongEntry song)
    {
        songEntry = song;
        titleText.text = $"{song.name} - {song.artist}     ({song.amountOfDancers} dancers)";
    }


    public void OnClick()
    {
        selectSongToReRecord.DeselectAllSongEntries();
        checkmark.SetActive(true);
        ReRecordingsGlobals.rerecodingSong = songEntry;
        
        selectSongToReRecord.SetNextPageAvailable(true);
    }

    public void Deselect()
    {
        checkmark.SetActive(false);
    }

    public string GetSearchName()
    {
        return songEntry.searchName;
    }

    public string GetArtistSearchName()
    {
        return songEntry.artist.ToLower().Replace(" ", "");
    }
}
