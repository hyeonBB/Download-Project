using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Weakness : Boss1F_Base // ��������, �ö󰡱�
{
    private bool m_isUsed = false;
    private bool m_isDown = false;

    private bool m_isWeakness = false;
    private float m_stateTime = 0f;

    public Boss1F_Weakness(StateMachine<Monster> stateMachine) : base(stateMachine)
    {

    }

    public override void Enter_State()
    {
        Debug.Log("��������");

        if (m_isUsed == false)
        {
            m_isUsed = true;

            m_isWeakness = false;
            m_stateTime = 0f;

            m_isDown = true;
            m_animator.speed = 1f;
            m_animator.SetLayerWeight(1, 0f);
            m_animator.SetLayerWeight(2, 0f);
            m_animator.SetBool("IsWeakDown", true);

            m_owner.IsInvincible = true;

            Debug.Log("���� �������� ����");
        }
        else
        {
            Debug.Log("���� ���� ����");
        }
    }

    public override void Update_State()
    {
        if (m_isDown == true)
        {
            if (m_animator.IsInTransition(0) == true)
                return;

            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("IsWeakDown") == true)
            {
                m_animator.SetBool("IsWeakDown", false);

                float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (animTime >= 1f) // �ִϸ��̼� ����
                {
                    m_isDown = false;
                    m_isWeakness = true;

                    Set_Weakness(true); // �ݶ��̴� Ȱ��ȭ
                    m_owner.IsInvincible = false;

                    Debug.Log("���� ���� ����");
                }
            }
        }
        else if (m_isWeakness == true)
        {
            m_stateTime += Time.deltaTime;
            if (m_stateTime > 5f) // 5�� ����
            {
                m_isWeakness = false;

                Set_Weakness(false); // �ݶ��̴� ��Ȱ��ȭ
                m_owner.IsInvincible = true;

                m_animator.speed = 1f;
                m_animator.SetLayerWeight(1, 0f);
                m_animator.SetLayerWeight(2, 0f);
                m_animator.SetBool("IsWeakUp", true);

                Debug.Log("���� �ö󰡱� ����");
            }
        }
        else
        {
            if (m_animator.IsInTransition(0) == true)
                return;

            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("IsWeakUp") == true)
            {
                m_animator.SetBool("IsWeakUp", false);

                float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (animTime >= 1f) // �ִϸ��̼� ����
                {
                    m_isUsed = false;

                    m_owner.CumulativeDamage = 0;
                    m_owner.IsInvincible = false;

                    Change_Patterns();

                    Debug.Log("���� �Ϸ� ����");
                }
            }
        }
    }

    public override void Exit_State()
    {
        m_animator.SetBool("IsWeakDown", false);
        m_animator.SetBool("IsWeakUp", false);
    }

    private void Set_Weakness(bool active)
    {
        //���̶� ���� ������ �κ��� �����ϸ� �⺻ �������� ����, ���� �����ϸ� �������� 2.5��� ����.
        if(active == true)
        {

        }
        else
        {

        }
    }
}
