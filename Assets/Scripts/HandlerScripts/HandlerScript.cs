using System;
using System.Collections.Generic;
using UnityEngine;

// A simple GameHandler
// Use this script when to apply permanent code logic to the game, such as the time tick system, which is used to send pulses to the breadboard logic and other scripts that need to be updated on a regular basis. This script should be attached to a GameObject in the scene, and it will handle the subscription to the time tick events and the logic that needs to be executed on each tick.
public class HandlerScript : MonoBehaviour
{
    
    [Tooltip("If true, TimeTickSystem will be instantiated on first frame of game start, otherwise it will not be created and no ticks will be sent. This can be useful for testing or if you want to control when the TimeTickSystem starts ticking.")]
    public bool CreateTickPulse = true;
    
    public static HandlerScript Instance { get; private set; } // A static instance of the HandlerScript to allow other scripts to easily access it and register themselves for ticking updates without needing to find the HandlerScript in the scene or pass references around. This is a common pattern for manager or handler scripts that need to be accessed globally.
    
    public List<PowerSource> powerSources = new List<PowerSource>(); // A list of power sources in the scene that need to be refreshed on each tick

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple instances of HandlerScript detected. Destroying duplicate.");
            Destroy(this);
            return;
        }
        Instance = this;
    }
    
    private void Start()
    {
        if (!CreateTickPulse) return;
        TimeTickSystem.Create();
        TimeTickSystem.OnTick += HandleTick;
        TimeTickSystem.OnTick5 += HandleTick5;
    }

    private void OnDestroy()
    {
        // Unsubscribe from events to prevent potential memory leaks or null reference exceptions when the object is destroyed
        if (!CreateTickPulse) return;
        TimeTickSystem.OnTick -= HandleTick;
        TimeTickSystem.OnTick5 -= HandleTick5;
    }

    private void HandleTick(object sender, TimeTickSystem.OnTickEventArgs e)
    {
        // This is a more concise way to call refreshCoroutine on each power source in the list, using a lambda expression. It achieves the same result as the foreach loop above, but in a single line of code.
        powerSources.ForEach(power => power.RefreshPower()); // This is called refreshCoroutine in the video
        //Debug.Log("Tick: " + e.TickCount);
    }

    private void HandleTick5(object sender, TimeTickSystem.OnTickEventArgs e)
    {
        //Debug.Log("Tick 5th increment: " + e.TickCount);
    }

    public void AssignPowerSource(PowerSource source) // This method can be called by PowerSource objects to register themselves with the HandlerScript, allowing them to be refreshed on each tick. This is a simple way to manage power sources without needing a more complex system for tracking them.
    {
        try
        {
            powerSources.Add(source);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to assign power source {source.gameObject.name} to HandlerScript: {e.Message}");
            throw;
        }
    }
}
