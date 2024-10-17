using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Tentacles : Boss1F_Base
{
    private bool m_recall = false;
    private float m_time = 0f;

    private int m_currentCount = 0;
    private int m_createCount = 0;
    private float m_retryTime = 0f;

    float m_symptomTime = 0f;
    float m_idleTime = 0f;

    public Boss1F_Tentacles(StateMachine<Monster> stateMachine) : base(stateMachine)
    {

    }

    public override void Enter_State()
    {
        if (m_recall == false)
        {
            m_recall = true;

            if (Change_Recall() == true)
                return;
        }

        Debug.Log("�˼� N�� ����");
        if (m_owner.Pattern == 1)
        {
            m_createCount = Random.Range(2, 4); // 2 ~ 3�� ���� �ݺ�
            m_retryTime = 1.5f; // �� ���� �ð�
            m_symptomTime = 1.5f;
            m_idleTime = 1.5f;
        }
        else if (m_owner.Pattern == 2)
        {
            m_createCount = Random.Range(3, 5); // 3 ~ 4�� ���� �ݺ�
            m_retryTime = 1.2f; // �� ���� �ð�
            m_symptomTime = 1.2f;
            m_idleTime = 1.2f;
        }

        Create_Tentacle(m_symptomTime, m_idleTime);
        m_currentCount++;
    }

    public override void Update_State()
    {
        if(m_currentCount < m_createCount)
        {
            m_time += Time.deltaTime;
            if (m_time >= m_retryTime)
            {
                m_time = 0f;

                Debug.Log("�˼� N�� �����");
                Create_Tentacle(m_symptomTime, m_idleTime);
                m_currentCount++;
            }
        }
        else
        {
            m_time += Time.deltaTime;
            if (m_time >= 1f) // �̺�Ʈ ���� Ȥ�� ��� �ð�
            {
                m_recall = false;
                m_stateMachine.Change_State((int)Boss1F.State.ST_REST);
            }
        }
    }

    public override void Exit_State()
    {
    }
}
