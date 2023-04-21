using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VitalEffector : MonoBehaviour
{
    [SerializeField] private PostProcessVolume volume;
    private Vignette vignette;
    private float heartRate_cur, heartRate_min, heartRate_max;

    private List<AudioSource> audioSources = new List<AudioSource>();
    private float temperature_cur, temperature_min, temperature_max;

    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGetSettings(out vignette);
        heartRate_cur = 100;
        heartRate_min = 100;
        heartRate_max = 140;
        temperature_cur = 36.5f;
        temperature_min = 36.5f;
        temperature_max = 37.5f;

        StartCoroutine(TestVitalEffect());
        StartCoroutine(FindAllAudios());
    }

    private void SetSightEffect(float _lerp)
    {
        vignette.intensity.value = _lerp;
        vignette.roundness.value = _lerp;
    }

    private void SetEarEffect(float _lerp)
    {
        foreach (var audio in audioSources)
        {
            audio.pitch = _lerp;
        }
    }

    IEnumerator FindAllAudios()
    {
        yield return null;

        audioSources.AddRange(FindObjectsOfType<AudioSource>());
    }

    IEnumerator TestVitalEffect()
    {
        while (true)
        {
            heartRate_cur += Time.deltaTime;
            SetSightEffect(Mathf.Lerp(0f, 1f, (heartRate_cur - heartRate_min) / (heartRate_max - heartRate_min)));

            temperature_cur += Time.deltaTime * 0.05f;
            SetEarEffect(Mathf.Lerp(1f, 1.5f, (temperature_cur - temperature_min) / (temperature_max - temperature_min)));
            yield return null;
        }
    }
}
