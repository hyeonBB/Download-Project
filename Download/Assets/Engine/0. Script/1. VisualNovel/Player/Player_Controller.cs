using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed = 5.0f;
    [SerializeField] private float m_turnSpeed = 500.0f;

    private Rigidbody m_rigidbodyCom;

    void Awake()
    {
        m_rigidbodyCom = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Input_Player();
    }

    private void Input_Player()
    {
        float InputX = Input.GetAxis("Horizontal");
        float InputZ = Input.GetAxis("Vertical");
        float MouseX = Input.GetAxis("Mouse X");

        // �÷��̾� �̵�
        if (InputX != 0.0f || InputZ != 0.0f)
        {
            Vector3 Dir = (transform.forward * InputZ) + (transform.right * InputX);
            // transform.Translate(Dir.normalized * m_moveSpeed * Time.deltaTime);
            m_rigidbodyCom.MovePosition(transform.position + Dir.normalized * m_moveSpeed * Time.deltaTime); // MovePosition : �������� ������ ǥ���� �� ���
        }

        // �÷��̾� ȸ��
        transform.Rotate(Vector3.up * m_turnSpeed * Time.deltaTime * MouseX);
    }
}