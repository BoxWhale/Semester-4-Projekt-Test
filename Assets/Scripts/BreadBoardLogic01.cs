using UnityEngine;

public class BreadBoardLogic01 : MonoBehaviour
{
    //Grab the gameobject that has the following attachement
    
    //Adjust the size and grid
    
    //Make the gameobject fit the size of the grid
    
    //Find a way to interconnect grid dots, for allowance of current flow, like that of a breadboard
    
    //Find a way to simulate current flow through the grid, and then signal if it works or not "use ticks and pulses"
    
    //Find a way to apply cables, that can transfer pulses post-cross the grid

    //Note: There must be a positive and negative end/start destination, or the simulation needs to indicate failure
    
    
    //Start idea:
    //Make a global clock that the entire simulation runs on
        //TimeTickSystem have been made, and takes account for this
        //By logic, it sends an invoke to:
            //
    //This clock sends a pulse to every object that either have a specific component or tag applied to it, prehaps use of event-listener can do this
    //For this to work correctly, use of HandlerScript would be better, so the code only need to exist in one place
    
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
