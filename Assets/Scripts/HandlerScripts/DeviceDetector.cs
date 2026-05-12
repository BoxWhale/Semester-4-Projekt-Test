using UnityEngine;
using UnityEngine.InputSystem;

public class DeviceDetector : MonoBehaviour
{
    public InputActionProperty triggerAction;
    
    public ElectricUnit detectedUnit;
    public float detectionRange = 5;

    #region Input Action Setup
    
    void OnEnable()
    {
        if (triggerAction.action != null)
        {
            triggerAction.action.performed += OnTriggerPressed;
            triggerAction.action.Enable();
        }
    }

    void OnDisable()
    {
        if (triggerAction.action != null)
        {
            triggerAction.action.performed -= OnTriggerPressed;
            triggerAction.action.Disable();
        }
    }
    #endregion

    void OnTriggerPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Trigger pressed!");
        HandleDetectedDevices();
    }

    public void HandleDetectedDevices()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, detectionRange))
        {
            if(hit.transform.TryGetComponent<ElectricUnit>(out ElectricUnit unit))
            {
                if (unit == detectedUnit) return;
                detectedUnit = unit;
                detectedUnit.OnDetected();
                //ElectricUI.instance.ShowDeviceName(unit.unitName);
            }
            else
            {
                if (detectedUnit == null) return;
                Debug.Log("Hit object does not have an ElectricUnit component.");
                detectedUnit = null;
                ElectricUI.instance.HideAll();
            }
        }
        else
        {
            if (detectedUnit == null) return;
            detectedUnit = null;
            ElectricUI.instance.HideAll();
        }
    }
    /*void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward);
    }*/
}
