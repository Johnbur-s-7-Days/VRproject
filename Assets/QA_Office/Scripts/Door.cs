using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
	private Animation anim;
	public float OpenSpeed = 1;
	public float CloseSpeed = 1;
	public bool isAutomatic;
	public bool AutoClose;
	public bool DoubleSidesOpen;
	public string PlayerHeadTag = "NPC";
	public string OpenForwardAnimName = "Door_anim";
	public string OpenBackwardAnimName = "DoorBack_anim";
	private string _animName;
	private bool inTrigger;
	private bool isOpen;
	private Vector3 relativePos;
	private bool isInteract;
	private bool isButtonDelay;

	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animation>();
		_animName = anim.clip.name;
		isAutomatic = false;
		AutoClose = true;
		DoubleSidesOpen = false;
		inTrigger = false;
		isOpen = false;
		isInteract = false;
		isButtonDelay = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (inTrigger == true)
		{
			if (isInteract)
			{
				isInteract = false;
				if (!isOpen)
				{
					isOpen = true;
					OpenDoor();
				}
				else
				{
					isOpen = false;
					CloseDoor();
				}
			}
		}
	}

	public void Player_Interact()
	{
		if (isButtonDelay)
			return;

		inTrigger = true;
		isInteract = true;
		StartCoroutine(OnButtonDelay());
	}

	IEnumerator OnButtonDelay()
	{
		isButtonDelay = true;
		yield return new WaitForSeconds(1f);
		isButtonDelay = false;
	}

	void OpenDoor()
	{
		anim[_animName].speed = 1 * OpenSpeed;
		anim[_animName].normalizedTime = 0;
		anim.Play(_animName);

	}
	void CloseDoor()
	{
		anim[_animName].speed = -1 * CloseSpeed;
		if (anim[_animName].normalizedTime > 0)
		{
			anim[_animName].normalizedTime = anim[_animName].normalizedTime;
		}
		else
		{
			anim[_animName].normalizedTime = 1;
		}
		anim.Play(_animName);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag == PlayerHeadTag)
		{
			if (DoubleSidesOpen)
			{
				relativePos = gameObject.transform.InverseTransformPoint(other.transform.position);
				if (relativePos.z > 0)
				{
					_animName = OpenForwardAnimName;
				}
				else
				{
					_animName = OpenBackwardAnimName;
				}
			}
			if (isAutomatic)
			{
				OpenDoor();
			}

			inTrigger = true;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.transform.tag == PlayerHeadTag)
		{
			if (isAutomatic)
			{
				CloseDoor();
			}
			else
			{
				inTrigger = false;
			}
			if (AutoClose && isOpen)
			{
				CloseDoor();
				inTrigger = false;
				isOpen = false;
			}
		}
	}
}
