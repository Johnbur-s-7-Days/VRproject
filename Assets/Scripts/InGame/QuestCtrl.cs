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

    // ������ ��� ����Ʈ ���� (����Ʈ �ϳ��� �����̶�� ����)
    public static List<Quest>[] quests;

    // ������ ��� ���� ����
    public static List<Frame> frames;

    // ���� �̺�Ʈ ����
    public static List<Event> events;

    // ���� ����ڰ� ��ġ�� StageCode
    public short currentStage;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        quests = new List<Quest>[DataPool.stageNum];
        for (int i = 0; i < quests.Length; i++)
        {
            quests[i] = new List<Quest>();
            for (int j = 0; j < DataPool.questNums[i]; j++)
                quests[i].Add(new Quest(i, j));
        }

        frames = new List<Frame>();
        for (int i = 0; i < DataPool.stageNum; i++)
            frames.Add(new Frame(i));

        events = new List<Event>();
        for (int i = 0; i < DataPool.eventNum; i++)
            events.Add(new Event(i));
    }
}
