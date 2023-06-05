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
            QuestCtrl.instance.frame.hasPuzzles[puzzle.itemCode] = true;
            hasPuzzles[puzzle.itemCode] = true;
            Destroy(puzzle.gameObject);
            SetPuzzleObject();
            //MapCtrl.instance.SetAudio(0);

            if (QuestCtrl.instance.frame.CheckDone())
            {
                // Clear
                MapCtrl.instance.StartEndAnim();
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
            if (hasPuzzles[i])
                puzzleObjects[i].SetActive(true);
            else
                puzzleObjects[i].SetActive(false);
        }
    }
}
