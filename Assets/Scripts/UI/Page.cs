using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Page : MonoBehaviour
{
    public PagesManager pagesManager;
    public bool nextPageAvailable = false;

    public string PageName { get; set; }


    public void NextPage()
    {
        SetNextPageAvailable(true);
        pagesManager.NextPage();
    }
    
    public void SetNextPageAvailable(bool available)
    {
        nextPageAvailable = available;
        pagesManager.RefreshNextPageAvailability();
    }

    public virtual void InitializePage() {}

    public virtual bool CanPageBeSkipped()
    {
        return false;
    }

}
