using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;

public class NPC : MonoBehaviour
{
    public new AudioSource audio;
    public bool isRunning;

    private void Start()
    {
        if ((audio = this.GetComponent<AudioSource>()) == null)
            audio = this.gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (isRunning)
        {
            this.transform.position += Vector3.forward * 6f * Time.deltaTime;
            if (CheckContact())
                Invoke("SetDisable", 1.5f);
        }
        else
        {
            if (CheckContact())
            {
                audio.clip = DataPool.SEs[2];
                audio.Play();
                Invoke("SetDisable", 1f);
            }
        }
    }

    private void SetDisable()
    {
        this.gameObject.SetActive(false);
    }

    private bool CheckContact()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(this.transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
            return true;
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isRunning)
            return;

        if (collision.transform.tag == "Player")
        {
            audio.clip = DataPool.SEs[2];
            audio.Play();
            this.gameObject.SetActive(false);
        }
    }
}
