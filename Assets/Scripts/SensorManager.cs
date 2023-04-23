using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;


public class SensorManager : MonoBehaviour
{

    SerialPort arduino = new SerialPort("\\\\.\\COM5", 115200);
    private string data;
    public static int HeartRate;
    public static float TempRate;
    public static int HeartRate_Min;
    public static int HeartRate_Max;
    public static float TempRate_Min;
    public static float TempRate_Max;


    // Start is called before the first frame update
    void Start()
    {
        arduino.Open();
        HeartRate = 0;
        TempRate = 0;
        HeartRate_Min = 200;
        HeartRate_Max = 50;
        TempRate_Min = 40.0f;
        TempRate_Max = 10.0f;
    }


    void Update()
    {
        data = arduino.ReadLine();
        string[] datas = data.Split(',');
        string BPM = datas[0];
        string TEMP = datas[3];

        HeartRate = 0;
        TempRate = 0.0f;

        for (int i = 0; i < BPM.Length; i++)
        {
            HeartRate = HeartRate * 10 + (BPM[i] - '0');
        }

        TempRate = float.Parse(TEMP);

        //MIN MAX값 설정
        if (HeartRate >= 50 && HeartRate < 140)
        {
           if(HeartRate_Min > HeartRate)
               HeartRate_Min = HeartRate;
           if(HeartRate_Max < HeartRate)
                HeartRate_Max = HeartRate;
        }
        else
        {
            Debug.Log("센서 위치 재조정 바람.");
            HeartRate = 0;
        }

        if (TempRate_Min > TempRate)
            TempRate_Min = TempRate;
        if (TempRate_Max < TempRate)
            TempRate_Max = TempRate;



    }
}
