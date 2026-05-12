using TMPro;
using UnityEngine;

public class ElectricUI : MonoBehaviour
{
    public static ElectricUI instance;

    void Awake()
    {
        instance = this;
    }

    [Header("Device Name Display")]
    public GameObject deviceNamePanel;
    public TMP_Text deviceNameText;

    [Header("Port Name Display")]
    public GameObject portNamePanel;
    public TMP_Text portNameText;
    public TMP_Text portValueText;
    
    [Header("Storage Name Display")]
    public GameObject storageNamePanel;
    public TMP_Text storageNameText;
    public TMP_Text storageCapacityText;
    public TMP_Text storageMaxOutputText;
    
    public void HideAll()
    {
        HideDeviceNamePanel();
        HidePortNamePanel();
        HideStorageNamePanel();
    }
    
    public void ShowDeviceName(string deviceName)
    {
        HideAll();
        
        deviceNamePanel.SetActive(true);
        deviceNameText.text = deviceName;
    }
    public void HideDeviceNamePanel()
    {
        deviceNamePanel.SetActive(false);
    }

    public void ShowPortName(string portName, int portValue)
    {
        HideAll();
        
        portNamePanel.SetActive(true);
        portNameText.text = portName;
        portValueText.text = $"{portValue}mV";
        
    }

    public void HidePortNamePanel()
    {
        portNamePanel.SetActive(false);
    }
    
    public void ShowStorageName(string storageName,  int currentStorageAmount, int maxCapacity, int storageMaxOutput)
    {
        HideAll();
        
        storageNamePanel.SetActive(true);
        storageNameText.text = storageName;
        storageCapacityText.text = $"{currentStorageAmount} / {maxCapacity}";
        storageMaxOutputText.text = $"Max output: {storageMaxOutput}";
    }

    public void HideStorageNamePanel()
    {
        storageNamePanel.SetActive(false);
    }
}
