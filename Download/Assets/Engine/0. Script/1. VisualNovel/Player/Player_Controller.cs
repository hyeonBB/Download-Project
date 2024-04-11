using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform m_mainCamera;
    [SerializeField] private float m_moveSpeed = 5.0f;
    [SerializeField] private float m_lerpSpeed = 5.0f;

    [SerializeField] private Minimap m_Minimap;

    private Rigidbody m_rigidbody;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Input_Player();
        m_Minimap.Update_Minimap(); // �̴ϸ� ������Ʈ
    }

    private void Input_Player()
    {
        // ȸ��
        transform.rotation = m_mainCamera.rotation;

        // �̵�
        Vector3 Velocity = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            Velocity += transform.forward * m_moveSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.S))
            Velocity += -transform.forward * m_moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
            Velocity += transform.right * m_moveSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.A))
            Velocity += -transform.right * m_moveSpeed * Time.deltaTime;
        m_rigidbody.velocity = Vector3.Lerp(m_rigidbody.velocity, Velocity, Time.deltaTime * m_lerpSpeed);

        // float InputX = Input.GetAxis("Horizontal");
        // float InputZ = Input.GetAxis("Vertical");
        // if(InputX != 0.0f || InputZ != 0.0f)
        // {
        //     Vector3 localDirection = new Vector3(InputX, 0, InputZ);
        //     localDirection.Normalize();
        //     Vector3 worldDirection = transform.TransformDirection(localDirection); // �̵� ������ �÷��̾��� ���� ��ǥ�迡�� ���� ��ǥ��� ��ȯ
        //     worldDirection.Normalize();

        //     Vector3 rayOrigin = transform.position + new Vector3(0, 0.5f, 0);
        //#if UNITY_EDITOR
        //     Debug.DrawRay(rayOrigin, worldDirection * 0.4f, Color.red); // ���� ����� ����
        //#endif
        //     if (!Physics.Raycast(rayOrigin, worldDirection, 0.4f, LayerMask.GetMask("Wall"))) // ���� ������ �ش� �������� �̵�.
        //          transform.Translate(localDirection * m_moveSpeed * Time.deltaTime);           // Translate�� �ش� ���⿡ �ݶ��̴� ���θ� �˻����� �ʰ� �̵���.
        //     }
    }

    private void OnCollisionEnter(Collision collision)
    {
    }
}