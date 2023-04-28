// 핸드트래킹을 감지하여 사용자 기능을 동작하도록 하는 스크립트이다.
// 예를 들어, 이동 및 플래시 키기/끄기 기능이 있다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Gesture
{
    public string name;
    public List<Vector3> fingerDatas;
    public UnityEvent onRecognized;
}

public class GestureDetector : MonoBehaviour
{
    const float threshold = 0.05f;
    const float opendoor_listTime = 0.5f;
    const float armShaking_listTime = 0.5f;
    const float armShaking_checkTime = 0.15f;
    const float opendoor_checkTime = 0.2f;
    const float armShakingY_min = 0.1f;
    const float armShakingZ_min = 0.0025f;
    const float opendoor_dis = 0.05f;

    public OVRSkeleton leftSkeleton, rightSkeleton;
    public HandTrigger leftHand, rightHand;
    public List<Gesture> gestures;
    public bool debugMode = true;
    private List<OVRBone> leftFingerBones, rightFingerBones;
    private Gesture previousRightGesture;
    private Gesture previousLeftGesture;
    [SerializeField] private bool isOpenDoor_left, isOpenDoor_right; // 모션에 대한 여부일 뿐, 실행에 대한 여부는 아님


    private float armShakingSpeed;
    [SerializeField] private bool isArmShaking;


    [SerializeField] private GameObject leftArm, rightArm;
    private List<Vector3> leftArmVecs = new List<Vector3>();
    private List<Vector3> leftArmVecs_grip = new List<Vector3>(); // Grip 시에만 데이터 저장
    private List<Vector3> rightArmVecs = new List<Vector3>();
    private List<Vector3> rightArmVecs_grip = new List<Vector3>(); // Grip 시에만 데이터 저장

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("FindFingerBones");
        previousRightGesture = new Gesture();
        previousLeftGesture = new Gesture();
    }

    private void FixedUpdate()
    {
        if (isArmShaking)
        {
            // Move Forward!
            float moveSpeed = Mathf.Lerp(2f, 3f, (armShakingSpeed - 0.075f) * 5f);
            PlayerCtrl.instance.MoveCtrl(moveSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (leftFingerBones == null || rightFingerBones == null || leftFingerBones.Count == 0 || rightFingerBones.Count == 0)
            return; // 감지 불가능 상태

        if (debugMode)
        {
            // Gesture Save!
            if (Input.GetKeyDown(KeyCode.F1))
                Save(HAND_TYPE.LEFT);
            else if (Input.GetKeyDown(KeyCode.F2))
                Save(HAND_TYPE.RIGHT);
        }

        Gesture currentLeftGesture = Recognize(HAND_TYPE.LEFT);
        Gesture currentRightGesture = Recognize(HAND_TYPE.RIGHT);
        if (!isArmShaking && (currentLeftGesture.name == "Left_Grip" || currentRightGesture.name == "Right_Grip"))
            SetGripArmLists();
        else
        {
            leftArmVecs_grip.Clear();
            rightArmVecs_grip.Clear();
        }
        SetArmLists();

        // Open Door
        if (!isArmShaking)
        {
            if (isOpenDoor_left && currentLeftGesture.name == "Left_Grip" && leftHand.isTriggerOn)
                leftHand.door.OpenDoor();
            if (isOpenDoor_right && currentRightGesture.name == "Right_Grip" && rightHand.isTriggerOn)
                rightHand.door.OpenDoor();
        }

        // Check New Gesture (Default = RightGesuter)
        bool hasRightRecognized = !currentRightGesture.Equals(new Gesture());
        bool hasLeftRecognized = !currentLeftGesture.Equals(new Gesture());

        if ((hasRightRecognized && !currentRightGesture.Equals(previousRightGesture)) || (hasLeftRecognized && !currentLeftGesture.Equals(previousLeftGesture)))
        {
            // New Gesture!
            bool isInvoke = true;
            Debug.Log("New Right Gesture Found : " + currentRightGesture.name);
            Debug.Log("New Left Gesture Found : " + currentLeftGesture.name);

            // Function is not work!
            if (currentRightGesture.name == "Flash_1" || currentRightGesture.name == "Flash_2" || currentRightGesture.name == "Right_Grip")
                isInvoke = false;

            // Function is work!
            if (currentRightGesture.name == "Flash_2" && (previousRightGesture.name == "Flash_1" || previousRightGesture.name == "Right_Grip"))
                isInvoke = CheckFlashOnOff();

            previousRightGesture = currentRightGesture;
            previousLeftGesture = currentLeftGesture;

            if (isInvoke)
            {
                currentRightGesture.onRecognized.Invoke();
            }
        }

    }

    IEnumerator CheckOpenDoor()
    {
        bool leftArmIsOpen = false, rightArmIsOpen = false;
        float maxX, minX;
        float maxZ, minZ;

        if (leftArmVecs_grip.Count > 0)
        {
            // Setting Left Arm Data
            maxX = minX = leftArmVecs_grip[0].y;
            maxZ = minZ = leftArmVecs_grip[0].z;
            for (int i = 0; i < leftArmVecs_grip.Count; i++)
            {
                Vector3 vec = leftArmVecs_grip[i];
                if (minX > vec.y) minX = vec.y;
                if (maxX < vec.y) maxX = vec.y;
                if (minZ > vec.z) minZ = vec.z;
                if (maxZ < vec.z) maxZ = vec.z;
            }

            // Check Left Arm OpenDoor
            if (Vector2.Distance(new Vector2(minX, minZ), new Vector2(maxX, maxZ)) > opendoor_dis)
                leftArmIsOpen = true;
        }

        if (rightArmVecs_grip.Count > 0)
        {
            // Setting Right Arm Data
            maxX = minX = rightArmVecs_grip[0].y;
            maxZ = minZ = rightArmVecs_grip[0].z;
            for (int i = 0; i < rightArmVecs_grip.Count; i++)
            {
                Vector3 vec = rightArmVecs_grip[i];
                if (minX > vec.y) minX = vec.y;
                if (maxX < vec.y) maxX = vec.y;
                if (minZ > vec.z) minZ = vec.z;
                if (maxZ < vec.z) maxZ = vec.z;
            }

            // Check Right Arm OpenDoor
            if (Vector2.Distance(new Vector2(minX, minZ), new Vector2(maxX, maxZ)) > opendoor_dis)
                rightArmIsOpen = true;
        }

        isOpenDoor_left = leftArmIsOpen;
        isOpenDoor_right = rightArmIsOpen;

        yield return new WaitForSeconds(opendoor_checkTime);
        StartCoroutine("CheckOpenDoor");
    }

    private bool CheckFlashOnOff()
    {
        if (isArmShaking)
            return false;

        bool isOn = false;
        Vector3 lookVec = PlayerCtrl.instance.camera.transform.forward.normalized;
        Vector3 planeNormal = Vector3.Cross(Vector3.up, lookVec).normalized;
        Vector3 armVec = Vector3.ProjectOnPlane(rightArm.transform.up, planeNormal).normalized;

        if (Vector3.Dot(armVec, lookVec) > 0f)
            isOn = true;

        return isOn;
    }


    private void SetGripArmLists()
    {
        if (leftArm == null || rightArm == null)
        {
            Debug.LogError("Error: No arm is found!");
            return;
        }

        leftArmVecs_grip.Add(leftArm.transform.localPosition);
        rightArmVecs_grip.Add(rightArm.transform.localPosition);

        while (leftArmVecs_grip.Count > opendoor_listTime / Time.deltaTime)
            leftArmVecs_grip.RemoveAt(0);
        while (rightArmVecs_grip.Count > opendoor_listTime / Time.deltaTime)
            rightArmVecs_grip.RemoveAt(0);
    }

    private void SetArmLists()
    {
        if (leftArm == null || rightArm == null)
        {
            Debug.LogError("Error: No arm is found!");
            return;
        }

        leftArmVecs.Add(leftArm.transform.localPosition);
        rightArmVecs.Add(rightArm.transform.localPosition);

        while (leftArmVecs.Count > armShaking_listTime / Time.deltaTime)
            leftArmVecs.RemoveAt(0);
        while (rightArmVecs.Count > armShaking_listTime / Time.deltaTime)
            rightArmVecs.RemoveAt(0);
    }

    IEnumerator CheckArmShaking()
    {
        bool leftArmShaking = false, rightArmShaking = false;
        float maxY, minY;
        float maxZ, minZ;
        int leftMinY_index = 0, rightMinY_index = 0;

        if (leftArmVecs.Count > 0)
        {
            // Setting Left Arm Data
            minY = maxY = leftArmVecs[0].y;
            maxZ = minZ = leftArmVecs[0].z;
            for (int i = 0; i < leftArmVecs.Count; i++)
            {
                Vector3 vec = leftArmVecs[i];
                if (minY > vec.y) { minY = vec.y; leftMinY_index = i; }
                if (maxY < vec.y) maxY = vec.y;
                if (minZ > vec.z) minZ = vec.z;
                if (maxZ < vec.z) maxZ = vec.z;
            }

            // Check Left Arm Shaking
            if (Mathf.Abs(maxY - minY) > armShakingY_min && Mathf.Abs(maxZ - minZ) > armShakingZ_min)
                leftArmShaking = true;
        }

        if (rightArmVecs.Count > 0)
        {
            // Setting Right Arm Data
            minY = maxY = rightArmVecs[0].y;
            maxZ = minZ = rightArmVecs[0].z;
            for (int i = 0; i < rightArmVecs.Count; i++)
            {
                Vector3 vec = rightArmVecs[i];
                if (minY > vec.y) { minY = vec.y; rightMinY_index = i; }
                if (maxY < vec.y) maxY = vec.y;
                if (minZ > vec.z) minZ = vec.z;
                if (maxZ < vec.z) maxZ = vec.z;
            }

            // Check Right Arm Shaking
            if (Mathf.Abs(maxY - minY) > armShakingY_min && Mathf.Abs(maxZ - minZ) > armShakingZ_min)
                rightArmShaking = true;
        }

        armShakingSpeed = 1f / Mathf.Abs(leftMinY_index - rightMinY_index);
        isArmShaking = leftArmShaking && rightArmShaking && Mathf.Abs(leftMinY_index - rightMinY_index) > 0.1f / Time.deltaTime;

        yield return new WaitForSeconds(armShaking_checkTime);
        StartCoroutine("CheckArmShaking");
    }

    IEnumerator FindFingerBones()
    {
        leftFingerBones = new List<OVRBone>(leftSkeleton.Bones);
        while (leftFingerBones.Count == 0)
        {
            leftFingerBones = new List<OVRBone>(leftSkeleton.Bones);
            Debug.Log("Left Finding finger bones is failed.");
            yield return null;
        }

        rightFingerBones = new List<OVRBone>(rightSkeleton.Bones);
        while (rightFingerBones.Count == 0)
        {
            rightFingerBones = new List<OVRBone>(rightSkeleton.Bones);
            Debug.Log("Right Finding finger bones is failed.");
            yield return null;
        }

        Debug.Log("Finding finger bones is done.");
        StartCoroutine("CheckArmShaking");
        StartCoroutine("CheckOpenDoor");
    }
    
    private void Save(HAND_TYPE type)
    {
        Gesture g = new Gesture();
        g.name = "New Gesture";
        List<Vector3> data = new List<Vector3>();
        switch (type)
        {
            case HAND_TYPE.LEFT:
                foreach (var bone in leftFingerBones)
                {
                    data.Add(leftSkeleton.transform.InverseTransformPoint(bone.Transform.position));
                }
                break;
            case HAND_TYPE.RIGHT:
                foreach (var bone in rightFingerBones)
                {
                    data.Add(rightSkeleton.transform.InverseTransformPoint(bone.Transform.position));
                }
                break;
        }

        g.fingerDatas = data;
        gestures.Add(g);
        Debug.Log("Creating new Gesture is Success.");
    }

    private Gesture Recognize(HAND_TYPE type)
    {
        Gesture currentGesture = new Gesture();
        float currentMin = Mathf.Infinity;

        foreach (var gesture in gestures)
        {
            float sumDistance = 0;
            bool isDiscarded = false;
            switch (type)
            {
                case HAND_TYPE.LEFT:
                    for (int i = 0; i < leftFingerBones.Count; i++)
                    {
                        Vector3 currentData = leftSkeleton.transform.InverseTransformPoint(leftFingerBones[i].Transform.position);
                        float distance = Vector3.Distance(currentData, gesture.fingerDatas[i]);
                        if (distance > threshold)
                        {
                            isDiscarded = true;
                            break;
                        }

                        sumDistance += distance;
                    }
                    break;
                case HAND_TYPE.RIGHT:
                    for (int i = 0; i < rightFingerBones.Count; i++)
                    {
                        Vector3 currentData = rightSkeleton.transform.InverseTransformPoint(rightFingerBones[i].Transform.position);
                        float distance = Vector3.Distance(currentData, gesture.fingerDatas[i]);
                        if (distance > threshold)
                        {
                            isDiscarded = true;
                            break;
                        }

                        sumDistance += distance;
                    }
                    break;
            }

            if (!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentGesture = gesture;
            }
        }

        return currentGesture;
    }
}
