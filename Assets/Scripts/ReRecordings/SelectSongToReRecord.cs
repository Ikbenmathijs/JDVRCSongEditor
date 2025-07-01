using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class SelectSongToReRecord : Page
{

    private string songsJsonUrl = "https://ikbenmathijs.github.io/JDVRCSongsList/songlist.json";

    [Header("Dependencies")] 
    [SerializeField] private Popup popup;
    

    [Header("UI Elements")]
    [SerializeField] private Transform songListParent;
    [SerializeField] private GameObject songListEntryPrefab;
    [SerializeField] private TMP_InputField searchBar;
        
    
    private void Start()
    {
        PageName = "Select Song For The Re-Recording";
        StartCoroutine(InitializeSongList());
    }

    private IEnumerator InitializeSongList()
    {
        UnityWebRequest dataJsonRequest = UnityWebRequest.Get(songsJsonUrl);
        yield return dataJsonRequest.SendWebRequest();
        
        if (dataJsonRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to get songlist.json: " + dataJsonRequest.error);
            popup.ShowPopup("Failed to get songlist.json: " + dataJsonRequest.error);
            yield break;
        }
        
        string dataJson = dataJsonRequest.downloadHandler.text;
        
        List<SongEntry> songs = JsonConvert.DeserializeObject<List<SongEntry>>(dataJson);
        
        foreach (SongEntry song in songs)
        {
            GameObject songListingObject = Instantiate(songListEntryPrefab, songListParent);
            SongListing songListing = songListingObject.GetComponent<SongListing>();
            songListing.InjectDependencies(this);
            songListing.SetSong(song);
        }
    }

    public void DeselectAllSongEntries()
    {
        for (int i = 0; i < songListParent.childCount; i++)
        {
            SongListing songListing = songListParent.GetChild(i).GetComponent<SongListing>();
            songListing.Deselect();
        }
    }

    public void SearchBarChanged()
    {
        string query = searchBar.text.ToLower().Replace(" ", "");
        
        for (int i = 0; i < songListParent.childCount; i++)
        {
            SongListing songListing = songListParent.GetChild(i).GetComponent<SongListing>();
            if (query == "" || songListing.GetSearchName().Contains(query) || songListing.GetArtistSearchName().Contains(query))
            {
                songListParent.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                songListParent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}

