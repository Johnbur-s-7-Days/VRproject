using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class measureHealth : MonoBehaviour
{
   
    public float time;
    public float fadeDuration;

    public TMP_Text sec_num;
    public TMP_Text heart_txt;
    public TMP_Text temp_txt;

    public GameObject measureUI;
    public GameObject ResultUI;

    public SensorManager sensorData;

    void Start()
    {
        ResultUI.GetComponent<CanvasGroup>().alpha = 0f;
        ResultUI.SetActive(false);
        StartCoroutine(waitMeasure());
    }

    public void RestartBtnClick()
    {
        StartCoroutine(FadeCoroutine(ResultUI, measureUI, fadeDuration));
        StartCoroutine(waitMeasure());
    }

    public void GamestartBtnClick()
    {
        StartCoroutine(FadeCoroutine(ResultUI,measureUI, fadeDuration));
        SceneManager.LoadScene("QA_Office_Nojun");
    }

    private IEnumerator waitMeasure()
    {
        float delta = time;
        sensorData.isReadingData = true;
        while(delta > 0)
        {
            sec_num.text = delta.ToString();
            yield return new WaitForSeconds(1f);
            delta--;
        }
        sensorData.isReadingData = false;

        heart_txt.text = sensorData.HeartRate.ToString();
        temp_txt.text = sensorData.TempRate.ToString();

        StartCoroutine(FadeCoroutine(measureUI, ResultUI, fadeDuration));
    }

    private IEnumerator FadeCoroutine(GameObject FadeoutPanel, GameObject FadeinPanel, float duration)
    {
        CanvasGroup targetCanvasGroup = FadeoutPanel.GetComponent<CanvasGroup>();
        float originalAlpha = targetCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float fadeRatio = elapsedTime / duration;

            float newAlpha = Mathf.Lerp(originalAlpha, 0f, fadeRatio);
            targetCanvasGroup.alpha = newAlpha;

            yield return null;
        }

        FadeoutPanel.SetActive(false);
        if(targetCanvasGroup.alpha == 0f)
            StartCoroutine(FadeInCoroutine(FadeinPanel, duration));
    }

    private IEnumerator FadeInCoroutine(GameObject FadeinPanel, float duration)
    {
        CanvasGroup targetCanvasGroup = FadeinPanel.GetComponent<CanvasGroup>();
        float originalAlpha = targetCanvasGroup.alpha;
        float elapsedTime = 0f;

        FadeinPanel.SetActive(true);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float fadeRatio = elapsedTime / duration;

            float newAlpha = Mathf.Lerp(originalAlpha, 1f, fadeRatio);
            targetCanvasGroup.alpha = newAlpha;

            yield return null;
        }

       
    }


}
