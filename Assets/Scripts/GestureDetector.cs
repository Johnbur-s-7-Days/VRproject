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
    const float threshold = 0.1f;
    const float checkArmTime = 0.5f;

    public OVRSkeleton skeleton;
    public List<Gesture> gestures;
    public bool debugMode = true;
    [SerializeField] private bool isArmShaking;
    private List<OVRBone> fingerBones;
    private List<Vector3> leftArmVecs = new List<Vector3>();
    private List<Vector3> rightArmVecs = new List<Vector3>();
    private Gesture previousGesture;

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
            InGameController.instance.playerCtrl.MoveCtrl();
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

        Gesture currentGesture = Recognize();
        // bool hasRecognized = !currentGesture.Equals(new Gesture());
        bool hasRecognized = false;
        // Check New Gesture
        if (hasRecognized && !currentGesture.Equals(previousGesture))
        {
            // New Gesture!
            Debug.Log("New Gesture Found : " + currentGesture.name);
            previousGesture = currentGesture;
            currentGesture.onRecognized.Invoke();
        }
    }

    IEnumerator CheckArmShaking()
    {
        bool armShaking = false;
        float time = 0f;
        bool_list.Clear();

        // Collect Datas
        while (time < checkArmTime)
        {
            bool_list.Add(skeleton.IsDataHighConfidence);
            time += Time.deltaTime;
            yield return null;
        }

        // Check Datas
        if (bool_list.Count != 0)
        {
            bool isDisable = false, isAnable = false;
            foreach (var isOn in bool_list)
            {
                if (isOn)
                    isDisable = true;
                else
                    isAnable = true;

                if (isDisable && isAnable)
                {
                    armShaking = true;
                    break;
                }
            }
        }

        isArmShaking = armShaking;
        Debug.Log("ÆÈ Èçµé¸² »óÅÂ: " + isArmShaking);
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
        // StartCoroutine("CheckArmShaking");
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
