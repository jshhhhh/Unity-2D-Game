using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public SpriteRenderer white;
    public SpriteRenderer black;
    private Color color;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);


    //페이드아웃 함수(코루틴으로 돌림)
    //기본값이 0.02f
    public void FadeOut(float _speed = 0.02f)
    {
        //맵을 왔다 갔다 하면 코루틴이 꼬이게 되므로 stop시켜줘야 함
        //마지막에 실행되는 코루틴이 우선순위를 가짐
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(_speed));
    }

    IEnumerator FadeOutCoroutine(float _speed)
    {
        //검정색을 color에 넣어줌
        color = black.color;

        //color의 알파값이 1 미만이 될 때까지 반복
        while (color.a < 1f)
        {
            color.a += _speed;
            black.color = color;
            yield return waitTime;
        }
    }

    public void Flash(float _speed = 0.1f)
    {
        StopAllCoroutines();
        StartCoroutine(FlashCoroutine(_speed));
    }

    IEnumerator FlashCoroutine(float _speed)
    {
        color = white.color;

        //color의 알파값이 1 미만이 될 때까지 반복
        while (color.a < 1f)
        {
            color.a += _speed;
            white.color = color;
            yield return waitTime;
        }

        color = white.color;

        //color의 알파값이 1 미만이 될 때까지 반복
        while (color.a > 0f)
        {
            color.a -= _speed;
            white.color = color;
            yield return waitTime;
        }
    }

    public void FadeIn(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine(_speed));
    }

    IEnumerator FadeInCoroutine(float _speed)
    {
        //검정색을 color에 넣어줌
        color = black.color;

        //color의 알파값이 0이 될 때까지 반복(검은색이 사라질 때까지)
        while (color.a > 0f)
        {
            color.a -= _speed;
            black.color = color;
            yield return waitTime;
        }
    }

    public void FlashOut(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FlashOutCoroutine(_speed));
    }

    IEnumerator FlashOutCoroutine(float _speed)
    {
        color = white.color;

        //color의 알파값이 1 미만이 될 때까지 반복
        while (color.a < 1f)
        {
            color.a += _speed;
            white.color = color;
            yield return waitTime;
        }
    }

    public void FlashIn(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FlashInCoroutine(_speed));
    }

    IEnumerator FlashInCoroutine(float _speed)
    {
        color = white.color;

        //color의 알파값이 1 미만이 될 때까지 반복
        while (color.a > 0f)
        {
            color.a -= _speed;
            white.color = color;
            yield return waitTime;
        }
    }
}
