using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_Die : Bug_Base
{
    public Bug_Die(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        // ��� �̺�Ʈ ó��
        Debug.Log("Bug ���");

        GameObject gameObject = m_owner.gameObject;
        GameManager.Instance.Destroy_GameObject(ref gameObject);
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
    }
}
