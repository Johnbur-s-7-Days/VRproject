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
    public float threshold = 0.1f;
    public OVRSkeleton skeleton;
    public List<Gesture> gestures;
    public bool debugMode = true;
    private List<OVRBone> fingerBones;
    private Gesture previousGesture;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("FindFingerBones");
        previousGesture = new Gesture();
    }

    private void FixedUpdate()
    {
        if (CheckArmShaking())
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

    

    private bool CheckArmShaking()
    {
        bool isArmShaking = false;

        

        return isArmShaking;
    }
}
