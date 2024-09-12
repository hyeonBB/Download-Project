using System.Collections;
using UnityEngine;

public class Jumpscare_Girl : Jumpscare
{
    [SerializeField] private GameObject m_girl;

    public override void Active_Jumpscare()
    {
        m_isTrigger = true;

        m_girl.SetActive(true);
        StartCoroutine(Move_Girl());
    }

    private IEnumerator Move_Girl()
    {
        // ��� ---
        float time = 0f;
        while(true)
        {
            time += Time.deltaTime;
            if (time >= 0.3f) // 0.3�� ����
                break;
            yield return null;
        }

        // �ȱ� ---
        float speed = 2.5f;
        while (true)
        {
            m_girl.transform.localPosition = new Vector3(m_girl.transform.localPosition.x, m_girl.transform.localPosition.y, m_girl.transform.localPosition.z + speed * Time.deltaTime);
            if (m_girl.transform.localPosition.z >= 3.2f)
                break;
            yield return null;
        }

        // ���ھ��� ���� ---
        Destroy(m_girl);
        yield break;
    }
}
