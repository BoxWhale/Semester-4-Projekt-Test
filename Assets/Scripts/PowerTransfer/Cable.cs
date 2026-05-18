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
    
    

    public bool CreateConnection(Port portA, Port portB, List<Vector3> wirePoints)
    {
        // Validate port types (must be one Input and one Output as of now)
        if (portA.portType == portB.portType) return false;
        
        Port outPort = (portA.portType == PortType.Output) ? portA : portB;
        Port inPort = (portA.portType == PortType.Input) ? portA : portB;
        
        this.portA = outPort;
        this.portB = inPort;
        
        this.portA.isConected = true;
        this.portB.isConected = true;
        
        this.portA.connectedCables.Add(this);
        this.portB.connectedCables.Add(this);

        // 2. Wire the Output to automatically update the Input
        outPort.OnValueChanged += inPort.SetValue;
        
        // 3. Kickstart the transfer immediately with whatever power the Output currently has
        // inPort.SetValue(outPort.value);        
        // Force refresh the cable update
        outPort.UpdateConnections();
        
        // Render the line
        line.positionCount = wirePoints.Count;
        line.SetPositions(wirePoints.ToArray());
        return true;
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