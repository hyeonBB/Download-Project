using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShootCountStart : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] private GameObject m_count;

    [Header("Resource")]
    [SerializeField] private Sprite[] m_image;

    private bool  m_click = false;
    private int   m_Index = 0;
    private float m_updatTime = 1.0f;
    private float m_time = 0.0f;

    private Image m_countImage;

    private void Start()
    {
        m_countImage = m_count.GetComponent<Image>();
    }

    private void Update()
    {
        if(!m_click)
        {
            if (EventSystem.current.IsPointerOverGameObject()) // Ŀ���� UI ��ġ�� ������ �� ��ȯ
                return;

            if (Input.GetMouseButtonDown(0))
            {
                m_click = true;
                Start_Count();
            }
        }
        else
        {
            Update_Count();
        }

    }

    private void Start_Count()
    {
        m_count.SetActive(true);
        m_countImage.sprite = m_image[m_Index];
    }

    private void Update_Count()
    {
        // 3/ 2/ 1 ī��Ʈ �ٿ� �� ���� ����
        m_time += Time.deltaTime;
        if(m_time >= m_updatTime)
        {
            m_time = 0f;
            m_Index++;
            if(m_Index > 2)
                Finish_Count();
            else
                m_countImage.sprite = m_image[m_Index];
        }    
    }

    private void Finish_Count()
    {
        VisualNovelManager.Instance.Play_ShootGame();
        Destroy(gameObject);
    }
}