using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VRWireTool : MonoBehaviour
{
    [Header("Runtime References for debugging")]
    public Port firstSelectedPort;
    public Port hoveredPort; 
    public Cable tempCable;
    public List<Vector3> wirePoints = new List<Vector3>();

    [Header("Input Actions")]
    public InputActionProperty gripAction;
    public InputActionProperty cancleAction;
    public GameObject cablePrefab;

    #region Input Action Setup
    
    private void OnEnable()
    {
        if (gripAction.action != null)
        {
            gripAction.action.performed += OnGripPressed;
            gripAction.action.Enable();
        }
        if (cancleAction.action != null)
        {
            cancleAction.action.performed += CancelSelection;
            cancleAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (gripAction.action != null)
        {
            gripAction.action.performed -= OnGripPressed;
            gripAction.action.Disable();
        }
        if (cancleAction.action != null)
        {
            cancleAction.action.performed -= CancelSelection;
            cancleAction.action.Disable();
        }
    }
    #endregion

    // XR Interactable Click (Handles Start and End of Cables)
    public void OnPortSelected(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.parent.tag != "Right") return; // Only allow the right hand to weld
        
        Port clickedPort = args.interactableObject.transform.GetComponent<Port>();
        if (clickedPort == null || !clickedPort.canAcceptMoreConnections) return;

        if (firstSelectedPort == null)
        {
            // Start the cable
            firstSelectedPort = clickedPort;
            firstSelectedPort.SelectedColor();

            // Setup new cable
            wirePoints.Clear();
            wirePoints.Add(firstSelectedPort.transform.position);

            tempCable = Instantiate(cablePrefab).GetComponent<Cable>();
            tempCable.line.positionCount = 2; // Point 0 is port, Point 1 will follow hand
            tempCable.line.SetPosition(0, firstSelectedPort.transform.position);
            tempCable.line.SetPosition(1, firstSelectedPort.transform.position);
            hoveredPort = null;
        }
        else
        {
            // Ignore selection if its the same port
            if (firstSelectedPort == clickedPort) return;
            // Check if there is already a cable connecting these exact two ports
            foreach (Cable existingCable in firstSelectedPort.connectedCables)
            {
                if (existingCable.portA == clickedPort || existingCable.portB == clickedPort)
                {
                    Debug.LogWarning("These ports are already connected!");
                    // Abort logic: delete tempCable, clear wirePoints...
                    return;
                }
            }
            // Optional: Pole validation can be done here
            wirePoints.Add(clickedPort.transform.position);
            
            // Finalize connection
            if (!tempCable.CreateConnection(firstSelectedPort, clickedPort, wirePoints))
            {
                Debug.LogWarning("Tried to connect two ports of the same type! Wiring Prevented");
                
                wirePoints.RemoveAt(wirePoints.Count-1);
            }
            else
            {
                // Reset Tool
                firstSelectedPort.HidePort();
                firstSelectedPort = null;
                tempCable = null;
                wirePoints.Clear();
            }
        }
    }

    // Empty Space Click (Handles Mid-Air Nodes exclusively)
    private void OnGripPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // If we aren't drawing a cable, or we are looking at a real port, IGNORE this click.
            if (firstSelectedPort == null || tempCable == null || hoveredPort != null) return;

            Debug.Log("Midair node placed!");
            
            // Add midair point
            Vector3 midairPoint = this.transform.position;
            wirePoints.Add(midairPoint);

            // Lock the previous tail to this new point, and spawn a new tail slot
            tempCable.line.positionCount = wirePoints.Count + 1;
            tempCable.line.SetPosition(wirePoints.Count - 1, midairPoint);
        }
    }

    // Live Drawing
    private void Update()
    {
        // Smoothly draw the unattached 'tail' of the wire to the controller's position every frame
        if (firstSelectedPort != null && tempCable != null && tempCable.line.positionCount > 0)
        {
            tempCable.line.SetPosition(tempCable.line.positionCount - 1, this.transform.position);
        }
    }

    // Hover Events (UI Feedback Only)
    public void OnPortHover(HoverEnterEventArgs args)
    {
        Port hovered = args.interactableObject.transform.GetComponent<Port>();
        if (hovered == null || hovered == firstSelectedPort || !hovered.canAcceptMoreConnections) return; 
        hoveredPort = hovered;
        hoveredPort.ShowPort();
    }
    
    public void OnPortHoverExit(HoverExitEventArgs args)
    {
        Port hovered = args.interactableObject.transform.GetComponent<Port>();
        if (hovered == null || hovered == firstSelectedPort) return; 
        hovered.HidePort(); 
        hoveredPort = null;
    }

    // Cancellation
    private void CancelSelection(InputAction.CallbackContext context)
    {
        // The cable can never be 0 or less, in this case ignore
        if (tempCable.line.positionCount == 0) return;
        
        if (tempCable.line.positionCount == 2)
        {
            wirePoints.Clear();
            firstSelectedPort.HidePort();
            firstSelectedPort = null;
            Destroy(tempCable.gameObject);
            Debug.Log("Cable Selection Cancelled");
        }
        else
        {
            // Remove the last midair point and update the line
            wirePoints.RemoveAt(tempCable.line.positionCount - 2);
            tempCable.line.positionCount -= 1;
            Debug.Log("Selection Undone");
        }
    }
}