using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionSpecificKeyframeEditor : MonoBehaviour
{
    public virtual void UpdateEditor() {}
    public virtual void InitializeEditor() {}

    
    
    public InstructionType EditorInstructionType { get; set; }
}
