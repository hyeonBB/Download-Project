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
        private bool m_switch = false;

        public Yandere_Wait(StateMachine<HallwayYandere> stateMachine) : base(stateMachine)
        {
            m_animator = m_stateMachine.Owner.GetComponentInChildren<Animator>();
            m_agent = m_stateMachine.Owner.GetComponent<NavMeshAgent>();
        }

        public override void Enter_State()
        {
            m_animator.SetBool("IsRun", true);
            m_agent.speed = 10f;

            //GameManager.Instance.UI.Start_FadeOut(1f, Color.black, () => Continue_Play(), 0f, false);
            // VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Chase>().Change_Text("����... �������� ���� �����ſ���.");
        }

        public override void Update_State()
        {
            // Ư�� �������� �̵� �� �ش� ��ġ���� ���
            m_agent.destination = new Vector3(0f, 0f, 10f);

            // ��ȯ
            if(m_switch == false && Camera.main.transform.position.z < 11.5f)
            {
                m_switch = true;
                GameManager.Instance.UI.Start_FadeOut(0.8f, Color.black, () => Continue_Play(), 0f, false);
            }

        }

        public override void Exit_State()
        {
            m_agent.speed = 5f;
        }

        private void Continue_Play() // �ƾ� ��� �� ���� ������
        {
            Camera.main.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("2. Sound/1. VisualNovel/BGM/�߰ݰ��� BGM");
            Camera.main.GetComponent<AudioSource>().Play();

            GameManager.Instance.Camera.Change_Camera(CAMERATYPE.CT_FOLLOW);
            Novel_Chase novel_Chase = VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Chase>();
            novel_Chase.Stage.transform.GetChild(0).gameObject.SetActive(true); // �̴ϸ� ī�޶�
            novel_Chase.Stage.transform.GetChild(1).gameObject.SetActive(true); // �̴ϸ� UI
            novel_Chase.ItemText.GetComponent<ItemText>().Start_ItemText("������...");
            novel_Chase.Player.Set_Lock(false);
            novel_Chase.Player.MoveSpeed = 400f;

            m_stateMachine.Change_State((int)HallwayYandere.YandereState.ST_CHASE);
            GameManager.Instance.UI.Start_FadeIn(1f, Color.black);
        }
    }
}


