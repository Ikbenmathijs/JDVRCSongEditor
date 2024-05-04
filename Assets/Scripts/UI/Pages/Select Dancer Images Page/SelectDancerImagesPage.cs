using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using UnityEngine.Networking;

public class SelectDancerImagesPage : Page
{
    public Transform dancerImagesParent;
    public GameObject dancerImageSelectorPrefab;
    public SelectDancerImageButton[] dancerImageSelectors = new SelectDancerImageButton[4];
    public Popup popup;
    private void Start()
    {
        PageName = "Select Dancer Images";
    }

    public override void InitializePage()
    {
        for (int i = 0; i < dancerImagesParent.childCount; i++)
        {
            dancerImagesParent.GetChild(i).gameObject.SetActive(i < SongData.dancerAmount);
        }
    }

    public void SelectDancerImageButtonPressed(int dancerIndex)
    {
        StartCoroutine(SetDancerImage(dancerIndex));
    }

    private IEnumerator SetDancerImage(int dancerIndex)
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
        
        Sprite imageSprite = Sprite.Create(imageTexture, new Rect(0, 0, imageTexture.width, imageTexture.height), new Vector2(0.5f, 0.5f));

        dancerImageSelectors[dancerIndex].dancerImage.sprite = imageSprite;
        
        SongData.dancerImagePaths[dancerIndex] = path;
        SongData.dancerImageSet[dancerIndex] = true;
        
        SetNextPageAvailable(AllDancerImagesAreSet());
    }

    private static bool AllDancerImagesAreSet()
    {
        for (int i = 0; i < SongData.dancerAmount; i++)
        {
            if (!SongData.dancerImageSet[i])
            {
                return false;
            }
        }

        return true;
    }
    
    public override bool CanPageBeSkipped()
    {
        return SongData.dancerAmount == 1;
    }
}
