using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerTrigger : MonoBehaviour
{
    private const float DELAY_TIME = 1f;

    private static MeshRenderer elevator_mesh;
    private static Elevator elevator;
    private static string elevator_floor;
    private static bool isTrigger;

    IEnumerator SetDelay()
    {
        isTrigger = true;
        yield return new WaitForSeconds(DELAY_TIME);
        isTrigger = false;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (isTrigger)
            return;

        if (col.tag == "ElevatorNumericButton")
        {
            elevator = col.transform.parent.GetComponent<Elevator>();
            elevator_mesh = col.GetComponent<MeshRenderer>();
            elevator_floor = col.transform.name;
            elevator.OnElevatorNumber(elevator_floor, elevator_mesh);
        }
        else if (col.tag == "ElevatorGoButton")
        {
            elevator = col.transform.parent.GetComponent<Elevator>();
            elevator_mesh = col.GetComponent<MeshRenderer>();
            elevator.OnElevatorGo(elevator_mesh);
        }
        else if (col.tag == "ElevatorButtonOpen")
        {
            elevator = col.transform.parent.GetComponent<Elevator>();
            elevator_mesh = col.GetComponent<MeshRenderer>();
            elevator.OnElevatorOpen(elevator_mesh);
        }

        StartCoroutine("SetDelay");
    }
}
