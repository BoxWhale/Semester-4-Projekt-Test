using UnityEngine;

public class PowerStorage : PowerSource
{
    public Port inputPort;
    //public Port outputPort; Inherited from PowerSource
    
    public int maxCapacity;
    public int currentStorageAmount;

    //public int maxOutput;  Inherited from PowerSource
    //public int currentOutput;  Inherited from PowerSource

    
    public bool isGenerating;
    

    public override void RefreshPower()
    {
        // 1. Calculate energy math
        int usedenergy = Mathf.Min(GetConnectedDeviceCost(), currentOutput);
        
        currentStorageAmount -= usedenergy;
        currentStorageAmount += inputPort.value;
        
        // 2. Clamp capacity
        currentStorageAmount = Mathf.Clamp(currentStorageAmount, 0, maxCapacity);
        
        // 3. Set output (using inherited variables)
        currentOutput = Mathf.Min(currentStorageAmount, maxOutput);

        // 4. IMPORTANT: Actually tell the port about the new output! ignored for now until more logic is added
        // outputPort.SetValue(currentOutput);
    }

    public int GetConnectedDeviceCost()
    {
        // Placeholder for future logic
        return 0;
    }
    
}
