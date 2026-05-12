using System;
using UnityEngine;


// This class creates a global time tick system that other scripts can subscribe to for regular updates.
// Inspiration was taken from https://www.youtube.com/watch?v=FNFJ_R9zqXI&t=6s
public static class TimeTickSystem
{
    public class OnTickEventArgs : EventArgs
    {
        public int TickCount;
    }
    
    // One tick increment invoke
    public static event EventHandler<OnTickEventArgs> OnTick;
    // 5th tick increment invoke, useful for less frequent updates or actions that should happen every 5th tick
    public static event EventHandler<OnTickEventArgs> OnTick5;

    // The time interval for each tick in seconds. Adjust as needed for faster or slower ticks
    // Note: decreasing interval will increase the frequency of ticks, which may impact performance if too low
    private const float TickTimerMax = 0.2f; // There should be 1 tick every 1/5th of a second
    
    
    
    // A private GameObject to hold the TimeTickSystemObject component, ensuring it persists across scenes and is only created once
    private static GameObject _timeTickSystemGameObject;

    // A private integer to keep track of the current tick count, which can be accessed publicly through the Tick property
    private static int _tick;
    //Tick property which is open accessable for other scripts to read the current tick count, but cannot be modified directly from outside this class
    public static int Tick => _tick;

    // singleton-like method to create the TimeTickSystemObject if it doesn't already exist
    public static void Create()
    {
        if (_timeTickSystemGameObject != null)
        {
            return;
        }

        _timeTickSystemGameObject = new GameObject("TimeTickSystem");
        UnityEngine.Object.DontDestroyOnLoad(_timeTickSystemGameObject);
        _timeTickSystemGameObject.AddComponent<TimeTickSystemObject>();
    }
    // The TimeTickSystemObject is a MonoBehaviour that handles the timing and ticking logic.
    // It updates every frame, checks if the tick timer has reached the maximum, and invokes the appropriate events when ticks occur.
    private class TimeTickSystemObject : MonoBehaviour
    {
        public bool halted =  false;
        public float tickAccelerator = 1;
        public bool resetTimer = false;
        private float _tickTimer;

        private void Awake()
        {
            _tick = 0;
            _tickTimer = 0f;
        }

        private void Update()
        {
            if (resetTimer) { _tick = 0; resetTimer = false; }
            if (halted) return;
            _tickTimer += Time.deltaTime * tickAccelerator;

            while (_tickTimer >= TickTimerMax)
            {
                _tickTimer -= TickTimerMax;
                _tick++;
                
                OnTick?.Invoke(this, new OnTickEventArgs { TickCount = _tick });
                
                if (_tick % 5 == 0)
                {
                    OnTick5?.Invoke(this, new OnTickEventArgs { TickCount = _tick });
                }
            }
        }
    }
}
