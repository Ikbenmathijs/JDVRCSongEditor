using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PagesManager : MonoBehaviour
{
    
    
    
    public Page[] menuPages;
    public int currentPage = 0;
    
    public TextMeshProUGUI pageNameText;

    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button previousPageButton;
    
    public bool loadInitializeScene = true;

    private void Start()
    {
        UpdatePage();
        menuPages[currentPage].InitializePage();
        menuPages[currentPage].GetComponent<Animator>().SetBool("PageIn", true);
        menuPages[currentPage].GetComponent<Animator>().SetBool("Right", true);
        
        
        // for convenience
#if UNITY_EDITOR
        if (!Config.initialized && loadInitializeScene)
            SceneManager.LoadScene("Init");
#endif
    }

    public void NextPage()
    {
        if (!menuPages[currentPage].nextPageAvailable)
        {
            Debug.LogWarning("Next page got called, but it's not marked as available");
            return;
        }
        menuPages[currentPage].GetComponent<Animator>().SetBool("Right", true);
        menuPages[currentPage].GetComponent<Animator>().SetBool("PageIn", false);
        menuPages[currentPage].OnPageUnfocus();
        for (int i = currentPage + 1; i < menuPages.Length; i++)
        {
            if (!menuPages[i].CanPageBeSkipped())
            {
                currentPage = i;
                break;
            }
        }
        menuPages[currentPage].GetComponent<Animator>().SetBool("Right", false);
        menuPages[currentPage].GetComponent<Animator>().SetBool("PageIn", true);
        UpdatePage();
        menuPages[currentPage].InitializePage();
    }
    
    public void PreviousPage()
    {
        menuPages[currentPage].GetComponent<Animator>().SetBool("Right", false);
        menuPages[currentPage].GetComponent<Animator>().SetBool("PageIn", false);
        menuPages[currentPage].OnPageUnfocus();
        for (int i = currentPage - 1; i >= 0; i--)
        {
            if (!menuPages[i].CanPageBeSkipped())
            {
                currentPage = i;
                break;
            }
        }
        menuPages[currentPage].GetComponent<Animator>().SetBool("Right", true);
        menuPages[currentPage].GetComponent<Animator>().SetBool("PageIn", true);
        UpdatePage();
    }
    
    private void UpdatePage()
    {
        nextPageButton.interactable = menuPages[currentPage].nextPageAvailable;
        previousPageButton.interactable = currentPage != 0;
        pageNameText.text = menuPages[currentPage].PageName;
        
    }
    
    public void RefreshNextPageAvailability()
    {
        nextPageButton.interactable = menuPages[currentPage].nextPageAvailable;
    }
    
    
    
}
