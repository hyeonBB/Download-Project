using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject MainCamera;   // ���� ī�޶�
    [SerializeField] private float MoveSpeed = 5.0f;
    [SerializeField] private float TurnSpeed = 600.0f;

    private Rigidbody RigidbodyCom;

    void Start()
    {
        RigidbodyCom = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

    }

    void Update()
    {
        Input_Player();
    }

    private void Input_Player()
    {
        float InputX = Input.GetAxis("Horizontal");
        float InputZ = Input.GetAxis("Vertical");

        if (InputX != 0.0f || InputZ != 0.0f)
        {
            // ī�޶� �ٶ󺸴� �������� ȸ��
            Vector3 CameraDirection = MainCamera.transform.forward;
            CameraDirection.y = 0f;
            Quaternion Rotation = Quaternion.LookRotation(CameraDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Rotation, Time.deltaTime * TurnSpeed);

            // ȸ���� �Ϸ�Ǹ� �̵�
            if (Quaternion.Angle(transform.rotation, Rotation) < 0.1f)
            {
                Vector3 Velocity = (transform.forward * InputZ + transform.right * InputX).normalized * MoveSpeed;
                RigidbodyCom.MovePosition(transform.position + Velocity * Time.deltaTime); // MovePosition : �������� ������ ǥ���� �� ���
            }
        }
    }
}