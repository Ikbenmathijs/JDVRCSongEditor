using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MakeSureYouHaveRightVideoPage : Page
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI url;

    public override void InitializePage()
    {
        PageName = "Check Video URL";
        url.text = $"<color=blue><u><link=\"{ReRecordingsGlobals.rerecodingSong.url}\">{ReRecordingsGlobals.rerecodingSong.url}</link></u></color>";
        SetNextPageAvailable(true);
    }
}
