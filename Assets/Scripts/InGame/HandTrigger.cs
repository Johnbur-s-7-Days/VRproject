using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrigger : MonoBehaviour
{
    public bool isTriggerOn;
    public Door door;

    private void OnTriggerStay(Collider col)
    {
        isTriggerOn = true;
        if (col.tag == "Door")
        {
            if (door == null)
                door = col.transform.parent.GetComponent<Door>();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        isTriggerOn = false;
        door = null;
    }
}
