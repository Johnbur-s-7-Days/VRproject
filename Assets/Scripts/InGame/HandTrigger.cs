using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrigger : MonoBehaviour
{
    public bool isTriggerOn;
    private Door door;

    private void OnTriggerEnter(Collider col)
    {
        isTriggerOn = true;

        if (col.tag == "Door")
        {
            door = col.transform.parent.GetComponent<Door>();
            door.OpenDoor();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        isTriggerOn = false;
        door = null;
    }
}
