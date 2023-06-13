using System.Globalization;
using TMPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VitalStateUI : MonoBehaviour
{
    public TextMeshProUGUI hearttxt;
    public TextMeshProUGUI temptxt;

    [SerializeField]
    private GameObject vitalUI;
    public SensorManager sensorData;

    // Start is called before the first frame update
    void Start()
    {
        hearttxt.text = " ";
        temptxt.text = " ";
        sensorData.isReadingData = false;
    }

    void Update()
    {
        if(vitalUI.activeSelf)
        {
            sensorData.isReadingData = true;
            hearttxt.text = sensorData.HeartRate.ToString();
            temptxt.text = sensorData.TempRate.ToString();
        }
        else sensorData.isReadingData = false;
        
    }
    



}
