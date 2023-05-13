using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    private const int FLASH_FLICKER_TIME_MIN = 15;
    private const int FLASH_FLICKER_TIME_MAX = 30;

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

    public new GameObject camera;
    public new AudioSource audio;
    public Rigidbody rigid;
    public Animator flashAnimator;
    public bool[] hasPuzzles;

    private bool isLockMove;
    private bool isFlashOn, isFlashFlicking;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        hasPuzzles = new bool[DataPool.puzzleNum];
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
