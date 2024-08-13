using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straitjacket_Run : Straitjacket_Base
{
    public Straitjacket_Run(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_speed = 5f;
    }

    public override void Enter_State()
    {
        base.Enter_State();

        Debug.Log("�÷��̾� �߰�");
    }

    public override void Update_State()
    {
        if (Change_Attack() == false)
        {
            // �÷��̾� ����
            Vector3 direction = (HorrorManager.Instance.Player.transform.position - m_owner.transform.position).normalized;
            Vector3 newPos = m_owner.transform.position + direction * m_speed * Time.deltaTime;

            m_owner.transform.position = newPos;
            m_owner.transform.LookAt(HorrorManager.Instance.Player.transform);

            //// �̵��� �� �ִ� ��ġ���� Ȯ��
            //if (m_owner.Spawner.Check_Position(newPos))
            //{
            //    m_owner.transform.position = newPos;
            //}
            //else
            //{
            //    // �̵��� �� ���� ��쿡�� ������ ��ȯ�� �� �ִ� ������ �߰��� �� �ֽ��ϴ�.
            //    Debug.Log("�̵��� �� ���� ��ġ");
            //}
        }
    }

    public override void Exit_State()
    {
        base.Exit_State();
    }
}
