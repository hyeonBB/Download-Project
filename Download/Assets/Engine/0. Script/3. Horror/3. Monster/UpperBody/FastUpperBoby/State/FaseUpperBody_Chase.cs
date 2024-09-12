using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FaseUpperBody_Chase : FaseUpperBody_Base // �� �ѹ��� ����
{
    private float m_attackDist = 1f;
    protected Vector3 m_targetPosition;

    private NavMeshAgent m_agent;

    public FaseUpperBody_Chase(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_agent = m_owner.GetComponent<NavMeshAgent>();
        m_agent.speed = 6f;
    }

    public override void Enter_State()
    {
        m_agent.enabled = true;
        m_agent.speed = 50f;
        m_agent.stoppingDistance = m_attackDist;

        m_targetPosition = HorrorManager.Instance.Player.transform.position;

        // m_animator.SetBool("IsRun", true);
    }

    public override void Update_State()
    {
        if (Change_Attack() == false)
        {
            // ���� ������� ������ �߰�
            //

            Vector3 direction = m_targetPosition - m_owner.transform.position;
            direction.y = 0;
            direction = direction.normalized;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                if (Quaternion.Angle(m_owner.transform.rotation, targetRotation) > 10f) // ���� ȸ���� ��ǥ ȸ���� ��ġ�ϴ��� Ȯ��/ ȸ���� �Ϸ���� �ʾ����� ȸ�� ����
                {
                    m_owner.transform.rotation = Quaternion.Slerp(m_owner.transform.rotation, targetRotation, Time.deltaTime * 10f);
                    return;
                }
            }

            m_agent.destination = m_targetPosition;
        }
    }

    public override void Exit_State()
    {
        m_agent.enabled = false;

        // m_animator.SetBool("IsRun", false);
    }

    private bool Change_Attack()
    {
        // �÷��̾ ���� ���� ���� �����ϸ� ���� ���� ��ȯ
        float distanceToPlayer = Vector3.Distance(m_stateMachine.Owner.transform.position, m_targetPosition);
        if (distanceToPlayer <= m_attackDist)
        {
            m_stateMachine.Change_State((int)FaseUpperBody.State.ST_ATTECK); // ���� ���·� ��ȯ
            return true;
        }

        return false;
    }
}
