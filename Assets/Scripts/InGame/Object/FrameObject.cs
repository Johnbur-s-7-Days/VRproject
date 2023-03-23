using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FrameObject : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    [SerializeField] private GameObject[] puzzleObjects;
    
    // 액자(Frame) 종류를 나타내는 Code
    public int frameCode;

    // Temp Variable
    

    // Start is called before the first frame update
    void Start()
    {
        grabInteractable = this.GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
            grabInteractable = this.gameObject.AddComponent<XRGrabInteractable>();

        grabInteractable.trackPosition = grabInteractable.trackRotation = false;
        grabInteractable.selectEntered.AddListener(Select_Enter);

        SetPuzzleObject();
    }

    public void Select_Enter(SelectEnterEventArgs args)
    {
        Debug.Log("Select Enter.");
        if (!CheckPuzzle() || QuestCtrl.frames[frameCode].isDone)
            return;

        // 플레이어가 가진 모든 액자 퍼즐 탐색
        for (int i = 0; i < SaveCtrl.Instance.myData.hasPuzzles.Length; i++)
        {
            // 만약 가지고 있음 && 해당 액자에 맞는 퍼즐임
            if (SaveCtrl.Instance.myData.hasPuzzles[i] && DataPool.frames[frameCode].IsCorrectPuzzle(i))
            {
                int index = Item.PuzzleCodeToIndex(i);
                if (DataPool.frames[frameCode].hasPuzzles[index])
                    continue; // 이미 액자에 넣은 퍼즐임

                // 액자에 퍼즐을 추가하는 로직
                Debug.Log("Puzzling is Success. Puzzle Code : " + i);
                SaveCtrl.Instance.myData.hasPuzzles[i] = false;
                QuestCtrl.frames[frameCode].hasPuzzles[index] = true;
                QuestCtrl.frames[frameCode].CheckDone();
                InGameController.instance.SetAudio(0);
                SetPuzzleObject();
                return;
            }
        }

        Debug.Log("Puzzling is failed.");
        InGameController.instance.SetAudio(1);
    }

    /// <summary>
    /// 현재 액자의 퍼즐 상태를 Object로 표현하는 함수
    /// </summary>
    public void SetPuzzleObject()
    {
        for (int i = 0; i < DataPool.puzzleNums[frameCode]; i++)
        {
            if (QuestCtrl.frames[frameCode].hasPuzzles[i])
                puzzleObjects[i].SetActive(true);
            else
                puzzleObjects[i].SetActive(false);
        }
    }

    /// <summary>
    /// 선택한 아이템이 액자에 맞는 퍼즐인가? (미구현 - 추후에 사용자가 인벤토리에서 오브젝트를 꺼내 액자에 놓을 때 발동할 함수)
    /// </summary>
    private bool CheckPuzzle()
    {
        return true;
    }
}
