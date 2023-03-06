using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
//using System.IO.Ports;

public class PulseSensorMonitor : MonoBehaviour
{
    // Start is called before the first frame update

    //SerialPort arduino = new SerialPort("COM3", 11520);

    public string data;
    public string bpm;
    public int bpmnum = 0;
    string[] words;

    float delayTime = 15.0f;
    float repeatTime = 1f;

    void Start()
    {
        //arduino.Open();
        InvokeRepeating("RepeatCalculate", delayTime, repeatTime);
    }

    void RepeatCalculate()
    {
        

    }


    // Update is called once per frame
    void Update()
    {
        //data = arduino.ReadLine();
        //words = data.Split(',');
        //bpm = words[0];
        

    }

}
