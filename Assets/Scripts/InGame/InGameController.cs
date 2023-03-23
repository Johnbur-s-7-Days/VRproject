using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameController : MonoBehaviour
{
    private const int AFTERNOON_TIME = 60 * 60 * 12; // AM 12:00 (�� ���� �ð�)
    private const int NIGHT_TIME = 60 * 60 * 18; // PM 6:00 (�� ���� �ð�)
    private const int END_TIME = 60 * 60 * 24; // PM 12:00 (�Ϸ� ���� �ð�)
    private const int AFTERNOON_PLAYTIME = 60 * 3; // ���� �÷��� �ð�
    private const int NIGHT_PLAYTIME = 60 * 3; // ���� �÷��� �ð�
    private const float SKYBOX_EXPOSUTE_MAX = 3.0f;
    private const float SKYBOX_EXPOSUTE_MIN = 0.1f;
    private const float DIRECT_INTENSITY_MAX = 1.0f;
    private const float DIRECT_INTENSITY_MIN = 0.1f;
    private const float TIME_PASS_SPEED = 1f;
    private const float SKYBOX_ROTATION_SPEED = 0.2f;

    private static InGameController Instance;
    public static InGameController instance
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

    public PlayerCtrl playerCtrl;
    public QuestCtrl questCtrl;
    public ObjectPool objectPool;

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

    private GameTime gameTime;
    private bool isGameStart;

    // Temp ������
    GameObject[] gameObjects;
    NPC[] npcs;
    ReflectionProbe reflection;

    private void Awake()
    {
        instance = this;
        npcs = npcParent.GetComponentsInChildren<NPC>();
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

        /*
        isGameStart = true;
        gameTime = GameTime.AFTERNOON;
        time = 0f;
        */

        isGameStart = false;
        gameTime = GameTime.NIGHT;
        time = AFTERNOON_PLAYTIME + NIGHT_PLAYTIME;

        SetLight(gameTime, time);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStart)
        {
            time += Time.deltaTime * TIME_PASS_SPEED;
            SetGameTime(time);
            SetLight(gameTime, time);
        }

        // �ӽ� ��� ���(Ű �Է�)
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (time < NIGHT_PLAYTIME)
            {
                time = NIGHT_PLAYTIME;
                StartCoroutine(SceneCtrl.instance.BGMCtrl.switchBGM(2, 1.5f, 1.5f));
                for (int i = 0; i < npcs.Length; i++)
                    npcs[i].mode = NPC_MODE.CHASE;
            }
            else
            {
                time = AFTERNOON_PLAYTIME;
                StartCoroutine(SceneCtrl.instance.BGMCtrl.switchBGM(1, 1.5f, 1.5f));
                for (int i = 0; i < npcs.Length; i++)
                    npcs[i].mode = NPC_MODE.RANDOM;
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

    private void SetGameTime(float _time)
    {
        if (_time < AFTERNOON_PLAYTIME)
            gameTime = GameTime.AFTERNOON;
        else if (_time < AFTERNOON_PLAYTIME + NIGHT_PLAYTIME)
            gameTime = GameTime.NIGHT;
        else
            gameTime = GameTime.END;
    }

    private void SetLight(GameTime gameTime, float _time)
    {
        float rate = 0f;
        float skybox_intensity = 0f;
        float skybox_rotation = _time * SKYBOX_ROTATION_SPEED;
        float direct_intensity = 0f;

        switch (gameTime)
        {
            case GameTime.AFTERNOON:
                rate = _time / AFTERNOON_PLAYTIME;
                skybox_intensity = Mathf.Lerp(SKYBOX_EXPOSUTE_MAX, SKYBOX_EXPOSUTE_MIN, rate);
                direct_intensity = Mathf.Lerp(DIRECT_INTENSITY_MAX, DIRECT_INTENSITY_MIN, rate);
                break;
            case GameTime.NIGHT:
                skybox_intensity = SKYBOX_EXPOSUTE_MIN;
                direct_intensity = DIRECT_INTENSITY_MIN;
                // ȸ�� �� ��� �� ����
                if (Mathf.RoundToInt(_time) == NIGHT_PLAYTIME)
                {
                    Debug.Log("�� " + allLights.Count + " ���� �� ����");
                    for (int i = 0; i < allLights.Count; i++)
                        allLights[i].gameObject.SetActive(false);
                    for (int i = 0; i < reflections.Count; i++)
                        reflections[i].enabled = false;
                }
                break;
            case GameTime.END:
                skybox_intensity = SKYBOX_EXPOSUTE_MIN;
                direct_intensity = DIRECT_INTENSITY_MIN;
                break;
        }

        RenderSettings.skybox.SetFloat("_Exposure", skybox_intensity);
        RenderSettings.skybox.SetFloat("_Rotation", skybox_rotation);
        directLight.intensity = direct_intensity;
        directLightParent.transform.rotation = Quaternion.Euler(new Vector3(0f, skybox_rotation, 0f));
        DynamicGI.UpdateEnvironment();
    }
}
