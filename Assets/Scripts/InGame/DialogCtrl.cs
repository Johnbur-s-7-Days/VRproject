using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog
{
    private string[] dialogs; // 대사들
    private int index; // 현재 몇 번째 대사까지 진행했는 지에 대한 Offset

    public string GetDialog()
    {
        return dialogs[index++];
    }
} 


public class DialogCtrl : MonoBehaviour
{
    private static DialogCtrl instance;
    public static DialogCtrl Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
