using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Tentacle : Boss1F_Base
{
    private bool m_recall = false;
    private float m_time = 0f;

    public Boss1F_Tentacle(StateMachine<Monster> stateMachine) : base(stateMachine)
    {

    }

    public override void Enter_State()
    {
        if (m_recall == false)
        {
            m_recall = true;

            if (Change_Recall() == true)
                return;
        }

        Create_Tentacle();
        Debug.Log("�˼� 1�� ����");
    }

    public override void Update_State()
    {
        m_time += Time.deltaTime;
        if (m_time >= 2f) // �̺�Ʈ ���� Ȥ�� ��� �ð�
        {
            m_recall = false;
            m_stateMachine.Change_State((int)Boss1F.State.ST_REST);
        }
    }

    public override void Exit_State()
    {

    }
}
