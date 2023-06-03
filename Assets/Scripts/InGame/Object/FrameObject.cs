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

            // 액자에 퍼즐을 추가하는 로직
            Debug.Log("퍼즐 성공적으로 추가, Puzzle Code : " + puzzle.itemCode);
            hasPuzzles[puzzle.itemCode] = false;
            Destroy(puzzle.gameObject);
            SetPuzzleObject();
            //MapCtrl.instance.SetAudio(0);

            QuestCtrl.instance.frame.hasPuzzles[puzzle.itemCode] = true;
            if (QuestCtrl.instance.frame.CheckDone())
            {
                // Clear
                MapCtrl.instance.StartEndAnim();
                Debug.Log("게임 클리어");
            }
        }
    }

    /// <summary>
    /// 현재 액자의 퍼즐 상태를 Object로 표현하는 함수
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
