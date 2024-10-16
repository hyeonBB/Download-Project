using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Weakness : Boss1F_Base // ��������, �±�, �ö󰡱�
{
    public Boss1F_Weakness(StateMachine<Monster> stateMachine) : base(stateMachine)
    {

    }

    public override void Enter_State()
    {
        m_owner.CumulativeDamage = 0;

        m_animator.speed = 1f;
        m_animator.SetBool("IsWeakDown", true);
    }

    public override void Update_State()
    {
        /*
         *  ��ü�� ���� �������� ���ϸ� *����������°� �ȴ�
     *�����������: �ϰ� �ִ� ������ ��� ���� ä ���� ������ �� �þ����� ���������� �Լ��� ���� �����Ų ����.
	         ���̶� ���� ������ �κ��� �����ϸ� �⺻ �������� ����, ���� �����ϸ� �������� 2.5��� ����.
	         ������ ���� ���´� 5�ʰ� �����Ѵ�.(������ �ٽ� ������� �Ͼ�� �Դݰ� �������� ����)

         */


        /*
         * . ���� ������°� �Ǿ� �ϴ� ������ ����ٸ�, ������ ������������ �����Ѵ�
    (ex. ���� ������ ABCDE�� �ִٸ�..... �� �� ������ �κ� ������ �ű⼭���� �ٽ� ����: C�� �����ؼ�DE...�ٽ� ABCDE������ ���� ����)

         */
    }

    public override void Exit_State()
    {

    }
}
