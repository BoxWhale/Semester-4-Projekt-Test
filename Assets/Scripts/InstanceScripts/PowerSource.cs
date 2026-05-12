using System.Collections;
using UnityEngine;

public class PowerSource : ElectricUnit {
    public Port outputPort;
    public int currentOutput;
    public int maxOutput;
    public bool isGeneratingPower;
    
    protected virtual void OnEnable()
    {
        if (HandlerScript.Instance != null)
        {
            Debug.Log($"Registering {gameObject.name} to {nameof(PowerSource)} via OnEnable...");
            HandlerScript.Instance.AssignPowerSource(this);
        }
        else
        {
            Debug.LogWarning($"HandlerScript instance not found when enabling {gameObject.name}. It will be registered in Start().");
        }
    }
    
    protected virtual void Start()
    {
        // We use this as a catch-up to ensure it gets registered on the first frame.
        if (HandlerScript.Instance != null && !HandlerScript.Instance.powerSources.Contains(this))
        {
            Debug.Log($"Registering {gameObject.name} to {nameof(PowerSource)} via Start...");
            HandlerScript.Instance.AssignPowerSource(this);
        }
    }
    
    protected virtual void OnDisable()
    {
        
        if (HandlerScript.Instance == null) return; // Prevent errors when stopping the game
        
        HandlerScript.Instance.powerSources.Remove(this); 
    }
    //The ticksystem is already made, and is instead run through the HandlerScript.cs which should synchronize it all better compared to running it all in coroutines
    public virtual void RefreshPower() //This needs to be connected to the time tick system through the HandlerScript, Registration happens through the OnEnable method, which adds this power source to the HandlerScript's list of power sources that need to be refreshed on each tick. The RefreshPower method is then called on each tick for all registered power sources, allowing them to update their power generation state and output accordingly. This is a more efficient way to manage regular updates for multiple power sources compared to using individual coroutines for each one.
    { // This used to be refreshCoroutine in the video, but since we have a time tick system, we can just call this method on each tick instead of using a coroutine to wait for a certain amount of time before refreshing the power. This should be more efficient and easier to manage than using coroutines for timing.
        Debug.Log($"{gameObject.name} with output: {currentOutput}");
        OnPowerGenerationStateChanged(CanGeneratePower());
    }
    
    public virtual bool CanGeneratePower()
    {
        return true;
    }

    public virtual void OnPowerGenerationStateChanged(bool newState)
    {
        if (newState == isGeneratingPower) return;
        
        isGeneratingPower = newState;
        
        // Supposedly, power discharges slowly over time, but for now its just a binary on/off state
        currentOutput = isGeneratingPower ? maxOutput : 0;
        
        outputPort.SetValue(currentOutput);
    }
}
