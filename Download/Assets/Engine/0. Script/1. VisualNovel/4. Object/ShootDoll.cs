using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DOLLTYPE { DT_TRASH, DT_BIRD, BT_SHEEP, BT_CAT, BT_END };

public class ShootDoll : MonoBehaviour
{
    [SerializeField] private GameObject m_Particle;
    [SerializeField] private GameObject m_PanelObject;
    [SerializeField] private GameObject m_ClearObject;

    [SerializeField] private DOLLTYPE m_dollType = DOLLTYPE.BT_END;
    [SerializeField] private int m_line = 0;
    public int Line
    {
        get { return m_line; }
        set { m_line = value; }
    }

    private int m_hp = 5;
    public int Hp
    {
        get { return m_hp; }
        set { m_hp = value; }
    }

    private ShootContainerBelt m_belt;
    public ShootContainerBelt Belt
    {
        set { m_belt = value; }
    }

    private GameObject m_hpbar;
    public GameObject Hpbar
    {
        set { m_hpbar = value; }
    }

    private int m_blinkCount = 2;
    private SpriteRenderer m_spriteRenderer;

    private bool m_clear = false;
    private bool m_clearImage = false;
    private float m_clearTime = 0f;

    private void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(m_clear)
        {
            m_clearTime += Time.deltaTime;
            if (m_clearTime > 1.5f)
            {
                if (!m_belt.OverEffect)
                {
                    m_belt.Over_Game(this); // 2) 1.5�� �� ���� ���� ���� ���� ����
                    Destroy(m_hpbar);       // ���� hp�� ����

                }
                else if (!m_clearImage && m_clearTime > 3) // 3) 1.5�� �� �ش� ������Ʈ �̹��� ȭ�� ��� ����
                {
                    m_clearImage = true;

                    m_PanelObject.SetActive(true);
                    m_ClearObject.SetActive(true);
                    m_ClearObject.GetComponent<Image>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                }
                else if (m_clearTime > 4.5) // 4) 1.5�� �� ���̵� �ƿ����� ��ȯ
                    VisualNovelManager.Instance.Change_Level(LEVELSTATE.LS_NOVELEND);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Ball(Clone)")
        {
            m_hp--;
            if(m_hp <= 0) // ���� ����
            {
                VisualNovelManager.Instance.ShootGameStop = true;
                m_belt.UseBelt = false; // 1) ���� �Ͻ� ����
                m_clear = true;
            }
            else
            {
                StartCoroutine(Blink());
            }

            Destroy(other.gameObject);
            Create_Particle();
        }
    }

    private IEnumerator Blink()
    {
        Color startColor = m_spriteRenderer.color;

        int Count = 0;
        while (Count < m_blinkCount)
        {
            float alpha = startColor.a;
            while (alpha > 0f)
            {
                alpha -= 0.1f;
                m_spriteRenderer.color = new Color(m_spriteRenderer.color.r, m_spriteRenderer.color.g, m_spriteRenderer.color.b, alpha);
                yield return new WaitForSeconds(0.005f);
            }
            yield return new WaitForSeconds(0.01f);

            while (alpha < 1f)
            {
                alpha += 0.1f;
                m_spriteRenderer.color = new Color(m_spriteRenderer.color.r, m_spriteRenderer.color.g, m_spriteRenderer.color.b, alpha);
                yield return new WaitForSeconds(0.005f);
            }
            yield return new WaitForSeconds(0.01f);

            Count++;
        }
        m_spriteRenderer.color = startColor;
    }

    private void Create_Particle()
    {
        GameObject clone = Instantiate(m_Particle);
        clone.transform.position = transform.position;
    }

    public void Explode_Doll()
    {
        Create_Particle();
        Destroy(gameObject);
    }
}
