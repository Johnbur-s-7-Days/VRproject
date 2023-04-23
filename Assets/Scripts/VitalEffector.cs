using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VitalEffector : MonoBehaviour
{
    [SerializeField] private PostProcessVolume volume;
    private Vignette vignette;
    private int heartRate_cur, heartRate_min, heartRate_max;

    private List<AudioSource> audioSources = new List<AudioSource>();
    private float temperature_cur, temperature_min, temperature_max;

    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGetSettings(out vignette);
        heartRate_cur = SensorManager.HeartRate;
        heartRate_min = SensorManager.HeartRate_Min;
        heartRate_max = SensorManager.HeartRate_Max;
        temperature_cur = SensorManager.TempRate;
        temperature_min = SensorManager.TempRate_Min;
        temperature_max = SensorManager.TempRate_Max;

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
            SetSightEffect(Mathf.Lerp(0f, 1f, (heartRate_cur - heartRate_min) / (heartRate_max - heartRate_min)));
            SetEarEffect(Mathf.Lerp(1f, 1.5f, (temperature_cur - temperature_min) / (temperature_max - temperature_min)));
            yield return null;
        }
    }
}
