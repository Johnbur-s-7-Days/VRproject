using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    public const int HEARTRATE_GAP = 30;
    private const int FLASH_FLICKER_TIME_MIN = 3;
    private const int FLASH_FLICKER_TIME_MAX = 5;

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
    public Rigidbody rigid;
    public Animator flashAnimator;
    public bool[] hasPuzzles;

    private float detectedDis;
    private bool isLockMove, isLockInteract;
    private bool isFlashOn, isFlashFlicking;

    // Temp Variables
    NPC npc;
    Door door;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        hasPuzzles = new bool[DataPool.puzzleNum];
        detectedDis = 1.5f;
        isFlashOn = isFlashFlicking = false;

        flashAnimator.SetBool("isOn", isFlashOn);
        flashAnimator.SetBool("isFliker", isFlashFlicking);
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        flashAnimator.transform.position = camera.transform.position;
        flashAnimator.transform.rotation = Quaternion.Euler(camera.transform.rotation.eulerAngles);
        CheckNPC();
    }

    public void MoveCtrl(float moveSpeed)
    {
        if (isLockMove) return;

        Vector3 moveVec = new Vector3(camera.transform.forward.x, 0f, camera.transform.forward.z);
        rigid.MovePosition(this.transform.position + moveVec * moveSpeed * Time.deltaTime);
    }

    public void SetFlash(bool _isOn)
    {
        isFlashOn = _isOn;
        flashAnimator.SetBool("isOn", isFlashOn);
    }

    public void FlashOn()
    {
        SetFlash(true);
        if (!isFlashFlicking)
            StartCoroutine("FlickerFlash");
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

    IEnumerator FlickerFlash()
    {
        isFlashFlicking = true;

        yield return new WaitForSeconds(Random.Range(FLASH_FLICKER_TIME_MIN, FLASH_FLICKER_TIME_MAX));

        flashAnimator.SetBool("isFliker", true);
        Invoke("Flash_Off", 2f);
    }

    public void Flash_Off()
    {
        SetFlash(false);
        flashAnimator.SetBool("isFliker", false);
        isFlashFlicking = false;
    }
}
