using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pulseUI : MonoBehaviour
{
    public Image image;
    public Sprite[] images;
    private int currentIndex = 0;
    public float timer = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // 초기 이미지 설정
        image.sprite = images[currentIndex];
        // 코루틴 시작
        StartCoroutine(ChangeImageCoroutine());
    }

    private IEnumerator ChangeImageCoroutine()
    {
        while (true)
        {
            // 1초 대기
            yield return new WaitForSeconds(timer);
            
            // 다음 이미지 인덱스로 이동
            currentIndex = (currentIndex + 1) % images.Length;
            // 이미지 변경
            image.sprite = images[currentIndex];
        }
    }
}
