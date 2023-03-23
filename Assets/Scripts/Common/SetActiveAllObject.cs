using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveAllObject : MonoBehaviour
{
    private GameObject[] allofScene;

    void Awake()
    {
        allofScene = Resources.FindObjectsOfTypeAll<GameObject>();
        for (int i = 0; i < allofScene.Length; i++)
            allofScene[i].SetActive(true);
    }
}
