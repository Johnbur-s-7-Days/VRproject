using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    private const float CAMERA_Y_IDLE = 0f;
    private const float CAMERA_Y_CROUCH = -1f;
    private const float MOVESPEED_IDLE = 2f;
    private const float MOVESPEED_SLOW = 0.5f;
    public const int HEARTRATE_GAP = 30;

    public static NPC currentNPC = null;

    public GameObject XROrigin;
    public new GameObject camera;
    // public Transform camera_offset;
    public Light flashLight;

    public int heartRate_midpoint; // 중간(시작) 심박수
    public int heartRate_minpoint, heartRate_maxpoint; // 최소 및 최대 심박수
    public int heartRate_current; // 현재 심박수

    private Vector3 moveVec;
    private float moveSpeed;
    private bool isSitDown;

    private float detectedDis;
    private bool isLockMove, isLockInteract;
    private bool isChangeHeartRate;
    private bool isFlashOn;

    // Temp Variables
    NPC npc;
    Door door;

    void Awake()
    {
        heartRate_current = 110;
        heartRate_midpoint = heartRate_current;
        heartRate_maxpoint = heartRate_midpoint + HEARTRATE_GAP;
        heartRate_minpoint = heartRate_midpoint - HEARTRATE_GAP;
    }

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = MOVESPEED_IDLE;
        detectedDis = 1.5f;
        isFlashOn = false;

        flashLight.gameObject.SetActive(isFlashOn);
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        XROrigin.transform.position = this.transform.position;
        flashLight.transform.position = camera.transform.position;
        flashLight.transform.rotation = Quaternion.Euler(camera.transform.rotation.eulerAngles);
        CheckNPC();

        if (!isChangeHeartRate)
            StartCoroutine("ChangeHaertRate");
    }

    public void OnMovementChanged(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        moveVec = new Vector3(direction.x, 0, direction.y);
    }

    public void OnSitDownChanged(InputAction.CallbackContext context)
    {
        isSitDown = !isSitDown;
    }

    public void OnFlashChanged(InputAction.CallbackContext context)
    {
        isFlashOn = !isFlashOn;
        flashLight.gameObject.SetActive(isFlashOn);
    }

    public void MoveCtrl()
    {
        if (isLockMove) return;

        transform.rotation = Quaternion.Euler(Vector3.up * camera.transform.rotation.eulerAngles.y);
        transform.position += (transform.right * moveVec.x + transform.forward * moveVec.z) * moveSpeed * Time.deltaTime;
    }

    /*
    public void SitDown()
    {
        if (isLockMove) return;
        if (isSitDown)
        {
            camera_offset.localPosition = new Vector3(0f, CAMERA_Y_CROUCH, 0f);
            moveSpeed = MOVESPEED_SLOW;
        }
        else
        {
            camera_offset.localPosition = new Vector3(0f, CAMERA_Y_IDLE, 0f);
            moveSpeed = MOVESPEED_IDLE;
        }
    }
    */

    public void CheckNPC()
    {
        // 전방에 NPC가 있는지 체크 
        RaycastHit npc_hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out npc_hit, detectedDis, 512))
        {
            // NPC가 있다면 이름 표시
            npc = npc_hit.collider.GetComponentInParent<NPC>();
            if (currentNPC != null && currentNPC != npc)
                currentNPC.nameTag.SetNameUI(false);
            currentNPC = npc;
            currentNPC.nameTag.SetNameUI(true);
        }
        else
        {
            // NPC가 없다면 이름 닫기
            if (currentNPC != null)
            {
                currentNPC.nameTag.SetNameUI(false);
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
