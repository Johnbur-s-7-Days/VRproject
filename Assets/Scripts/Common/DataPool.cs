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

    public static short stageNum = 4;
    public static short eventNum = 99;

    /// <summary> 각 Stage마다 존재하는 퀘스트(퍼즐)의 개수 </summary>
    public static short[] questNums = { 4, 4, 4, 1 };

    /// <summary> 각 Stage마다 존재하는 액자의 퍼즐 개수 </summary>
    public static short[] puzzleNums = { 4, 0, 0, 0 };

    /// <summary> 이 게임에 존재하는 모든 아이템의 개수 </summary>
    public static short itemNum = 1;

    /// <summary> 모든 액자 퍼즐의 개수 </summary>
    public static short totalPuzzleNum;



    /// <summary> 액자(Frame) 데이터 </summary>
    public static List<Frame> frames = new List<Frame>();

    /// <summary> 액자 퍼즐(Puzzle) 데이터 </summary>
    public static List<Item> puzzles = new List<Item>();

    /// <summary> 방탈출용 아이템(Item) 데이터 </summary>
    public static List<Item> items = new List<Item>();

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

            totalPuzzleNum = 0;
            for (int i = 0; i < puzzleNums.Length; i++)
                totalPuzzleNum += puzzleNums[i];

            for (int i = 0; i < stageNum; i++)
                frames.Add(new Frame(i));
            for (int i = 0; i < totalPuzzleNum; i++)
                puzzles.Add(new Item(ITEM_TYPE.PUZZLE, i));
            for (int i = 0; i < itemNum; i++)
                items.Add(new Item(ITEM_TYPE.ETC, i));

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
public class Item
{
    private static Sprite[][] sprites;
    private static string[][] names =
    {
        // 퍼즐 이름
        new string[]
        {
            "< 마녀의 집 > 그림의 첫번째 조각",  "< 마녀의 집 > 그림의 두번째 조각",  "< 마녀의 집 > 그림의 세번째 조각", "< 마녀의 집 > 그림의 네번째 조각",
            "< 헤라의 정원 > 그림의 첫번째 조각",  "< 헤라의 정원 > 그림의 두번째 조각",  "< 헤라의 정원 > 그림의 세번째 조각", "< 헤라의 정원 > 그림의 세번째 조각",
            "< 빛바랜 오후 > 그림의 첫번째 조각",  "< 빛바랜 오후 > 그림의 두번째 조각",  "< 빛바랜 오후 > 그림의 세번째 조각", "< 빛바랜 오후 > 그림의 세번째 조각",
            "< 행복한 죽음 > 그림의 첫번째 조각",  "< 행복한 죽음 > 그림의 두번째 조각",  "< 행복한 죽음 > 그림의 세번째 조각", "< 행복한 죽음 > 그림의 세번째 조각",
        },
        // 아이템 이름
        new string[]
        {
            "빛바랜 열쇠",
        },
    };

    public Sprite sprite;
    public string name;

    /// <summary> 퍼즐 아이템인지, 방탈출용 아이템인지 </summary>
    public ITEM_TYPE itemType;

    /// <summary> 몇 번째 아이템인지 (Index를 의미) </summary>
    public int itemCode;

    public Item(ITEM_TYPE _type, int _code)
    {
        if (sprites == null)
        {
            sprites = new Sprite[2][];
            sprites[0] = Resources.LoadAll<Sprite>("Image/Sprite/Puzzle");
            sprites[1] = Resources.LoadAll<Sprite>("Image/Sprite/Item");
        }

        itemType = _type;
        itemCode = _code;
        name = names[(int)itemType][itemCode];
        // sprite = sprites[(int)itemType][itemCode];
    }

    /// <summary>
    /// Stage와 Index를 통해 Code를 반환하는 함수 | 참고: (Stage, Index)는 행렬이며 (Code)는 일차원 배열에 쓰이는 Index
    /// </summary>
    /// <param name="_stage">스테이지</param>
    /// <param name="_index">스테이지의 Index번째를 의미</param>
    static public int PuzzleIndexToCode(int _stage, int _index)
    {
        int code = 0;
        for (int i = 0; i < _stage; i++)
            code += DataPool.puzzleNums[i];
        code += _index;

        return code;
    }

    /// <summary>
    /// Code를 Index로 반환하는 함수 | 참고: (Stage, Index)는 행렬이며 (Code)는 일차원 배열에 쓰이는 Index
    /// </summary>
    /// <param name="_puzzleCode">일차원 배열 Code</param>
    static public int PuzzleCodeToIndex(int _puzzleCode)
    {
        int index = _puzzleCode;
        int stage = 0;

        while (index >= DataPool.puzzleNums[stage])
        {
            index -= DataPool.puzzleNums[stage];
            stage++;
        }

        return index;
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

    /// <summary> 몇 번째 Stage 인가 </summary>
    public int stage;

    public Frame(int _stage)
    {
        stage = _stage;
        hasPuzzles = new bool[DataPool.puzzleNums[stage]];
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

    /// <summary>
    /// PuzzleCode가 이 액자에 맞는 퍼즐인지 체크하는 함수
    /// </summary>
    /// <param name="_puzzleCode">퍼즐의 Code</param>
    public bool IsCorrectPuzzle(int _puzzleCode)
    {
        for (int i = 0; i < DataPool.puzzleNums[stage]; i++)
            if (_puzzleCode == Item.PuzzleIndexToCode(stage, i))
                return true;
        return false;
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

/// <summary>
/// Quest - 하나의 방탈출 요소 (다양한 Event들이 모여 하나의 Quest가 됨)
/// </summary>
public class Quest
{
    public List<Event> events = new List<Event>();
    public int type; // 몇 번째 Stage의 퀘스트인지
    public int code; // (type) 번째에서 몇 번째 퀘스트인지

    public Quest(int _type, int _code)
    {
        type = _type;
        code = _code;
    }

    /// <summary>
    /// 현재 Quest가 완료됐는지에 대한 여부를 반환하는 함수
    /// </summary>
    public bool GetSuccess()
    {
        for (int i = 0; i < events.Count; i++)
            if (!events[i].isDone)
                return false;
        return true;
    }
}

public enum GameTime
{
    AFTERNOON,
    NIGHT,
    END,
}

public enum ITEM_TYPE
{
    PUZZLE,
    ETC
}

public enum NPC_MODE
{
    IDLE,
    CHASE,
    RANDOM,
    DIALOG
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
