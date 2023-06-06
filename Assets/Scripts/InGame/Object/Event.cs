using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    public AudioSource audioSource;

    public EVENT_TYPE eventType;
    public int eventCode;
    public int se_index; // 충돌 시 출력할 SE 효과음 (-1이면 효과음 X)
    public GameObject[] eventObjects; // 충돌 시 상호작용할 물체
    public bool isDone;

    private int audioLoopCount;

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
                        eventObjects[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.white);
                    break;
                case 4:
                    eventObjects[0].SetActive(false);
                    eventObjects[1].SetActive(false);
                    break;
                case 6:
                    eventObjects[0].SetActive(false);
                    break;
            }
        }
        if (eventType == EVENT_TYPE.SUBEVENT)
        {
            switch (eventCode)
            {
                case 3:
                    eventObjects[0].SetActive(false);
                    break;
                case 7:
                    eventObjects[0].SetActive(false);
                    break;
            }
        }
    }

    private void PlaySE()
    {
        if (se_index == -1 || audioSource == null)
            return; // 무효과음 처리
        audioSource.clip = DataPool.SEs[se_index];
        audioSource.Play();
    }

    IEnumerator PlayTelephone()
    {
        PlaySE();
        if (++audioLoopCount == 3)
            yield break;

        yield return new WaitForSeconds(5f);

        StartCoroutine(PlayTelephone());
    }

    public void SetEvent()
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
                case 4:
                    PlaySE();
                    eventObjects[0].SetActive(true);
                    eventObjects[1].SetActive(true);
                    break;
                case 5:
                    eventObjects[0].SetActive(false);
                    break;
                case 6:
                    PlaySE();
                    eventObjects[0].SetActive(true);
                    break;
            }
        }
        else if (eventType == EVENT_TYPE.SUBEVENT)
        {
            switch (eventCode)
            {
                case 0:
                    audioLoopCount = 0;
                    StartCoroutine(PlayTelephone());
                    break;
                case 1:
                    for (int i = 0; i < eventObjects.Length; i++)
                    {
                        eventObjects[i].GetComponent<Animation>().Play("WhiteChair");
                        eventObjects[i].GetComponent<AudioSource>().Play();
                    }
                    break;
                case 2:
                    PlaySE();
                    break;
                case 3:
                    PlaySE();
                    eventObjects[0].SetActive(true);
                    break;
                case 4:
                    Invoke("PlaySE", 0.7f);
                    eventObjects[0].GetComponent<Rigidbody>().AddForce(Vector3.forward * 3f, ForceMode.Impulse);
                    eventObjects[0].GetComponent<Rigidbody>().AddTorque(Vector3.right * 180f);
                    break;
                case 5:
                    PlaySE();
                    break;
                case 6:
                    eventObjects[0].GetComponent<AudioSource>().Play();
                    eventObjects[1].GetComponent<AudioSource>().Play();
                    break;
                case 7:
                    eventObjects[0].SetActive(true);
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
