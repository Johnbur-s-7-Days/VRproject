using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_Section : MonoBehaviour
{
    private BoxCollider col;
    private AudioSource audioSource;
    private Event @event;

    public int eventCode;
    public int se_index; // 충돌 시 출력할 SE 효과음 (-1이면 효과음 X)
    public GameObject eventObject; // 충돌 시 상호작용할 물체

    // Start is called before the first frame update
    void Start()
    {
        col = this.GetComponentInChildren<BoxCollider>();
        audioSource = this.GetComponentInChildren<AudioSource>();

        @event = QuestCtrl.events[eventCode];
        if (eventObject != null)
            eventObject.SetActive(false);
        SetSection(true);
    }

    public void SetSection(bool _isOn)
    {
        col.enabled = _isOn;
    }

    private void PlaySE()
    {
        if (se_index == -1 || audioSource == null)
            return; // 무효과음 처리
        audioSource.clip = DataPool.SEs[se_index];
        audioSource.Play();
    }

    private void SetEventObject()
    {
        if (eventObject == null)
            return; // 상호작용 미처리

        switch (eventCode)
        {
            case 0:
                eventObject.SetActive(true);
                break;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (@event == null || !@event.isOn) return;
        if (col.gameObject.tag == "Player")
        {
            if (@event == null)
            {
                Debug.LogError("Event is null.");
                return;
            }
            Debug.Log("Event trigger is on.");

            @event.isDone = true;
            PlaySE();
            SetEventObject();
            SetSection(false);

            /*
            code = InGameController.instance.questCtrl.GetDoneQuestCode();
            if (code != -1)
            {
                // 완료된 퀘스트가 있습니다.

            }
            */
        }
    }
}
