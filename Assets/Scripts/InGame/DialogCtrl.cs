using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog
{
    private string[] dialogs; // ����
    private int index; // ���� �� ��° ������ �����ߴ� ���� ���� Offset

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
