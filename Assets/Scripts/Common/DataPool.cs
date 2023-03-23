using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ӿ��� ���Ǵ� ������ ���� Class�� �ڷᱸ������ �����ϴ� Class
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

    /// <summary> �� Stage���� �����ϴ� ����Ʈ(����)�� ���� </summary>
    public static short[] questNums = { 4, 4, 4, 1 };

    /// <summary> �� Stage���� �����ϴ� ������ ���� ���� </summary>
    public static short[] puzzleNums = { 4, 0, 0, 0 };

    /// <summary> �� ���ӿ� �����ϴ� ��� �������� ���� </summary>
    public static short itemNum = 1;

    /// <summary> ��� ���� ������ ���� </summary>
    public static short totalPuzzleNum;



    /// <summary> ����(Frame) ������ </summary>
    public static List<Frame> frames = new List<Frame>();

    /// <summary> ���� ����(Puzzle) ������ </summary>
    public static List<Item> puzzles = new List<Item>();

    /// <summary> ��Ż��� ������(Item) ������ </summary>
    public static List<Item> items = new List<Item>();

    /// <summary> ���� BGM(Background Music) ���� </summary>
    public static AudioClip[] BGMs;

    /// <summary> ���� SE(Sound Effect) ���� </summary>
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
/// �κ��丮�� ���� ������ ������ Class
/// </summary>
public class Item
{
    private static Sprite[][] sprites;
    private static string[][] names =
    {
        // ���� �̸�
        new string[]
        {
            "< ������ �� > �׸��� ù��° ����",  "< ������ �� > �׸��� �ι�° ����",  "< ������ �� > �׸��� ����° ����", "< ������ �� > �׸��� �׹�° ����",
            "< ����� ���� > �׸��� ù��° ����",  "< ����� ���� > �׸��� �ι�° ����",  "< ����� ���� > �׸��� ����° ����", "< ����� ���� > �׸��� ����° ����",
            "< ���ٷ� ���� > �׸��� ù��° ����",  "< ���ٷ� ���� > �׸��� �ι�° ����",  "< ���ٷ� ���� > �׸��� ����° ����", "< ���ٷ� ���� > �׸��� ����° ����",
            "< �ູ�� ���� > �׸��� ù��° ����",  "< �ູ�� ���� > �׸��� �ι�° ����",  "< �ູ�� ���� > �׸��� ����° ����", "< �ູ�� ���� > �׸��� ����° ����",
        },
        // ������ �̸�
        new string[]
        {
            "���ٷ� ����",
        },
    };

    public Sprite sprite;
    public string name;

    /// <summary> ���� ����������, ��Ż��� ���������� </summary>
    public ITEM_TYPE itemType;

    /// <summary> �� ��° ���������� (Index�� �ǹ�) </summary>
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
    /// Stage�� Index�� ���� Code�� ��ȯ�ϴ� �Լ� | ����: (Stage, Index)�� ����̸� (Code)�� ������ �迭�� ���̴� Index
    /// </summary>
    /// <param name="_stage">��������</param>
    /// <param name="_index">���������� Index��°�� �ǹ�</param>
    static public int PuzzleIndexToCode(int _stage, int _index)
    {
        int code = 0;
        for (int i = 0; i < _stage; i++)
            code += DataPool.puzzleNums[i];
        code += _index;

        return code;
    }

    /// <summary>
    /// Code�� Index�� ��ȯ�ϴ� �Լ� | ����: (Stage, Index)�� ����̸� (Code)�� ������ �迭�� ���̴� Index
    /// </summary>
    /// <param name="_puzzleCode">������ �迭 Code</param>
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
/// ���������� Ŭ�����ϱ� ���� ���� ������ Class
/// </summary>
public class Frame
{
    /// <summary> ���� ���ڸ� �̷�� ������� ���� </summary>
    public bool[] hasPuzzles;

    /// <summary> ���� ���ڰ� �ϼ��� ���� ���� ���� </summary>
    public bool isDone;

    /// <summary> �� ��° Stage �ΰ� </summary>
    public int stage;

    public Frame(int _stage)
    {
        stage = _stage;
        hasPuzzles = new bool[DataPool.puzzleNums[stage]];
    }

    /// <summary>
    /// ���� ���ڰ� �ϼ����� üũ�ϰ� ��ȯ�ϴ� �Լ�
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
    /// PuzzleCode�� �� ���ڿ� �´� �������� üũ�ϴ� �Լ�
    /// </summary>
    /// <param name="_puzzleCode">������ Code</param>
    public bool IsCorrectPuzzle(int _puzzleCode)
    {
        for (int i = 0; i < DataPool.puzzleNums[stage]; i++)
            if (_puzzleCode == Item.PuzzleIndexToCode(stage, i))
                return true;
        return false;
    }
}

/// <summary>
/// ��Ż�� ��Ҹ� �ذ��ϱ� ���� ���� ���� Event (Ż�� Sequence�� �̷�� �ϳ��� ���)
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
/// Quest - �ϳ��� ��Ż�� ��� (�پ��� Event���� �� �ϳ��� Quest�� ��)
/// </summary>
public class Quest
{
    public List<Event> events = new List<Event>();
    public int type; // �� ��° Stage�� ����Ʈ����
    public int code; // (type) ��°���� �� ��° ����Ʈ����

    public Quest(int _type, int _code)
    {
        type = _type;
        code = _code;
    }

    /// <summary>
    /// ���� Quest�� �Ϸ�ƴ����� ���� ���θ� ��ȯ�ϴ� �Լ�
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
