using UnityEngine;

public class ElectricUnit : MonoBehaviour
{
    public string unitName;

    public virtual void OnDetected()
    {
        ElectricUI.instance.ShowDeviceName(unitName);
    }
    
    public virtual void OnEnteract(){ }
}
