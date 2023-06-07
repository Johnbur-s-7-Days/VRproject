using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class measureHealth : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float time;
    public float fadeDuration;
    public TMP_Text secnum;

    public GameObject measureUI;
    public GameObject ResultUI;


    void Start()
    {
        this.StartCoroutine(this.waitMeasure());
    }

    public void RestartBtnClick()
    {
        StartCoroutine(FadeOutCoroutine(ResultUI, measureUI, fadeDuration));
        //StartCoroutine(waitMeasure());
    }

    public void GamestartBtnClick()
    {
        StartCoroutine(FadeOutCoroutine(ResultUI,measureUI, fadeDuration));
        SceneManager.LoadScene("QA_Office_Nojun");
    }


    private IEnumerator waitMeasure()
    {
        float delta = time;
        while(true)
        {
            delta -= Time.deltaTime;
            this.secnum.text = string.Format ("{0}",(int)delta);
            if (delta <= 0)
            {
                StartCoroutine(FadeOutCoroutine(measureUI, ResultUI,  fadeDuration));
                break;
            }
            yield return null;
        }

    }

    private IEnumerator FadeOutCoroutine(GameObject FadeoutPanel, GameObject FadeinPanel, float duration)
    {
        CanvasGroup targetCanvasGroup = FadeoutPanel.GetComponent<CanvasGroup>();
        float originalAlpha = targetCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float fadeRatio = elapsedTime / duration;

            // 패널 투명도 조절
            float newAlpha = Mathf.Lerp(originalAlpha, 0f, fadeRatio);
            targetCanvasGroup.alpha = newAlpha;

            yield return null;
        }

        FadeoutPanel.SetActive(false);
        if(targetCanvasGroup.alpha == 0f)
            StartCoroutine(FadeInCoroutine(FadeinPanel, duration));
    }

    private IEnumerator FadeInCoroutine(GameObject Panel, float duration)
    {
        CanvasGroup targetCanvasGroup = Panel.GetComponent<CanvasGroup>();
        float originalAlpha = targetCanvasGroup.alpha;
        float elapsedTime = 0f;

        Panel.SetActive(true);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float fadeRatio = elapsedTime / duration;

            // 패널 투명도 조절
            float newAlpha = Mathf.Lerp(originalAlpha, 1f, fadeRatio);
            targetCanvasGroup.alpha = newAlpha;

            yield return null;
        }

       
    }


}
