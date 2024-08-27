using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class HorrorPlayer_Attack : HorrorPlayer_Base
    {
        public HorrorPlayer_Attack(StateMachine<HorrorPlayer> stateMachine) : base(stateMachine)
        {
        }

        public override void Enter_State()
        {
            //Debug.Log("���� ���·� ��ȯ");
            if (m_player.WeaponManagement.Attack_Weapon() == false) // ���� ������ �����ΰ�?
            {
                m_player.StateMachine.Change_State(m_player.StateMachine.PreState);
                return;
            }

            Change_Animation("Attack");
        }

        public override void Update_State()
        {
            base.Update_State();

            // ���� �ִϸ��̼� ���� Ȯ��
            if (m_animator.gameObject.activeSelf == true && m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_triggerName) == true)
            {
                Reset_Animation();

                float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (animTime >= 1.0f) // �ִϸ��̼� ����
                    m_player.StateMachine.Change_State(m_player.StateMachine.PreState);
            }
        }

        public override void Exit_State()
        {
            Reset_Animation();
        }
    }
}

