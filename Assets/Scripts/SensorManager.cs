using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class SensorManager : MonoBehaviour
{
    SerialPort arduino;
    public static SensorManager instance;
    
    private int bpmCount, tempCount;
    private int totalBPM, totalTEMP;
    private int h_rate,t_rate;

    public bool isReadingData {get; set;}
    public int HeartRate { get; private set; } 
    public int TempRate { get; private set; }

    public int HeartRate_Min { get; private set; }
    public int HeartRate_Max { get; private set; }
    public int TempRate_Min { get; private set; }
    public int TempRate_Max { get; private set ; }

    private void Awake()
    {
        if(SensorManager.instance == null)
            SensorManager.instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        arduino = new SerialPort("\\\\.\\COM3", 115200);
        arduino.Open();
        HeartRate_Min = 200;
        TempRate_Min = 200;
    }

    void Update()
    {
        StartCoroutine(ReadArduinoCouroutine());
    }

    private IEnumerator ReadArduinoCouroutine()
    {
        while (isReadingData)
        {
            if (arduino.IsOpen && arduino.BytesToRead > 0)
            {
                string data = arduino.ReadLine();
                string[] datas = data.Split(',');
                string BPM = datas[0];
                string TEMP = datas[3];
                int bmpvalue;
                float tempvalue;

                if (int.TryParse(BPM, out bmpvalue))
                {
                    if(bmpvalue > 30 && bmpvalue < 120)
                    {
                        totalBPM += bmpvalue; 
                        HeartRate_Max = Math.Max(HeartRate_Max,bmpvalue);
                        HeartRate_Min = Math.Min(HeartRate_Min,bmpvalue);
                        bpmCount++;
                    }
                    
                }

                if (float.TryParse(TEMP, out tempvalue))
                {
                    if(tempvalue > 0.0f && tempvalue < 50.0f)
                    {
                        totalTEMP += (int)tempvalue;
                        TempRate_Max = Math.Max(TempRate_Max,(int)tempvalue);
                        TempRate_Min = Math.Min(TempRate_Min,(int)tempvalue);
                        tempCount++;
                    }
                }
            }
            if (bpmCount > 0) HeartRate = totalBPM / bpmCount;
            if (tempCount > 0) TempRate = totalTEMP / tempCount;
            Debug.Log(HeartRate);

            yield return null;
        }

    }

}
