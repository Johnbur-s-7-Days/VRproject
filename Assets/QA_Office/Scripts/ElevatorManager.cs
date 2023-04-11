using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElevatorManager : MonoBehaviour {

	public int InitialFloor = 1;
	public UnityAction WasStarted;
	[HideInInspector]
	public int _floor;
}
