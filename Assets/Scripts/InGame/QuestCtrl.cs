using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestCtrl : MonoBehaviour
{
    private static QuestCtrl Instance; 
    public static QuestCtrl instance
    {
        set
        {
            if (Instance == null)
                Instance = value;
        }
        get
        {
            return Instance;
        }
    }

    // ������ ���� ����
    public Frame frame;

    // ���� �̺�Ʈ ����
    public List<Event> events;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        frame = new Frame();
        events = new List<Event>();
        for (int i = 0; i < DataPool.eventNum; i++)
            events.Add(new Event(i));
    }
}
