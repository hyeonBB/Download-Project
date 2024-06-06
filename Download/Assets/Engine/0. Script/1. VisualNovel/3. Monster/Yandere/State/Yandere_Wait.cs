using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace VisualNovel
{
    public class Yandere_Wait : State<HallwayYandere>
    {
        private Animator m_animator;
        private NavMeshAgent m_agent;

        public Yandere_Wait(StateMachine<HallwayYandere> stateMachine) : base(stateMachine)
        {
            m_animator = m_stateMachine.Owner.GetComponentInChildren<Animator>();
            m_agent = m_stateMachine.Owner.GetComponent<NavMeshAgent>();
        }

        public override void Enter_State()
        {
            m_animator.SetBool("IsRun", true); // �ִϸ��̼� ����
            m_agent.speed = 10f;
            UIManager.Instance.Start_FadeOut(1f, Color.black, () => Continue_Play(), 0f, false);
        }

        public override void Update_State()
        {
            // Ư�� �������� �̵� �� �ش� ��ġ���� ���
            m_agent.destination = new Vector3(0f, 0f, 10f);
        }

        public override void Exit_State()
        {
            m_agent.speed = 5f;
        }

        private void Continue_Play() // �ƾ� ��� �� ���� ������
        {
            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_FOLLOW);
            VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Chase>().Player.Set_Lock(false);

            // �� �� ��� �Ϸ� �� ���ΰ� �������� ���ƿ��� �� "������..." ��� ���
            VisualNovelManager.Instance.PlayerObj.GetComponent<HallwayPlayer>().MoveSpeed = 400f;
            VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Chase>().ItemText.GetComponent<ItemText>().Start_ItemText("������...");

            m_stateMachine.Change_State((int)HallwayYandere.YandereState.ST_CHASE);
            UIManager.Instance.Start_FadeIn(1f, Color.black);
        }
    }
}


