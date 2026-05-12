using System;
using System.Collections.Generic;
using UnityEngine;

public class Battery : PowerSource
{
    public override bool CanGeneratePower()
    {
        return true;
    }
    
    
    public override void OnPowerGenerationStateChanged(bool newState)
    {
        isGeneratingPower  = newState;

        if (!isGeneratingPower) return;
        float tempOutput = maxOutput;
        currentOutput = (int)tempOutput;
        
        outputPort.SetValue(currentOutput); 
    }
}
