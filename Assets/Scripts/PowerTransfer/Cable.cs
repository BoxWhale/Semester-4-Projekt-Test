using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class Cable : MonoBehaviour
{
    public Port portA;
    public Port portB;
    public LineRenderer line;
    
    

    public void CreateConnection(Port portA, Port portB, List<Vector3> wirePoints)
    {
        this.portA = portA;
        this.portB = portB;
        
        this.portA.isConected = true;
        this.portB.isConected = true;
        
        this.portA.connectedCable = this;
        this.portB.connectedCable = this;

        Port outPort = null;
        Port inPort = null;

        if (portA.portType == PortType.Output && portB.portType == PortType.Input)
        {
            outPort = portA;
            inPort = portB;
        }
        else if (portA.portType == PortType.Input && portB.portType == PortType.Output)
        {
            outPort = portB;
            inPort = portA;
        }
        else
        {
            Debug.LogWarning("Tried to connect two ports of the same type! Wiring failed.");
            return;
        }

        // 2. Wire the Output to automatically update the Input
        outPort.OnValueChanged += inPort.SetValue;
        
        // 3. Kickstart the transfer immediately with whatever power the Output currently has
        inPort.SetValue(outPort.value); 
        
        // Render the line
        line.positionCount = wirePoints.Count;
        line.SetPositions(wirePoints.ToArray());
    }
    

    public bool CanCreateConnection()
    {
        return portA && portB;
    }

    private void ActivateConnection(Port a = null, Port b = null)
    {
                            
        portA.isConected = true;
        portB.isConected = true;
        
        line.positionCount = 2;
        line.SetPosition(0, a.transform.position);
        line.SetPosition(1, b.transform.position);
    }
}