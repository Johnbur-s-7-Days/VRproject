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

    private const string endGame_anim = "End_Anim";

    public GameObject directLightParent;
    public GameObject mapParent;
    public Light directLight;
    public Animation anim;
    private List<GameObject> allLights = new List<GameObject>();
    private List<ReflectionProbe> reflections = new List<ReflectionProbe>();

    // Temp 변수들
    GameObject[] gameObjects;
    ReflectionProbe reflection;
    new Light light;

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
        StartEndAnim();
    }

    public void StartEndAnim()
    {
        anim.Play(endGame_anim);
    }

    public void EndGame()
    {
        Debug.Log("End Game");
        Application.Quit();
    }
}
