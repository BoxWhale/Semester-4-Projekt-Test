using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum PortType
{
    Input,
    Output,
}
public class Port : ElectricUnit
{
    public int value;
    public int maxConnections { private set; get; }
    public Action<int> OnValueChanged;
    public PortType portType;
    public bool isConected = false;
    public List<Cable> connectedCables = new List<Cable>();
    
    // Construct a bool limit to prevent overloading, this is mostly for debugging for now.
    public bool canAcceptMoreConnections => connectedCables.Count < maxConnections;
    
    public void SetValue(int newValue)
    {
        // If the new value is the same as the current value, we can skip updating and invoking the event to avoid unnecessary updates and potential infinite loops in case of circular references between ports.
        if(newValue ==  value){
            return;
        }
        value = newValue; // Distribute the value equally among connected cables (simple model for current flow for now, does not account for resistance over a grid)

        try
        {
            UpdateConnections();
        }
        catch (Exception e)
        {
            Debug.Log($"Failed to update SetValue. \nConsider forced update with internal function UpdateConnections: {e.Message} ");
            throw;
        }
    }

    // 
    public void UpdateConnections()
    {
        if (connectedCables.Count == 0) return;
        
        int powerDivision = (int)(value / connectedCables.Count);
        
        OnValueChanged?.Invoke(powerDivision);
    }
    
    void Start()
    {
        maxConnections = 2;
        rend = GetComponent<Renderer>();
        HidePort();
    }

    void Update()
    {
        
    }

    public Color selectedColor;
    public Color showColor;
    public Color hideColor;
    public Renderer rend;

    public void SelectedColor()
    {
        rend.material.color = selectedColor;
    }
    
    public void ShowPort()
    {
        rend.material.color  = showColor; 
    }

    public void HidePort()
    {
        rend.material.color = hideColor;
    }

    public override void OnDetected()
    {
        ElectricUI.instance.ShowPortName(unitName, value);
    }
}
