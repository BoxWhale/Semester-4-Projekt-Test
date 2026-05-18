using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ElectricUI : MonoBehaviour
{
    public static ElectricUI instance;
    void Awake()
    {
        instance = this;
        if (BumperButton != null)
        {
            _bumperColor = BumperButton.GetComponent<Renderer>().material.color;
            _buttonBColor = ButtonB.GetComponent<Renderer>().material.color;
        }
    }
    
    void OnEnable()
    {
        if (TutorialAction.action != null)
        {
            TutorialAction.action.performed += TutorialSwitch;
            TutorialAction.action.Enable();
        }
    }
    public InputActionProperty TutorialAction;
    public int tutorialStage = 1;
    
    [Header("Tutorial Displays")]
    public Color tutorialHighlightColor = Color.red;
    [Space(10)]
    public GameObject tutorialPanelBumper;
    public TMP_Text tutorialTextBumper;
    public GameObject BumperButton;
    private Color _bumperColor;
    [Space(10)]
    public GameObject tutorialPanelButtonB;
    public TMP_Text tutorialTextButtonB;
    public GameObject ButtonB;
    private Color _buttonBColor;

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
        HideTutorialBumperPanel();
        HideTutorialButtonB();
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
        storageCapacityText.text = $"{currentStorageAmount}mV / {maxCapacity}mV";
        storageMaxOutputText.text = $"Max output: {storageMaxOutput}mV";
    }

    public void HideStorageNamePanel()
    {
        storageNamePanel.SetActive(false);
    }

    public void ShowTutorialBumperPanel()
    {
        tutorialPanelBumper.SetActive(true);
    }

    public void HideTutorialBumperPanel()
    {
        tutorialPanelBumper.SetActive(false);
    }

    public void ShowTutorialButtonB()
    {
        tutorialPanelButtonB.SetActive(true);
    }

    public void HideTutorialButtonB()
    {
        tutorialPanelButtonB.SetActive(false);
    }
    
    public void TutorialSwitch(InputAction.CallbackContext context)
    {
        tutorialStage++;
        switch (tutorialStage)
        {
            case 1:
                HideAll();
                ReturnColors();
                ShowTutorialBumperPanel();
                break;
            case 2:
                HideAll();
                ReturnColors();
                ShowTutorialButtonB();
                break;
            default:
                HideAll();
                ReturnColors();
                tutorialStage = 0; // Needs to be at the end case
                return;
        }
    }

    void Update()
    {
        float pulse = Mathf.PingPong(Time.time, 1f);
        switch (tutorialStage)
        {
            case 1:
                Color newColorBumper = Color.Lerp(_bumperColor, tutorialHighlightColor, pulse);
                BumperButton.GetComponent<Renderer>().material.color = newColorBumper;
                break;
            case 2:
                Color newColorButtonB =  Color.Lerp(_bumperColor, tutorialHighlightColor, pulse);
                ButtonB.GetComponent<Renderer>().material.color = newColorButtonB;
                break;
        }
    }

    private void ReturnColors()
    {
        BumperButton.GetComponent<Renderer>().material.color = _bumperColor;
        ButtonB.GetComponent<Renderer>().material.color = _buttonBColor;
    }
}


