using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class HorrorPlayer_Attack : HorrorPlayer_Base
    {
        private bool isAttak = false;
        private NoteItem.ITEMTYPE m_weaponType;

        private float m_soundTime = 0f;

        public HorrorPlayer_Attack(StateMachine<HorrorPlayer> stateMachine) : base(stateMachine)
        {
        }

        public override void Enter_State()
        {
            // ���� ���� ���� Ÿ�� �˻�
            NoteItem itemType = m_player.WeaponManagement.Get_CurrentWeaoponType();
            if(itemType == null)
            {
                m_player.StateMachine.Change_State(m_player.StateMachine.PreState);
                return;
            }

            // ���⿡ ���� ���� ���� ���� �Ǻ�
            m_weaponType = itemType.m_itemType;
            switch (m_weaponType)
            {
                case NoteItem.ITEMTYPE.TYPE_GUN:
                    if (Attak_Weapon() == false) return;
                    break;

                case NoteItem.ITEMTYPE.TYPE_FLASHLIGHT:
                    m_player.StateMachine.Change_State(m_player.StateMachine.PreState);
                    return;
            }    

            // ���� �ִϸ��̼� ����
            Change_Animation("Attack");
            m_soundTime = 0f;
        }

        public override void Update_State()
        {
            if (Input_Move() == true) // �̵� �Է� ���� ���� ��
            {
                if(m_player.StateMachine.PreState == (int)HorrorPlayer.State.ST_WALK)
                    Play_WalkSound(ref m_soundTime, 0.6f, 1f);
                else
                    Play_WalkSound(ref m_soundTime, 0.4f, 1f);
            }

            // ���� �ִϸ��̼� ���� Ȯ��
            if (m_animator.gameObject.activeSelf == false || m_animator.IsInTransition(0) == true) return;
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_triggerName) == true)
            {
                Reset_Animation();

                float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if(isAttak == false)
                {
                    switch (m_weaponType)
                    {
                        case NoteItem.ITEMTYPE.TYPE_PIPE:
                            if (animTime >= 0.35f)
                            {
                                Attak_Weapon();
                            }
                            break;
                    }
                }

                if (animTime >= 1.0f) // �ִϸ��̼� ����
                    m_player.StateMachine.Change_State(m_player.StateMachine.PreState);
            }
        }

        public override void Exit_State()
        {
            Reset_Animation();
            isAttak = false;
        }

        private bool Attak_Weapon()
        {
            if (m_player.WeaponManagement.Attack_Weapon() == false) // ���� �õ�, ���� ������ �����ΰ�?
            {
                m_player.StateMachine.Change_State(m_player.StateMachine.PreState);
                return false;
            }

            isAttak = true;
            return true;
        }
    }
}

