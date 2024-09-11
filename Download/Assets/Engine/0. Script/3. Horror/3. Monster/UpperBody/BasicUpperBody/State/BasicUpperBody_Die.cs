using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUpperBody_Die : BasicUpperBody_Base
{
    public BasicUpperBody_Die(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        // ������ ���� ����Ʈ ����
        // 

        // 2�ܰ� �� ����
        GameObject gameObject = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/Monster/FastUpperBody");
        if (gameObject == null)
            return;
        Monster monster = gameObject.GetComponent<Monster>();
        if (monster == null)
            return;
        monster.Initialize_Monster(m_owner.Spawner);
        monster.gameObject.transform.position = m_owner.gameObject.transform.position;
        monster.gameObject.transform.rotation = m_owner.gameObject.transform.rotation;

        // 1�ܰ� �� �Ҹ�
        GameManager.Ins.Resource.Destroy(m_owner.gameObject);
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
    }
}
