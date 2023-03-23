using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMCtrl : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private bool isFadeOut;
    private bool isStartFade;
    private float fadeTime;

    private void Start()
    {
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStartFade)
            BGMFade();
    }

    /// <summary>
    /// BGM을 재생시키는 함수
    /// </summary>
    /// <param name="bgmIndex">실행시킬 BGM 번호</param>
    /// <param name="_isFade">BGM을 종료할 지 여부</param>
    /// <param name="_fadeTime">Fade가 진행되는 시간(초)</param>
    public void BGMSetting(int bgmIndex, bool _isFadeOut, float _fadeTime)
    {
        if (bgmIndex != -1)
            audioSource.clip = DataPool.BGMs[bgmIndex];
        isFadeOut = _isFadeOut;
        fadeTime = _fadeTime;
        isStartFade = true;
    }

    public IEnumerator switchBGM(int BGMindex, float fadeInTime, float fadeOutTime)
    {
        BGMSetting(-1, true, fadeInTime);
        yield return new WaitForSeconds(fadeInTime);
        BGMSetting(BGMindex, false, fadeOutTime);
        audioSource.Play();
    }

    public void BGMFade()
    {
        if (isFadeOut)
        {
            if (audioSource.volume > 0f)
            {
                audioSource.volume -= Time.deltaTime * 1f / fadeTime;
            }
            else
            {
                audioSource.volume = 0f;
                isStartFade = false;
            }
        }
        else
        {
            if (audioSource.volume < 1f)
            {
                audioSource.volume += Time.deltaTime * 1f / fadeTime;
            }
            else
            {
                audioSource.volume = 1f;
                isStartFade = false;
            }
        }
    }
}
