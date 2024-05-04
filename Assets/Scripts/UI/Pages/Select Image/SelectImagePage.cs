using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;


public class SelectImagePage : Page
{
    public Popup popup;
    public Image imagePreview;
    void Start()
    {
        PageName = "Select Image";
    }

    public void SelectImageButtonPressed()
    {
        StartCoroutine(SelectImage());
    }

    public IEnumerator SelectImage()
    {
        // open file browser
        string[] files = StandaloneFileBrowser.OpenFilePanel("Select Recording File", "", "png", false);
        
        if (files.Length != 1)
        {
            popup.ShowPopup("Please select 1 image file");
            yield break;
        }
        
        string path = files[0];

        UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(path);
        yield return imageRequest.SendWebRequest();
        
        if (imageRequest.result != UnityWebRequest.Result.Success)
        {
            popup.ShowPopup($"Failed to get image {path}: {imageRequest.error}");
            yield break;
        }
        
        Texture2D imageTexture = ((DownloadHandlerTexture)imageRequest.downloadHandler).texture;

        if (imageTexture.width != imageTexture.height)
        {
            popup.ShowPopup($"Image must be a square! Use an image editing program such as Paint.NET to crop the image: https://getpaint.net/");
            yield break;
        }
        
        Sprite imageSprite = Sprite.Create(imageTexture, new Rect(0, 0, imageTexture.width, imageTexture.height), new Vector2(0.5f, 0.5f));

        imagePreview.sprite = imageSprite;
        File.Copy(path, $"{Config.tempFolder}/image.png", true);
        SongData.imagePath = $"{Config.tempFolder}/image.png";
        
        SetNextPageAvailable(true);
    }

    
}
