using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    public float duration = 1.0f; // 색상 전환에 걸리는 시간
    public bool isInit = false;

    Material _material;

    void Start()
    {
        _material = GetComponent<Renderer>().material;

        _material = Instantiate(_material);

        GetComponent<Renderer>().material = _material;

        gameObject.SetActive(false);
    }

    public void StartExit()
    {
        Debug.Log("kks StartExit!");
        isInit = true;
        gameObject.SetActive(true);
        StartCoroutine(LerpColor());
    }

    private IEnumerator LerpColor()
    {
        while (true)
        {
            // 빨간색에서 흰색으로 변화
            yield return StartCoroutine(ChangeColor(Color.red, Color.white, duration));

            // 흰색에서 빨간색으로 변화
            yield return StartCoroutine(ChangeColor(Color.white, Color.red, duration));
        }
    }

    private IEnumerator ChangeColor(Color startColor, Color endColor, float duration)
    {
        float time = 0;

        while (time < duration)
        {
            // 시간에 따라 색상을 선형 보간
            _material.color = Color.Lerp(startColor, endColor, time / duration);
            time += Time.deltaTime;

            // 한 프레임 대기
            yield return null;
        }

        // 마지막 색상 설정 (누락 방지)
        _material.color = endColor;
    }
}
