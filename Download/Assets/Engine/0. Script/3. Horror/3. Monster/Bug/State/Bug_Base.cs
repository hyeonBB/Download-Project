using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_Base : State<Monster>
{
    protected Bug m_owner = null;

    protected float m_attackDist = 1.5f;
    protected float m_chaseDist = 3f;

    protected Animator m_animator = null;
    protected AudioSource m_audioSource = null;

    public Bug_Base(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_owner    = m_stateMachine.Owner.GetComponent<Bug>();

        m_animator = m_owner.Animator;
        m_audioSource = m_stateMachine.Owner.GetComponent<AudioSource>();
    }

    public override void Enter_State()
    {
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
    }

    protected bool Change_FLY()
    {
        // �÷��̾ ���� ���� ���� �����ϸ� ���� ���� ��ȯ
        float distanceToPlayer = Vector3.Distance(m_stateMachine.Owner.transform.position, HorrorManager.Instance.Player.transform.position);
        if (distanceToPlayer <= m_chaseDist)
        {
            m_stateMachine.Change_State((int)Bug.State.ST_FLY); // ���� ���·� ��ȯ
            return true;
        }

        return false;
    }

    public override void OnDrawGizmos()
    {
#if UNITY_EDITOR
        // �߰� ���� ǥ��
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(m_stateMachine.Owner.transform.position, m_chaseDist);

        // ���� ���� ǥ��
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_stateMachine.Owner.transform.position, m_attackDist);
#endif
    }

    protected Vector3 Calculate_BezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) // Bezier curve calculation method
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;

        /*Vector3 M0 = Vector3.Lerp(p0, p1, t);
        Vector3 M1 = Vector3.Lerp(p1, p2, t);
        Vector3 M2 = Vector3.Lerp(p2, p3, t);

        Vector3 B0 = Vector3.Lerp(M0, M1, t);
        Vector3 B1 = Vector3.Lerp(M1, M2, t);

        return Vector3.Lerp(B0, B1, t);*/
    }
}
