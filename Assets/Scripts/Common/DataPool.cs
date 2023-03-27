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

    public static short eventNum = 99;

    /// <summary> ������ ���� ���� </summary>
    public static short puzzleNum = 4;

    /// <summary> ���� ����(Puzzle) ������ </summary>
    public static List<Puzzle> puzzles = new List<Puzzle>();

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
/// �κ��丮�� ���� ������ ������ Class
/// </summary>
public class Puzzle
{
    private static Sprite[] sprites;

    public Sprite sprite;

    /// <summary> �� ��° ���������� (Index�� �ǹ�) </summary>
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
/// ���������� Ŭ�����ϱ� ���� ���� ������ Class
/// </summary>
public class Frame
{
    /// <summary> ���� ���ڸ� �̷�� ������� ���� </summary>
    public bool[] hasPuzzles;

    /// <summary> ���� ���ڰ� �ϼ��� ���� ���� ���� </summary>
    public bool isDone;

    public Frame()
    {
        hasPuzzles = new bool[DataPool.puzzleNum];
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
