using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneCtrl : MonoBehaviour
{
    static public SceneCtrl instance;
    static public bool isEndChange;

    public BGMCtrl BGMCtrl;
    public Animator animator;

    public Image blindImage;
    private int currentSceneIndex;
    private float fadeTime_default;

    /*****************************************
    0. LobbyScene 
    1. InGameScene
    *****************************************/

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
            fadeTime_default = 1.5f;

            StartCoroutine(FadeInit());
            BGMCtrl.BGMSetting(0, false, 1f);
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if (isEndChange)
        {
            if (currentSceneIndex == 0) // LobbyScene
            {
                if (LobbyCtrl.isGameStart)
                {
                    currentSceneIndex = 1;
                    StartCoroutine(switchScene(currentSceneIndex, 1, fadeTime_default, fadeTime_default));
                    LobbyCtrl.isGameStart = false;
                }
            }
            else if (currentSceneIndex == 1) // InGameScene
            {

            }
        }
    }

    IEnumerator FadeInit()
    {
        FadeOut();
        yield return new WaitForSeconds(fadeTime_default);
        FadeEnd();
    }

    public void FadeIn()
    {
        animator.SetBool("isFadeOn", true);
        animator.SetBool("isFadeOff", false);
        blindImage.raycastTarget = true;
        isEndChange = false;
    }

    public void FadeOut()
    {
        animator.SetBool("isFadeOn", false);
        animator.SetBool("isFadeOff", true);
    }

    public void FadeEnd()
    {
        animator.SetBool("isFadeOn", false);
        animator.SetBool("isFadeOff", false);
        blindImage.raycastTarget = false;
        isEndChange = true;
    }

    // BGM이 변하면서 씬을 변경
    public IEnumerator switchScene(int Sceneindex, int BGMindex, float fadeInTime, float fadeOutTime)
    {
        FadeIn();
        animator.speed = 1f / fadeInTime;
        StartCoroutine(BGMCtrl.switchBGM(BGMindex, fadeInTime, fadeOutTime));

        yield return new WaitForSeconds(fadeInTime);

        SceneManager.LoadScene(Sceneindex);
        FadeOut();
        animator.speed = 1f / fadeOutTime;

        yield return new WaitForSeconds(fadeOutTime);

        FadeEnd();
    }

    // BGM이 변하면서 위치 이동
    public IEnumerator switchPos(Vector3 _pos, int BGMindex, float fadeInTime, float fadeOutTime)
    {
        animator.speed = 1f / fadeInTime;
        StartCoroutine(BGMCtrl.switchBGM(BGMindex, fadeInTime, fadeOutTime));
        FadeIn();

        yield return new WaitForSeconds(fadeInTime);

        FadeOut();
        InGameController.instance.playerCtrl.transform.position = _pos;
        Camera.main.transform.position = _pos + new Vector3Int(0, 0, -10);
        animator.speed = 1f / fadeOutTime;

        yield return new WaitForSeconds(fadeOutTime);

        FadeEnd();
    }

    // BGM이 변하지 않으면서 위치 이동
    public IEnumerator switchPos(Vector3 _pos, float fadeInTime, float fadeOutTime)
    {
        animator.speed = 1f / fadeInTime;
        FadeIn();

        yield return new WaitForSeconds(fadeInTime);

        FadeOut();
        InGameController.instance.playerCtrl.transform.position = _pos;
        animator.speed = 1f / fadeOutTime;
        Camera.main.transform.position = _pos + new Vector3Int(0, 0, -10);

        yield return new WaitForSeconds(fadeOutTime);

        FadeEnd();
    }
}