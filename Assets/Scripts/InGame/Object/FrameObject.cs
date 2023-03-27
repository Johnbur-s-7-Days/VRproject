using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FrameObject : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    [SerializeField] private GameObject[] puzzleObjects;
    
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
        if (!CheckPuzzle() || QuestCtrl.instance.frame.isDone)
            return;

        // �÷��̾ ���� ��� ���� ���� Ž��
        for (int i = 0; i < PlayerCtrl.instance.hasPuzzles.Length; i++)
        {
            // ���� ������ ���� 
            if (PlayerCtrl.instance.hasPuzzles[i])
            {
                if (QuestCtrl.instance.frame.hasPuzzles[i])
                    continue; // �̹� ���ڿ� ���� ������

                // ���ڿ� ������ �߰��ϴ� ����
                Debug.Log("Puzzling is Success. Puzzle Code : " + i);
                PlayerCtrl.instance.hasPuzzles[i] = false;
                QuestCtrl.instance.frame.hasPuzzles[i] = true;
                QuestCtrl.instance.frame.CheckDone();
                MapCtrl.instance.SetAudio(0);
                SetPuzzleObject();
                return;
            }
        }

        Debug.Log("Puzzling is failed.");
        MapCtrl.instance.SetAudio(1);
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

    /// <summary>
    /// ������ �������� ���ڿ� �´� �����ΰ�? (�̱��� - ���Ŀ� ����ڰ� �κ��丮���� ������Ʈ�� ���� ���ڿ� ���� �� �ߵ��� �Լ�)
    /// </summary>
    private bool CheckPuzzle()
    {
        return true;
    }
}
