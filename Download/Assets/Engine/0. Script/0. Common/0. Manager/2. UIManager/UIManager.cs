using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager m_instance = null;
    public static UIManager Instance
    {
        get
        {
            if (null == m_instance)
                return null;
            return m_instance;
        }
    }

    [SerializeField] private GameObject m_fadePanalObj;
    private Image m_fadeImg; // ���̵忡 ����� �̹���

    private bool isFade = false;

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;

        m_fadeImg = m_fadePanalObj.GetComponent<Image>();
    }

    public void Start_FadeIn(float duration, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (isFade)
            return;

        StartCoroutine(FadeCoroutine(1f, 0f, duration, color, onComplete, waitTime, panalOff));
    }

    public void Start_FadeOut(float duration, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (isFade)
            return;

        StartCoroutine(FadeCoroutine(0f, 1f, duration, color, onComplete, waitTime, panalOff));
    }

    public void Start_FadeInOut(float duration, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (isFade)
            return;

        StartCoroutine(FadeCoroutine(1f, 0f, duration, color, () => Start_FadeOut(duration, color, onComplete), waitTime, panalOff));
    }

    public void Start_FadeOutIn(float duration, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (isFade)
            return;

        StartCoroutine(FadeCoroutine(0f, 1f, duration, color, () => Start_FadeIn(duration, color, onComplete), waitTime, panalOff));
    }

    private IEnumerator FadeCoroutine(float startAlpha, float targetAlpha, float duration, Color color, Action onComplete, float waitTime, bool panalOff)
    {
        isFade = true;
        m_fadePanalObj.SetActive(true);
        float currentTime = 0f;

        Color startColor = color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float fadeProgress = currentTime / duration;
            m_fadeImg.color = new Color(startColor.r, startColor.g, startColor.b, 
                Mathf.Lerp(startAlpha, targetAlpha, fadeProgress));

            yield return null;
        }

        m_fadeImg.color = targetColor;
        isFade = false;

        if (panalOff)
        {
            m_fadePanalObj.SetActive(false);
        }

        if (onComplete != null)
        {
            yield return new WaitForSeconds(waitTime);
            onComplete?.Invoke(); // �ݹ� �Լ� ȣ��
        }
    }
}
