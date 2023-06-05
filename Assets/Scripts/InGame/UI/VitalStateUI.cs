using System.Globalization;
using TMPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VitalStateUI : MonoBehaviour
{
    public TextMeshProUGUI heartratetxt;
    public TextMeshProUGUI tempratetxt;

    [SerializeField]
    private GameObject vitalUI;

    [SerializeField]
    private GameObject targetHand;

    // Start is called before the first frame update
    void Start()
    {
        heartratetxt.text = " ";
        tempratetxt.text = " ";
    }

    void Update()
    {
        heartratetxt.text = SensorManager.HeartRate.ToString();
        tempratetxt.text = SensorManager.TempRate.ToString();
    }

    public void VitalVisibility(bool state)
    {
        vitalUI.SetActive(state);
    }
}
