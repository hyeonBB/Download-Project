using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class HorrorPlayer_Idle : HorrorPlayer_Base
    {
        public HorrorPlayer_Idle(StateMachine<HorrorPlayer> stateMachine) : base(stateMachine)
        {
        }

        public override void Enter_State()
        {
            //Debug.Log("���̵� ���·� ��ȯ");
            Check_Stamina();

            Change_Animation("Idle");
        }

        public override void Update_State()
        {
            base.Update_State();

            if (m_animator.gameObject.activeSelf == true && m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_triggerName) == true)
                Reset_Animation();

            if (Input.GetMouseButtonDown(0)) // ���� ���콺 ��ư
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_ATTACK);
            }
            else if (Input.GetKey(KeyCode.Space) && m_player.Stamina > 0)
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_RUN);
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                m_player.StateMachine.Change_State((int)HorrorPlayer.State.ST_WALK);
            }
            else
            {
                Recover_Stamina();
                Input_Rotation();
                Input_Weapon();
                Input_Interaction();
            }
        }

        public override void Exit_State()
        {
            Reset_Animation();
        }
    }
}

