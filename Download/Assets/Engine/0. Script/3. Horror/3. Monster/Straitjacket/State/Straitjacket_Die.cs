using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straitjacket_Die : Straitjacket_Base
{
    public Straitjacket_Die(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        base.Enter_State();

        // ��� �̺�Ʈ ó��
        m_animator.SetBool("IsDie", true);

        GameObject gameObject = m_owner.gameObject;
        GameManager.Instance.Destroy_GameObject(ref gameObject);
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
        base.Exit_State();
    }
}
