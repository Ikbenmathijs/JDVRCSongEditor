using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LightingCodeGen
{
    public static string GenerateCode()
    {
        Keyframe initialKeyframe = null;
        foreach (Keyframe k in KeyframesManager.instance.sortedKeyframes)
        {
            if (k.initialKeyframe)
            {
                initialKeyframe = k;
            }
        }
        if (initialKeyframe == null) return "";
        
        string code = generateCodeInstruction(initialKeyframe).Split(';')[1] + "|";

        for (int i = 0; i < KeyframesManager.instance.sortedKeyframes.Count; i++)
        {
            Keyframe keyframe = KeyframesManager.instance.sortedKeyframes[i];
            if (keyframe.initialKeyframe) continue;
            code += generateCodeInstruction(keyframe);
            if (i < KeyframesManager.instance.sortedKeyframes.Count - 1)
            {
                code += "|";
            }
        }

        return code;
    }

    private static string generateCodeInstruction(Keyframe keyframe)
    {
        Instruction instruction = keyframe.instruction;
        string time = keyframe.time.ToString("0.000");
        if (instruction.instructionType == InstructionType.SetColors)
        {
            if (instruction.changeBackgroundColor && instruction.colors.Count > 0)
            {
                string code;
                if (!instruction.fadeBackgroundColor)
                {
                    code = $"{time};SetColors:>{colorToRgb(instruction.backgroundColor)}/";
                }
                else
                {
                    code = $"{time};LerpDefault:{colorToRgb(instruction.backgroundColor)}/{instruction.backgroundColorFadeSpeed.ToString("0.00")}|{time};SetColors:";
                }
                
                
                string colors = "";
                for (int i = 0; i < instruction.colors.Count; i++)
                {
                    colors += colorToRgb(instruction.colors[i]);
                    if (i < instruction.colors.Count - 1)
                    {
                        colors += "/";
                    }
                }

                return code + colors;
            }
            
            
            if (instruction.changeBackgroundColor && instruction.colors.Count == 0)
            {
                if (instruction.fadeBackgroundColor)
                {
                    return $"{time};LerpDefault:{colorToRgb(instruction.backgroundColor)}/{instruction.backgroundColorFadeSpeed.ToString("0.00")}";
                }
                return $"{time};LerpDefault:{colorToRgb(instruction.backgroundColor)}/999";
            }
            
            
            if (!instruction.changeBackgroundColor && instruction.colors.Count > 0)
            {
                string code = $"{time};SetColors:";
                
                for (int i = 0; i < instruction.colors.Count; i++)
                {
                    code += colorToRgb(instruction.colors[i]);
                    if (i < instruction.colors.Count - 1)
                    {
                        code += "/";
                    }
                }
                return code;
            }
            return "";
        } else if (instruction.instructionType == InstructionType.SetBrightness)
        {
            return $"{time};SetBrightness:{instruction.brightnessMultiplier.ToString("0.00")}";
        } else if (instruction.instructionType == InstructionType.SetFill)
        {
            return $"{time};SetFill:{colorToRgb(instruction.fillColor)}";
        } else if (instruction.instructionType == InstructionType.DisableFill)
        {
            return $"{time};DisableFill";
        } else if (instruction.instructionType == InstructionType.SolidColors)
        {
            if (instruction.solidColorOnBeat && instruction.solidColorsOnInterval)
            {
                return $"{time};SolidColors:All";
            } else if (!instruction.solidColorOnBeat && instruction.solidColorsOnInterval)
            {
                return $"{time};SolidColors:Interval";
            } else if (instruction.solidColorOnBeat && !instruction.solidColorsOnInterval)
            {
                return $"{time};SolidColors:Beat";
            } 
            return $"{time};SolidColors:None";
        } else if (instruction.instructionType == InstructionType.Disable)
        {
            if (instruction.disableBeat && instruction.disableInterval)
            {
                return $"{time};Disable:All";
            } else if (!instruction.disableBeat && instruction.disableInterval)
            {
                return $"{time};Disable:Interval";
            } else if (instruction.disableBeat && !instruction.disableInterval)
            {
                return $"{time};Disable:Beat";
            } 
            return $"{time};Disable:None";
        } else if (instruction.instructionType == InstructionType.GoldMove)
        {
            string code = $"{time};GoldMove:";
            if (instruction.goldMoveType == GoldMoveType.All)
            {
                code += "All";
            } else if (instruction.goldMoveType == GoldMoveType.Dancer0)
            {
                code += "0";
            } else if (instruction.goldMoveType == GoldMoveType.Dancer1)
            {
                code += "1";
            } else if (instruction.goldMoveType == GoldMoveType.Dancer2)
            {
                code += "2";
            } else if (instruction.goldMoveType == GoldMoveType.Dancer3)
            {
                code += "3";
            }
            return code;
        }

        return "";
    }

    private static string colorToRgb(Color color)
    {
        return (int)(color.r * 255f) + "," + (int)(color.g * 255f) + "," + (int)(color.b * 255f);
    }
}
