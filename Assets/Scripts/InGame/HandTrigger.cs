using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrigger : MonoBehaviour
{
    public bool isTriggerOn;
    private Door door;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        isTriggerOn = true;

        if (col.tag == "Door")
        {

        }
    }

    private void OnTriggerExit(Collider col)
    {
        isTriggerOn = false;
    }

    public Door GetDoor()
    {
        return door;
    }
}
