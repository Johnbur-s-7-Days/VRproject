using SerializableCallback;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FrameObject : MonoBehaviour
{
    [SerializeField] private GameObject[] puzzleObjects;
    private bool[] hasPuzzles;
    
    // Start is called before the first frame update
    void Start()
    {
        hasPuzzles = new bool[DataPool.puzzleNum];
        SetPuzzleObject();
    }

    private void OnCollisionStay(Collision col)
    {
        if (col.transform.tag.Equals("Puzzle"))
        {
            ItemObject puzzle = col.transform.GetComponent<ItemObject>();
            if (puzzle == null)
                return;

            // ���ڿ� ������ �߰��ϴ� ����
            Debug.Log("���� ���������� �߰�, Puzzle Code : " + puzzle.itemCode);
            hasPuzzles[puzzle.itemCode] = false;
            Destroy(puzzle.gameObject);
            SetPuzzleObject();
            //MapCtrl.instance.SetAudio(0);

            QuestCtrl.instance.frame.hasPuzzles[puzzle.itemCode] = true;
            if (QuestCtrl.instance.frame.CheckDone())
            {
                // Clear
                MapCtrl.instance.StartEndAnim();
                Debug.Log("���� Ŭ����");
            }
        }
    }

    /// <summary>
    /// ���� ������ ���� ���¸� Object�� ǥ���ϴ� �Լ�
    /// </summary>
    public void SetPuzzleObject()
    {
        for (int i = 0; i < DataPool.puzzleNum; i++)
        {
            if (QuestCtrl.instance.frame.hasPuzzles[i])
                puzzleObjects[i].SetActive(true);
            else
                puzzleObjects[i].SetActive(false);
        }
    }
}
