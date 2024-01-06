using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;


public class SelectGamePage : Page
{
    public GameObject gameButtonPrefab;
    public Transform gameButtonParent;
    private string dataJsonUrl = "https://ikbenmathijs.github.io/jdvrc-song-editor-data/Data.json";
    private string gameImagesBaseUrl = "https://ikbenmathijs.github.io/jdvrc-song-editor-data/Images/GameIcons";
    public ErrorScreen errorScreen;
    
    private void Start()
    {
        PageName = "Select Game";
        StartCoroutine(InitializeGamesList());
    }

    private IEnumerator InitializeGamesList()
    {
        UnityWebRequest dataJsonRequest = UnityWebRequest.Get(dataJsonUrl);
        yield return dataJsonRequest.SendWebRequest();
        
        if (dataJsonRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to get data.json: " + dataJsonRequest.error);
            errorScreen.SetErrorScreen("Failed to get data.json: " + dataJsonRequest.error);
            yield break;
        }
        
        string dataJson = dataJsonRequest.downloadHandler.text;
        
        DataJson data = JsonConvert.DeserializeObject<DataJson>(dataJson);
        foreach (Icon icon in data.icons)
        {
            UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(gameImagesBaseUrl + "/" + icon.filename);
            yield return imageRequest.SendWebRequest(); 
            
            if (imageRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to get image {icon.filename}: {imageRequest.error}");
                errorScreen.SetErrorScreen($"Failed to get image {icon.filename}: {imageRequest.error}");
                yield break;
            }

            Texture2D imageTexture = ((DownloadHandlerTexture)imageRequest.downloadHandler).texture;
            Sprite gameImageSprite = Sprite.Create(imageTexture, new Rect(0, 0, imageTexture.width, imageTexture.height), new Vector2(0.5f, 0.5f));
            
            GameObject gameButton = Instantiate(gameButtonPrefab, gameButtonParent);
            GameButton gameButtonScript = gameButton.GetComponent<GameButton>();
            gameButtonScript.gameCodeName = icon.id;
            gameButtonScript.gameNameText.text = icon.name;
            gameButtonScript.gameImage.sprite = gameImageSprite;
            gameButtonScript.selectGamePage = this;
        }
        
    }
    
    public void SelectGame(string gameCodeName)
    {
        SongData.game = gameCodeName;
        
        for (int i = 0; i < gameButtonParent.childCount; i++)
        {
            Transform child = gameButtonParent.GetChild(i);
            child.GetComponent<Animator>().SetBool("UISelected", child.GetComponent<GameButton>().gameCodeName == gameCodeName);
        }
        SetNextPageAvailable(true);
    }
}



// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
[Serializable]
public class Icon
{
    public string id { get; set; }
    public string name { get; set; }
    public string filename { get; set; }
}

[Serializable]
public class DataJson
{
    public Icon[] icons { get; set; }
}

