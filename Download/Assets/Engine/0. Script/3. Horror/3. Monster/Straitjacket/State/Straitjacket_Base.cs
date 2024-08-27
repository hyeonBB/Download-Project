using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straitjacket_Base : State<Monster>
{
    protected Straitjacket m_owner  = null;
    protected Animator  m_animator  = null;
    protected Rigidbody m_rigidbody = null;

    protected Vector3 m_moveDirection;
    protected float   m_speed = 5f;

    protected float m_chaseDist  = 5f;
    protected float m_attackDist = 2.5f;

    public Straitjacket_Base(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_owner = m_stateMachine.Owner.GetComponent<Straitjacket>();
        m_animator = m_owner.Animator;

        m_rigidbody = m_owner.gameObject.GetComponent<Rigidbody>();
        m_rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        m_rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
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

    protected bool Change_Run() // �������� ��(������)�� ���ΰ��� �� �� ���ΰ��� ���� �޷����.
    {
        // �÷��̾ ���� ���� ���� �����ϸ� �߰� ���� ��ȯ
        float distanceToPlayer = Vector3.Distance(m_stateMachine.Owner.transform.position, HorrorManager.Instance.Player.transform.position);
        if (distanceToPlayer <= m_chaseDist)
        {
            m_stateMachine.Change_State((int)Straitjacket.State.ST_RUN); // �߰� ���·� ��ȯ
            return true;
        }

        return false;
    }

    protected bool Change_Attack()
    {
        // �÷��̾ ���� ���� ���� �����ϸ� ���� ���� ��ȯ
        float distanceToPlayer = Vector3.Distance(m_stateMachine.Owner.transform.position, HorrorManager.Instance.Player.transform.position);
        if (distanceToPlayer <= m_attackDist)
        {
            m_stateMachine.Change_State((int)Straitjacket.State.ST_ATTACK); // ���� ���·� ��ȯ
            return true;
        }

        return false;
    }

    protected void Move_Monster()
    {
        Vector3 newPos = m_owner.gameObject.transform.position + m_moveDirection * m_speed * Time.deltaTime;
        if (m_owner.Spawner != null && m_owner.Spawner.Check_Position(newPos) == true && Check_Collider(m_moveDirection) == false)
        {
            m_owner.transform.forward  = m_moveDirection.normalized;
            m_owner.transform.position = newPos;
        }
        else
            Reset_RandomDirection();
    }

    protected void Reset_RandomDirection()
    {
        Vector3 newDir = Random.insideUnitSphere;
        newDir = newDir.normalized;
        newDir.y = 0;

        Vector3 newPos = m_owner.gameObject.transform.position + newDir * m_speed * Time.deltaTime;
        if (m_owner.Spawner != null && m_owner.Spawner.Check_Position(newPos) == true && Check_Collider(newDir) == false)
            m_moveDirection = newDir;
        else
            m_moveDirection = -m_moveDirection;
    }

    private bool Check_Collider(Vector3 dir)
    {
        RaycastHit hit = GameManager.Instance.Start_Raycast(m_owner.transform.position, dir, 1f, LayerMask.GetMask("Wall", "Static", "Interaction"));
        if (hit.collider != null)
            return true;

        return false;
    }
}
