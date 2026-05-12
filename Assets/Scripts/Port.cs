using System;
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
    public Action<int> OnValueChanged;
    public PortType portType;
    public bool isConected = false;
    public Cable connectedCable;
    
    public void SetValue(int newValue)
    {
        // If the new value is the same as the current value, we can skip updating and invoking the event to avoid unnecessary updates and potential infinite loops in case of circular references between ports.
        if(newValue ==  value){
            return;
        }
        value = newValue;
        OnValueChanged?.Invoke(value);
    }
    
    void Start()
    {
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
