using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class RuntimeTestScript : MonoBehaviour
{
    public bool run;
    
    public RectTransform rectTransform;

    private void Update()
    {
        if (!run) return;
        run = false;
        
        Debug.Log(rectTransform.position.y);
    }
}
