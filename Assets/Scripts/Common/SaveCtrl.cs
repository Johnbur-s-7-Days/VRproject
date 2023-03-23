using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public bool[] hasPuzzles;
    public bool[] hasItems;

    public SaveData()
    {
        hasPuzzles = new bool[DataPool.totalPuzzleNum];
        hasItems = new bool[DataPool.itemNum];
    }
}

public class SaveCtrl : MonoBehaviour
{
    private static SaveCtrl instance;
    public static SaveCtrl Instance
    {
        get
        {
            return instance;
        }
    }

    public SaveData myData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            myData = new SaveData();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
