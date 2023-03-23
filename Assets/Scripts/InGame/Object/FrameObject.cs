using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FrameObject : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    [SerializeField] private GameObject[] puzzleObjects;
    
    // ����(Frame) ������ ��Ÿ���� Code
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

        // �÷��̾ ���� ��� ���� ���� Ž��
        for (int i = 0; i < SaveCtrl.Instance.myData.hasPuzzles.Length; i++)
        {
            // ���� ������ ���� && �ش� ���ڿ� �´� ������
            if (SaveCtrl.Instance.myData.hasPuzzles[i] && DataPool.frames[frameCode].IsCorrectPuzzle(i))
            {
                int index = Item.PuzzleCodeToIndex(i);
                if (DataPool.frames[frameCode].hasPuzzles[index])
                    continue; // �̹� ���ڿ� ���� ������

                // ���ڿ� ������ �߰��ϴ� ����
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
    /// ���� ������ ���� ���¸� Object�� ǥ���ϴ� �Լ�
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
    /// ������ �������� ���ڿ� �´� �����ΰ�? (�̱��� - ���Ŀ� ����ڰ� �κ��丮���� ������Ʈ�� ���� ���ڿ� ���� �� �ߵ��� �Լ�)
    /// </summary>
    private bool CheckPuzzle()
    {
        return true;
    }
}
