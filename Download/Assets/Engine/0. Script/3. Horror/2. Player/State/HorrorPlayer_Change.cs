using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class HorrorPlayer_Change : HorrorPlayer_Base
    {
        private float m_soundTime = 0f;

        public HorrorPlayer_Change(StateMachine<HorrorPlayer> stateMachine) : base(stateMachine)
        {
        }

        public override void Enter_State()
        {
            Change_Animation("Hold", true);
            m_soundTime = 0f;

            // ���� ���� ��Ȱ��ȭ
            if (m_player.WeaponManagement.PreWeapon != -1)
            {
                m_player.WeaponManagement.Weapons[m_player.WeaponManagement.PreWeapon].gameObject.SetActive(false);
                m_player.WeaponManagement.Weapons[m_player.WeaponManagement.PreWeapon].gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }

            // ���� ���� Ȱ��ȭ
            m_player.WeaponManagement.Weapons[m_player.WeaponManagement.CurWeapon].gameObject.SetActive(true);
        }

        public override void Update_State()
        {
            if (Input_Move() == true) // �̵� �Է� ���� ���� ��
            {
                if (m_player.StateMachine.PreState == (int)HorrorPlayer.State.ST_WALK)
                    Play_WalkSound(ref m_soundTime, 0.6f, 1f);
                else
                    Play_WalkSound(ref m_soundTime, 0.4f, 1f);
            }

            if (m_animator.gameObject.activeSelf == false || m_animator.IsInTransition(0) == true) return;
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_triggerName) == true)
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
            m_player.WeaponManagement.Weapons[m_player.WeaponManagement.CurWeapon].gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}

