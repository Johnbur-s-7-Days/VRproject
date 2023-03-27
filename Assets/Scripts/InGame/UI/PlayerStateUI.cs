using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStateUI : MonoBehaviour
{
    private const float MOVESPEED = 3f;
    private static PlayerStateUI instance;
    public static PlayerStateUI Instance
    {
        get
        {
            return instance;
        }
    }

    public Image fillImage;
    public TMP_Text heartRateText;

    public bool isOn;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;

        isOn = true;
        fillImage.fillAmount = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOn) 
            return; // 동작 하지 않음

        ShowHeartRate();
    }

    private void ShowHeartRate()
    {
        float gap = ((PlayerCtrl.instance.heartRate_current - PlayerCtrl.instance.heartRate_minpoint) / (PlayerCtrl.HEARTRATE_GAP * 2f)) - fillImage.fillAmount;
        fillImage.fillAmount += gap * MOVESPEED * Time.deltaTime;
        heartRateText.text = PlayerCtrl.instance.heartRate_current.ToString();
    }
}
