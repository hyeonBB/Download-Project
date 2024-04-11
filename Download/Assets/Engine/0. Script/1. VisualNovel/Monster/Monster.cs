using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public enum State { ST_CHASE, ST_ATTCK, ST_END }

    [SerializeField] private State m_state = State.ST_CHASE;
    [SerializeField] private float m_attackDist = 2.0f;
    [SerializeField] private bool m_attacked = false;

    private Transform m_playerTr;
    private Transform m_monsterTr;
    private NavMeshAgent m_Agent;

    private void Start()
    {
        m_playerTr  = GameObject.FindWithTag("Player").GetComponent<Transform>();
        m_monsterTr = GetComponent<Transform>();
        m_Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Check_State();  // ���� ���� üũ
        Update_State(); // ���� ���� ������Ʈ
    }

    private void Check_State()
    {
        if (m_attacked)
            return;

        float distance = Vector3.Distance(m_playerTr.position, m_monsterTr.position);
        if (distance <= m_attackDist)
            m_state = State.ST_ATTCK;
        else
            m_state = State.ST_CHASE;
    }

    private void Update_State()
    {
        switch(m_state)
        {
            case State.ST_CHASE:
                m_Agent.destination = m_playerTr.position; // �߰� ��� ��ġ�� ����
                break;

            case State.ST_ATTCK: // ���� ����
                m_attacked = true;
                break;
        }
    }

    private void OnDrawGizmos()
    {
        // ���� ���� ǥ��
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_attackDist);
    }
}
