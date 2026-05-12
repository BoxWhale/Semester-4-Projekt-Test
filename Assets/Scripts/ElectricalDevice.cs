using System;
using UnityEngine;
using System.Collections.Generic;

public class ElectricalDevice : ElectricUnit
{
    public int deviceCost;
    public List<Port> inputPorts;
    public List<Port> outputPorts;
    public float capacitor = 500;
    private float flow;
    public int totalInput;
    public bool isDeviceActive;
    
    public virtual void Start()
    {
        
        foreach (Port port in inputPorts)
        {
            port.OnValueChanged += OnInputPortsValueChanged;
            
        }
    }

    public void OnInputPortsValueChanged(int portsNewValue)
    {
        int newTotalInput = 0;
        foreach (Port port in inputPorts)
        {
            newTotalInput += port.value;
        }

        if (newTotalInput != totalInput)
        {
            OnTotalInputChange(newTotalInput);
        }
    }

    public virtual void OnTotalInputChange(int newTotalInput)
    {
        totalInput = newTotalInput;
        if (deviceCost <= totalInput && deviceCost > 0)
        {
            if (!isDeviceActive)
            {
                totalInput -= deviceCost;
                OnDevicePowerStateChange(true);
            }
        }
        else
        {
            OnDevicePowerStateChange(false);
        }
        
        // Electrical example of current trying to flow equally down each lane with least resistances
        if (outputPorts.Count > 0)
        {
            int outValuePerPort = totalInput / outputPorts.Count;
            foreach (Port port in outputPorts) port.SetValue(outValuePerPort);
        }
    }

    public virtual void OnDevicePowerStateChange(bool newState)
    {
        isDeviceActive = newState;
    }
}
