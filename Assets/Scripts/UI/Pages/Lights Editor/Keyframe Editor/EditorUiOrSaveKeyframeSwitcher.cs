using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorUiOrSaveKeyframeSwitcher : MonoBehaviour
{
    public static EditorUiOrSaveKeyframeSwitcher instance;
    public GameObject saveKeyframeUi;
    public GameObject editorUi;
    
    public EditorUiOrSaveKeyframeSwitcher()
    {
        instance = this;
    }
    
    public void SwitchToSaveKeyframeUi()
    {
        saveKeyframeUi.SetActive(true);
        editorUi.SetActive(false);
        SaveKeyframeMenu.instance.InitializeSaveKeyframeMenu();
    }
    
    public void SwitchToEditorUi()
    {
        saveKeyframeUi.SetActive(false);
        editorUi.SetActive(true);
    }
}
