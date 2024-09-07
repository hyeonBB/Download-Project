using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straitjacket_Attack : Straitjacket_Base
{
    private float m_change = 1f;
    private float m_time   = 0f;

    public Straitjacket_Attack(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_time = 0;
        HorrorManager.Instance.Player.Damage_Player(m_owner.Attack);

        m_animator.SetBool("IsAttack", true);
    }

    public override void Update_State()
    {
        // ���� ����� ������ �ٽ� ����
        m_time += Time.deltaTime;
        if(m_time >= m_change)
        {
            if (Change_Attack() == false) // �Ÿ��� ���� �̻��� �� �� �߰�, �ƴ� �� �����
                m_stateMachine.Change_State((int)Straitjacket.State.ST_RUN);
        }
    }

    public override void Exit_State()
    {
        m_animator.SetBool("IsAttack", false);
    }
}
