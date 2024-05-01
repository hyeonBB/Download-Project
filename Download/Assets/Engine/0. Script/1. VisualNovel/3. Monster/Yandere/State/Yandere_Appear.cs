using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yandere_Appear : State<HallwayYandere>
{
    private Animator m_animator;

    public Yandere_Appear(StateMachine<HallwayYandere> stateMachine) : base(stateMachine)
    {
        m_animator = m_stateMachine.Owner.GetComponentInChildren<Animator>();
    }

    public override void Enter_State()
    {
    }

    public override void Update_State()
    {
        // 0�� �� �÷��� ���� �ƴϸ�, 1�̰ų� 1���� Ŭ�� �ִϸ��̼� ����
        if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)  // �ִϸ��̼� ������ ��
            m_stateMachine.Change_State((int)HallwayYandere.YandereState.ST_WAIT);
    }

    public override void Exit_State()
    {
    }
}
