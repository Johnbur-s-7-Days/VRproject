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
    const float armShaking_listTime = 0.5f;
    const float armShaking_checkTime = 0.1f;
    const float armShakingY_min = 0.1f;
    const float armShakingZ_min = 0.0025f;

    public OVRSkeleton skeleton;
    public List<Gesture> gestures;
    public bool debugMode = true;
    private List<OVRBone> fingerBones;
    private Gesture previousGesture;

    [SerializeField] private bool isArmShaking;
    [SerializeField] private GameObject leftArm, rightArm;
    private List<Vector3> leftArmVecs = new List<Vector3>();
    private List<Vector3> rightArmVecs = new List<Vector3>();

    // Temp Variables
    List<bool> bool_list = new List<bool>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("FindFingerBones");
        previousGesture = new Gesture();
    }

    private void FixedUpdate()
    {
        if (isArmShaking)
        {
            // Move Forward!
            PlayerCtrl.instance.MoveCtrl();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (debugMode && Input.GetKeyDown(KeyCode.Space))
        {
            // Gesture Save!
            Save();
        }

        SetArmLists();
        Gesture currentGesture = Recognize();
        bool hasRecognized = !currentGesture.Equals(new Gesture());
        // Check New Gesture
        if (hasRecognized && !currentGesture.Equals(previousGesture))
        {
            // New Gesture!
            bool isInvoke = true;
            Debug.Log("New Gesture Found : " + currentGesture.name);

            // Function is not work!
            if (currentGesture.name == "Flash_1" || (currentGesture.name == "Flash_2" && previousGesture.name != "Flash_1"))
                isInvoke = false;

            // Function is work!
            if (currentGesture.name == "Flash_2" && previousGesture.name == "Flash_1")
                isInvoke = CheckFlashOnOff();

            previousGesture = currentGesture;
            if (isInvoke)
                currentGesture.onRecognized.Invoke();
        }
    }

    private bool CheckFlashOnOff()
    {
        bool isOn = false;
        Vector3 lookVec = PlayerCtrl.instance.camera.transform.forward.normalized;
        Vector3 planeNormal = Vector3.Cross(Vector3.up, lookVec).normalized;
        Vector3 armVec = Vector3.ProjectOnPlane(rightArm.transform.up, planeNormal).normalized;

        Debug.Log("손가락 튕기기 내적 : " + Vector3.Dot(armVec, lookVec));
        if (Vector3.Dot(armVec, lookVec) > 0f)
            isOn = true;

        return isOn;
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

        isArmShaking = leftArmShaking && rightArmShaking && Mathf.Abs(leftMinY_index - rightMinY_index) > 0.1f / Time.deltaTime;

        yield return new WaitForSeconds(armShaking_checkTime);
        StartCoroutine("CheckArmShaking");
    }

    IEnumerator FindFingerBones()
    {
        fingerBones = new List<OVRBone>(skeleton.Bones);
        while (fingerBones.Count == 0)
        {
            fingerBones = new List<OVRBone>(skeleton.Bones);
            Debug.Log("Finding finger bones is failed.");
            yield return null;
        }
        Debug.Log("Finding finger bones is done.");
        StartCoroutine("CheckArmShaking");
    }
    
    private void Save()
    {
        Gesture g = new Gesture();
        g.name = "New Gesture";
        List<Vector3> data = new List<Vector3>();
        foreach (var bone in fingerBones)
        {
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }

        g.fingerDatas = data;
        gestures.Add(g);
        Debug.Log("Creating new Gesture is Success.");
    }

    private Gesture Recognize()
    {
        Gesture currentGesture = new Gesture();
        float currentMin = Mathf.Infinity;

        foreach (var gesture in gestures)
        {
            float sumDistance = 0;
            bool isDiscarded = false;
            for (int i = 0; i < fingerBones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData, gesture.fingerDatas[i]);
                if (distance > threshold)
                {
                    isDiscarded = true;
                    break;
                }

                sumDistance += distance;
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
