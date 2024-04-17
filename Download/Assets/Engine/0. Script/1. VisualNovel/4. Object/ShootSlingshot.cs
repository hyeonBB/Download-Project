using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootSlingshot : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] private GameObject m_gauge;
    [SerializeField] private GameObject m_ball;

    [Header("Resource")]
    [SerializeField] private Sprite[] m_Image;

    [Header("Value")]
    [SerializeField] private float m_curSpeed = 0.0f;
    [SerializeField] private float m_minSpeed = 3.0f;
    [SerializeField] private float m_maxSpeed = 8.0f;
    [SerializeField] private float m_gaugeSpeed = 7.0f;

    private SpriteRenderer m_spriteRenderer;
    private Slider m_barSlider;
    private bool m_Up = true;

    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_barSlider = m_gauge.GetComponent<Slider>();

        m_curSpeed = m_minSpeed;
    }

    private void Update()
    {
        if (!VisualNovelManager.Instance.ShootGameStart)
            return;

        if (Input.GetMouseButton(0))
        {
            // ���콺�� ���� ��ġ�� ������
            Vector3 mousePosition = Input.mousePosition;

            // ȭ���� ���� ���̸� 3����Ͽ� �� ������ ��踦 ���
            float screenWidth = Screen.width;
            float sectionWidth = screenWidth / 3f;

            // ���콺�� x ��ǥ�� ���ǿ� ���� �з�
            int section = (int)(mousePosition.x / sectionWidth);
            switch (section)
            {
                case 0:
                    m_spriteRenderer.sprite = m_Image[3];
                    break;
                case 1:
                    m_spriteRenderer.sprite = m_Image[2];
                    break;
                case 2:
                    m_spriteRenderer.sprite = m_Image[1];
                    break;
                default:
                    break;
            }

            if (m_Up)
            {
                m_curSpeed += Time.deltaTime * m_gaugeSpeed;
                if (m_curSpeed >= m_maxSpeed)
                    m_Up = false;
            }
            else
            {
                m_curSpeed -= Time.deltaTime * m_gaugeSpeed;
                if (m_curSpeed <= m_minSpeed)
                    m_Up = true;
            }
            m_barSlider.value = m_curSpeed;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 NewPosition = Vector3.zero;

            Vector3 mousePosition = Input.mousePosition;
            float screenWidth = Screen.width;
            float sectionWidth = screenWidth / 3f;
            int section = (int)(mousePosition.x / sectionWidth);
            switch (section)
            {
                case 0:
                    NewPosition = new Vector3(7.948f, -4.5f, 0f); 
                    break;
                case 1:
                    NewPosition = new Vector3(6.861f, -4.5f, 0f);
                    break;
                case 2:
                    NewPosition = new Vector3(5.7f, -4.5f, 0f);
                    break;
            }

            GameObject ball = Instantiate(m_ball);
            ball.GetComponent<Transform>().position = NewPosition;
            ball.GetComponent<ShootBall>().TargetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            ball.GetComponent<ShootBall>().Speed = m_curSpeed;

            m_spriteRenderer.sprite = m_Image[0];

            m_curSpeed = m_minSpeed;
            m_barSlider.value = m_curSpeed;
        }
    }
}
