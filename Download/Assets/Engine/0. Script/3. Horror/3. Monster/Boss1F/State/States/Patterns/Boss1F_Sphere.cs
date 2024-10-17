using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Sphere : Boss1F_Base
{
    public enum STATE { ST_SYMPTOMS, ST_ATTACK, ST_END }

    private bool m_recall = false;
    private float m_time = 0f;
    private float m_symptomsTime = 0f;
    private float m_rotationSpeed = 0f;
    private float m_attackTime = 0f;
    private STATE m_state = STATE.ST_END;

    public Boss1F_Sphere(StateMachine<Monster> stateMachine) : base(stateMachine)
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

        Debug.Log("��ü ����");

        m_rotationSpeed = 2f; // �⺻ ���ǵ� : ȸ���ϴ� �ӵ��� �ణ ������ �ؼ� ���ΰ��� ���׹̳��� ���ų�, �̸� ������ ������ ������ ��� �������� �Ե��� ����.
        if (m_owner.Pattern == 1)
        {
            m_symptomsTime = 1.5f;
        }
        else if (m_owner.Pattern == 1)
        {
            m_symptomsTime = 1f;
            m_rotationSpeed *= 1.25f; // 25% ����
        }

        // ��������
        m_state = STATE.ST_SYMPTOMS;
        m_time = 0f;
        // �� ���� ��ȿ �ִϸ��̼� ���
        // ī�޶� ����ŷ
        // ���ÿ� õ�忡�� �ٴ����� ������ ��ġ�� ���ڰ� ������. ���� 2��
    }

    public override void Update_State()
    {
        switch(m_state)
        {
            case STATE.ST_SYMPTOMS:
                m_time += Time.deltaTime;
                if(m_time >= m_symptomsTime)
                {
                    m_time = 0f;
                    m_state = STATE.ST_ATTACK;
                    Attack_Sphere();
                }
                break;

            case STATE.ST_ATTACK:
                m_time += Time.deltaTime;
                if (m_time >= 5f)
                {
                    m_stateMachine.Change_State((int)Boss1F.State.ST_REST);
                }
                else
                {
                    m_attackTime += Time.deltaTime;
                    if(m_attackTime >= 1.5f)
                    {
                        m_attackTime = 0f;
                        Attack_Sphere();
                    }
                }
                break;
        }
    }

    public override void Exit_State()
    {
    }

    private void Attack_Sphere()
    {
        //�Կ��� ����� ������ ������ �߻��Ѵ�. �����鼭(�ξƾ�~��ó��) ������ �������� ȸ���Ѵ�.
        //ȸ�� ������ 50 % Ȯ���� ���� / 50 % Ȯ���� ���ΰ��� ����� ����
    }
}
