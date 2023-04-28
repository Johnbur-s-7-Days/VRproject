using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    public AudioSource audioSource;

    public EVENT_TYPE eventType;
    public int eventCode;
    public int se_index; // �浹 �� ����� SE ȿ���� (-1�̸� ȿ���� X)
    public GameObject[] eventObjects; // �浹 �� ��ȣ�ۿ��� ��ü
    public bool isDone;

    // Start is called before the first frame update
    void Start()
    {
        if (eventType == EVENT_TYPE.MAINEVENT)
        {
            switch (eventCode)
            {
                case 0:
                    PlaySE();
                    eventObjects[0].SetActive(true);
                    eventObjects[1].SetActive(false);
                    break;
                case 1:
                    eventObjects[0].SetActive(false);
                    break;
                case 3:
                    for (int i = 0; i < eventObjects.Length; i++)
                    {
                        eventObjects[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.white);
                    }
                    break;
            }
        }
    }

    private void PlaySE()
    {
        if (se_index == -1 || audioSource == null)
            return; // ��ȿ���� ó��
        audioSource.clip = DataPool.SEs[se_index];
        audioSource.Play();
    }

    private void SetEvent()
    {
        if (eventType == EVENT_TYPE.MAINEVENT)
        {
            switch (eventCode)
            {
                case 0:
                    eventObjects[0].SetActive(false);
                    eventObjects[1].SetActive(true);
                    break;
                case 1:
                    Invoke("PlaySE", 0.8f);
                    eventObjects[0].SetActive(true);
                    break;
                case 2:
                    PlaySE();
                    eventObjects[0].GetComponent<Door>().isOpen = false;
                    eventObjects[0].transform.rotation = Quaternion.Euler(Vector3.up * -180f);
                    break;
                case 3:
                    PlaySE();
                    for (int i = 0; i < eventObjects.Length; i++)
                        eventObjects[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color(0.8f, 0f, 0f));
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (isDone) return;
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Event trigger is on.");

            isDone = true;
            SetEvent();
        }
    }
}
