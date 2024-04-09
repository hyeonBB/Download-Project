using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform m_mainCamera;
    [SerializeField] private float m_moveSpeed = 5.0f;

    void FixedUpdate()
    {
        Input_Player();
    }

    private void Input_Player()
    {
        // ȸ��
        transform.rotation = m_mainCamera.rotation;

        // �̵�
        float InputX = Input.GetAxis("Horizontal");
        float InputZ = Input.GetAxis("Vertical");
        if(InputX != 0.0f || InputZ != 0.0f)
        {
            Vector3 localDirection = new Vector3(InputX, 0, InputZ);
            localDirection.Normalize();
            Vector3 worldDirection = transform.TransformDirection(localDirection); // �̵� ������ �÷��̾��� ���� ��ǥ�迡�� ���� ��ǥ��� ��ȯ
            worldDirection.Normalize();

            Vector3 rayOrigin = transform.position + new Vector3(0, 0.5f, 0);
#if UNITY_EDITOR
            Debug.DrawRay(rayOrigin, worldDirection * 0.4f, Color.red); // ���� ����� ����
#endif
            if (!Physics.Raycast(rayOrigin, worldDirection, 0.4f, LayerMask.GetMask("Wall"))) // ���� ������ �ش� �������� �̵�.
                transform.Translate(localDirection * m_moveSpeed * Time.deltaTime);           // Translate�� �ش� ���⿡ �ݶ��̴� ���θ� �˻����� �ʰ� �̵���.
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
    }
}