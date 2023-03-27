using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임에서 사용되는 데이터 전용 Class와 자료구조들을 관리하는 Class
/// </summary>
public class DataPool : MonoBehaviour
{
    private static DataPool instance;
    public static DataPool Instance
    {
        get
        {
            return instance;
        }
    }

    public static short eventNum = 99;

    /// <summary> 액자의 퍼즐 개수 </summary>
    public static short puzzleNum = 4;

    /// <summary> 액자 퍼즐(Puzzle) 데이터 </summary>
    public static List<Puzzle> puzzles = new List<Puzzle>();

    /// <summary> 게임 BGM(Background Music) 모음 </summary>
    public static AudioClip[] BGMs;

    /// <summary> 게임 SE(Sound Effect) 모음 </summary>
    public static AudioClip[] SEs;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            for (int i = 0; i < puzzleNum; i++)
                puzzles.Add(new Puzzle(i));

            SEs = Resources.LoadAll<AudioClip>("Audio/SE");
            BGMs = Resources.LoadAll<AudioClip>("Audio/BGM");
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}

/// <summary>
/// 인벤토리에 들어가는 아이템 데이터 Class
/// </summary>
public class Puzzle
{
    private static Sprite[] sprites;

    public Sprite sprite;

    /// <summary> 몇 번째 아이템인지 (Index를 의미) </summary>
    public int itemCode;

    public Puzzle(int _code)
    {
        if (sprites == null)
            sprites = Resources.LoadAll<Sprite>("Image/Sprite/Puzzle");

        itemCode = _code;
        // sprite = sprites[itemCode];
    }
}

/// <summary>
/// 스테이지를 클리어하기 위한 액자 데이터 Class
/// </summary>
public class Frame
{
    /// <summary> 현재 액자를 이루는 퍼즐들의 상태 </summary>
    public bool[] hasPuzzles;

    /// <summary> 현재 액자가 완성된 지에 대한 여부 </summary>
    public bool isDone;

    public Frame()
    {
        hasPuzzles = new bool[DataPool.puzzleNum];
    }

    /// <summary>
    /// 현재 액자가 완성된지 체크하고 반환하는 함수
    /// </summary>
    public bool CheckDone()
    {
        isDone = true;
        for (int i = 0; i < hasPuzzles.Length; i++)
            if (!hasPuzzles[i])
                isDone = false;
        return isDone;
    }
}

/// <summary>
/// 방탈출 요소를 해결하기 위한 아주 작은 Event (탈출 Sequence를 이루는 하나의 요소)
/// </summary>
public class Event
{
    public int code;
    public bool isOn;
    public bool isDone;

    public Event(int _code)
    {
        code = _code;
        isOn = true;
        isDone = false;
    }
}

public enum HAND_TYPE
{
    LEFT,
    RIGHT
}

public enum NPC_MODE
{
    IDLE,
    CHASE,
    RANDOM,
}

public enum LineType
{
    THIN,
    NORMAL,
    THICK
}

public enum LineCode
{
    IDLE,
    CLICKED_CANNOT,
    CLICKED_CAN
}
