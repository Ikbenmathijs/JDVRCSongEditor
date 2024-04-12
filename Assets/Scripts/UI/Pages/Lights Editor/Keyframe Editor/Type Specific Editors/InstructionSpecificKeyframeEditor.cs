using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionSpecificKeyframeEditor : MonoBehaviour
{
    public virtual void UpdateEditor() {}
    
    public InstructionType EditorInstructionType { get; set; }
}
