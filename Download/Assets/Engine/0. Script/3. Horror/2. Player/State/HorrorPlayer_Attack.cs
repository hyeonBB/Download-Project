using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class HorrorPlayer_Attack : HorrorPlayer_Base
    {
        private bool isAttak = false;
        private NoteItem.ITEMTYPE m_weaponType;

        public HorrorPlayer_Attack(StateMachine<HorrorPlayer> stateMachine) : base(stateMachine)
        {
        }

        public override void Enter_State()
        {
            //Debug.Log("���� ���·� ��ȯ");
            NoteItem itemType = m_player.WeaponManagement.Get_CurrentWeaoponType();
            if(itemType == null)
            {
                m_player.StateMachine.Change_State(m_player.StateMachine.PreState);
                return;
            }

            m_weaponType = itemType.m_itemType;
            switch (m_weaponType)
            {
                case NoteItem.ITEMTYPE.TYPE_GUN:
                    Attak_Weapon();
                    break;

                case NoteItem.ITEMTYPE.TYPE_FLASHLIGHT:
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
                if(isAttak == false)
                {
                    switch (m_weaponType)
                    {
                        case NoteItem.ITEMTYPE.TYPE_PIPE:
                            if (animTime >= 0.35f) Attak_Weapon();
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

        private void Attak_Weapon()
        {
            if (m_player.WeaponManagement.Attack_Weapon() == false) // ���� �õ�, ���� ������ �����ΰ�?
            {
                m_player.StateMachine.Change_State(m_player.StateMachine.PreState);
                return;
            }

            isAttak = true;
        }
    }
}

