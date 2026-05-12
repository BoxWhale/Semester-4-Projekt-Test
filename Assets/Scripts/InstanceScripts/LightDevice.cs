using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightDevice : ElectricalDevice
{
    public GameObject lightObject;
    private Renderer lightRenderer;
    public Light light;
    public Color lightColor;
    
    public override void Start()
    {
        base.Start();
        lightRenderer = lightObject.GetComponent<Renderer>();
        
    }

    public override void OnDevicePowerStateChange(bool newState)
    {
        base.OnDevicePowerStateChange(newState);
        if (newState)
        {
            //the device is on
            light.color = lightColor;
            light.gameObject.SetActive(true);
            lightRenderer.material.EnableKeyword("_EMISSION");
            lightRenderer.material.SetColor("_EmissionColor", lightColor);
        }
        else
        {
            //the device is off
            light.gameObject.SetActive(false);
            lightRenderer.material.DisableKeyword("_EMISSION");
        }
    }
}
