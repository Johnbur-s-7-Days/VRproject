using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class measureHealth : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float time;
    public float fadeDuration = 1f; 
    public TMP_Text secnum;
    CanvasGroup panelCanvasGroup;

    void Start()
    {
        time = this.time;
        panelCanvasGroup = GetComponent<CanvasGroup>();
        this.secnum.text = string.Format("{0}",this.time);
        this.StartCoroutine(this.waitMeasure());
    }

    private IEnumerator waitMeasure()
    {
        float delta = this.time;
        while(true)
        {
            delta -= Time.deltaTime;
            this.secnum.text = string.Format ("{0}",(int)delta);
            if (delta <= 0)
            {
                StartCoroutine(FadeOutCoroutine(panelCanvasGroup, fadeDuration));
                break;
            }
            yield return null;
        }
    }

    private IEnumerator FadeOutCoroutine(CanvasGroup targetCanvasGroup, float duration)
    {
        float elapsedTime = 0f;
        float startAlpha = targetCanvasGroup.alpha;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float fadeRatio = elapsedTime / duration;

            Debug.Log(startAlpha);

            // 알파 값 조절
            targetCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, fadeRatio);

            yield return null;
        }

    }


    
}
