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

    // 게임의 모든 퀘스트 상태 (퀘스트 하나가 퍼즐이라고 생각)
    public static List<Quest>[] quests;

    // 게임의 모든 액자 상태
    public static List<Frame> frames;

    // 게임 이벤트 상태
    public static List<Event> events;

    // 현재 사용자가 위치한 StageCode
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
