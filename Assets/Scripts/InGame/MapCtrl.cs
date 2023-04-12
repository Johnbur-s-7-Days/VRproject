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
    /// ���� �� ���� �ð� (AFTERNOON_TIME = ��ħ ����, NIGHT_TIME = �� ����, END_TIME = �Ϸ� �ϰ� ����)
    /// </summary>
    public float time;

    // Temp ������
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

        Debug.Log("�� " + allLights.Count + " ���� �� ����");
        Debug.Log("�� " + reflections.Count + " ���� �ݻ�� ����");
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
    /// ȿ������ ����ϴ� �Լ�
    /// </summary>
    /// <param name="_SE_index">�� ��° ȿ���������� ���� Code</param>
    public void SetAudio(int _SE_index)
    {
        audio.clip = DataPool.SEs[_SE_index];
        audio.Play();
    }
}
