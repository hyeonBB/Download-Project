using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Rest : Boss1F_Base
{
    private float m_time = 0f;

    public Boss1F_Rest(StateMachine<Monster> stateMachine) : base(stateMachine)
    {

    }

    public override void Enter_State()
    {
        m_time = 0f;
        Debug.Log("���� ���");
    }

    public override void Update_State()
    {
        m_time += Time.deltaTime;
        if(m_time >= 1f)
        {
            Change_Patterns(); // ���� ���� ����
        }
    }

    public override void Exit_State()
    {

    }
}
