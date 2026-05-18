using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// A simple GameHandler
// Use this script when to apply permanent code logic to the game, such as the time tick system, which is used to send pulses to the breadboard logic and other scripts that need to be updated on a regular basis. This script should be attached to a GameObject in the scene, and it will handle the subscription to the time tick events and the logic that needs to be executed on each tick.
public class HandlerScript : MonoBehaviour
{
    private float _timer = 0f;
    private bool fadeActive;
    private bool mustReleaseButtonFirst = false; // <--- The ultimate lock
    [Tooltip("The time it takes before the scene reset occurs by holding down R")]
    public float resetDelay = 3f;
    [Tooltip("The button which initiate the scene resetting")]
    public InputActionProperty resetButton;
    public RawImage fadeImage;
    
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
        
        
        _timer = resetDelay-0.1f;
        fadeActive = false;
    }
    
    private void Start()
    {
        if (resetButton.action != null)
        {
            resetButton.action.performed += FadeToBlack;
            resetButton.action.canceled += FadeToBlack;
            resetButton.action.Enable();
            
            if (resetButton.action.IsPressed())
            {
                // If they are holding it from the previous scene, LOCK THEM OUT
                mustReleaseButtonFirst = true; 
            }
        }
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


    void FadeToBlack(InputAction.CallbackContext context)
    {
        Debug.Log($"Button {context.action.name} is pressed");

        if (context.canceled)
        { 
            // They finally let go of the button! Lift the lock.
            mustReleaseButtonFirst = false;
            fadeActive = false;
        }
        else if (context.performed)
        {
            // If the lock is active, ignore this press completely
            if (mustReleaseButtonFirst) return;

            fadeActive = true;
        }
    }

    void Update()
    {
        // There's a persistent problem with this code, where if the player holds the reset button from the previous scene, it will immediately trigger the fade and reset on the new scene.
        // No prevention of this seems possible as input system, always carries over their state to the next scene, but bool resetting doesnt.
        // DONT HOLD DOWN THE R BUTTON
        if (fadeActive)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            _timer -= Time.deltaTime;
        }
        
        _timer = Mathf.Clamp(_timer, 0, resetDelay);
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, _timer / resetDelay);
        }
        else
        {
            // Since the scene is rendering in, the process power is not in question here.
            fadeImage = GameObject.Find("FadeImage").GetComponent<RawImage>();
        }
        
        if (fadeActive && _timer >= resetDelay)
        {
            Debug.Log("Resetting Scene...");
            fadeActive = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
