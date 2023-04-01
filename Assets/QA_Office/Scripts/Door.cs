using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
	public enum DoorType
    {
		LEFT,
		RIGHT
    }

	private Animation anim;
	public DoorType doorType;
	public float OpenSpeed;
	public bool isOpen;
	private string OpenForwardAnimName;
	private string OpenBackwardAnimName;
	private string _animName;
	private Vector3 relativePos;

	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animation>();
		_animName = anim.clip.name;
		OpenSpeed = 1f;
		OpenForwardAnimName = "DoorForward_Open";
		OpenBackwardAnimName = "DoorBackward_Open";

		isOpen = false;
	}

	public void OpenDoor()
	{
		if (isOpen)
			return;

		relativePos = gameObject.transform.InverseTransformPoint(PlayerCtrl.instance.transform.position);
		if (relativePos.z > 0)
		{
			if (doorType == DoorType.LEFT)
				_animName = OpenForwardAnimName;
			else
				_animName = OpenBackwardAnimName;
		}
		else
		{
			if (doorType == DoorType.LEFT)
				_animName = OpenBackwardAnimName;
			else
				_animName = OpenForwardAnimName;
		}

		Debug.Log(doorType + " / Open");
		isOpen = true;
		anim[_animName].speed = 1 * OpenSpeed;
		anim[_animName].normalizedTime = 0;
		anim.Play(_animName);
	}
}
