using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCtrl : MonoBehaviour
{
    private static MapCtrl Instance;
    public static MapCtrl instance
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

    public new AudioSource audio;

    public GameObject directLightParent;
    public GameObject mapParent;
    public GameObject npcParent;
    public Light directLight;
    private List<GameObject> allLights = new List<GameObject>();
    private List<ReflectionProbe> reflections = new List<ReflectionProbe>();

    /// <summary>
    /// 게임 속 가상 시간 (AFTERNOON_TIME = 아침 시작, NIGHT_TIME = 밤 시작, END_TIME = 하루 일과 종료)
    /// </summary>
    public float time;

    // Temp 변수들
    GameObject[] gameObjects;
    ReflectionProbe reflection;
    Light light;

    private void Awake()
    {
        instance = this;
        gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i].name.Contains("light") || gameObjects[i].name.Contains("Light"))
                allLights.Add(gameObjects[i]);
            reflection = gameObjects[i].GetComponent<ReflectionProbe>();
            if (reflection != null)
                reflections.Add(reflection);
        }

        audio = this.GetComponent<AudioSource>();
        if (audio == null)
            audio = this.gameObject.AddComponent<AudioSource>();

        Debug.Log("총 " + allLights.Count + " 개의 빛 감지");
        Debug.Log("총 " + reflections.Count + " 개의 반사경 감지");
        for (int i = 0; i < allLights.Count; i++)
            allLights[i].gameObject.SetActive(true);
        for (int i = 0; i < reflections.Count; i++)
        {
            reflections[i].enabled = true;
            if (reflections[i].GetComponent<Light>() == null)
            {
                light = reflections[i].gameObject.AddComponent<Light>();
                light.intensity = 0.25f;
                light.type = LightType.Point;
            }
        }
    }

    /// <summary>
    /// 효과음을 출력하는 함수
    /// </summary>
    /// <param name="_SE_index">몇 번째 효과음인지에 대한 Code</param>
    public void SetAudio(int _SE_index)
    {
        audio.clip = DataPool.SEs[_SE_index];
        audio.Play();
    }
}
