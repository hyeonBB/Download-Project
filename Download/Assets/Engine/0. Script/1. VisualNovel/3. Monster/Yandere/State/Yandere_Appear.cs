using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class Yandere_Appear : State<HallwayYandere>
    {
        private Animator m_animator;

        public Yandere_Appear(StateMachine<HallwayYandere> stateMachine) : base(stateMachine)
        {
            m_animator = m_stateMachine.Owner.GetComponentInChildren<Animator>();
        }

        public override void Enter_State()
        {
            GameObject clothA = m_stateMachine.Owner.transform.GetChild(0).GetChild(3).gameObject;
            clothA.GetComponent<SkinnedMeshRenderer>().materials[0].SetTexture("_BaseMap", Resources.Load<Texture2D>("1. Graphic/3D/1. VisualNovel/Character/Texture/Yandere/Pink_UV_ClothA_BaseMap"));

            m_animator.SetTrigger("IsAppear");
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
}

