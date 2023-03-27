using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    private const float MOVESPEED_IDLE = 2f;
    public const int HEARTRATE_GAP = 30;

    private static PlayerCtrl Instance;
    public static PlayerCtrl instance
    {
        get
        {
            return Instance;
        }
        set
        {
            if (Instance == null)
                Instance = value;
        }
    }
    public static NPC currentNPC = null;

    public new GameObject camera;
    public HandTrigger leftHand;
    public HandTrigger rightHand;
    // public Transform camera_offset;
    public Light flashLight;
    public bool[] hasPuzzles;

    public int heartRate_midpoint; // 중간(시작) 심박수
    public int heartRate_minpoint, heartRate_maxpoint; // 최소 및 최대 심박수
    public int heartRate_current; // 현재 심박수

    private float moveSpeed;

    private float detectedDis;
    private bool isLockMove, isLockInteract;
    private bool isChangeHeartRate;
    private bool isFlashOn;

    // Temp Variables
    NPC npc;
    Door door;

    void Awake()
    {
        instance = this;
        heartRate_current = 110;
        heartRate_midpoint = heartRate_current;
        heartRate_maxpoint = heartRate_midpoint + HEARTRATE_GAP;
        heartRate_minpoint = heartRate_midpoint - HEARTRATE_GAP;
    }

    // Start is called before the first frame update
    void Start()
    {
        hasPuzzles = new bool[DataPool.puzzleNum];
        moveSpeed = MOVESPEED_IDLE;
        detectedDis = 1.5f;
        isFlashOn = false;

        flashLight.gameObject.SetActive(isFlashOn);
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        flashLight.transform.position = camera.transform.position;
        flashLight.transform.rotation = Quaternion.Euler(camera.transform.rotation.eulerAngles);
        CheckNPC();

        if (!isChangeHeartRate)
            StartCoroutine("ChangeHaertRate");
    }

    public void MoveCtrl()
    {
        if (isLockMove) return;

        transform.position += camera.transform.forward * moveSpeed * Time.deltaTime;
    }

    public void FlashOnOff()
    {
        isFlashOn = !isFlashOn;
        flashLight.gameObject.SetActive(isFlashOn);
    }

    public void CheckNPC()
    {
        // 전방에 NPC가 있는지 체크 
        RaycastHit npc_hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out npc_hit, detectedDis, 512))
        {
            // NPC가 있다면 이름 표시
            npc = npc_hit.collider.GetComponentInParent<NPC>();
            currentNPC = npc;
        }
        else
        {
            // NPC가 없다면 이름 닫기
            if (currentNPC != null)
            {
                currentNPC = null;
            }
        }
    }

    public void Interact()
    {
        if (isLockInteract) return;

        // 전방에 Door
        RaycastHit door_hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out door_hit, detectedDis, 1024))
        {
            // Door가 있다면 ON/OFF
            door = door_hit.collider.GetComponentInParent<Door>();
            door.Player_Interact();
        }
    }

    IEnumerator ChangeHaertRate()
    {
        isChangeHeartRate = true;
        if (heartRate_current >= heartRate_maxpoint - 10)
            heartRate_current += Random.Range(-4, 0);
        else if (heartRate_current <= heartRate_minpoint + 10)
            heartRate_current += Random.Range(0, 5);
        else
            heartRate_current += Random.Range(-3, 4);

        yield return new WaitForSeconds(Random.Range(0f, 0.5f));

        isChangeHeartRate = false;
    }
}
