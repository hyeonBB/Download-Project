using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraTypes { Follow, Cutscene };

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject MainCamera;   // ���� ī�޶�
    [SerializeField] private GameObject CameraTarget; // ī�޶� Ÿ��

    [SerializeField] private Vector3 Offset = new Vector3(0.0f, 1.5f, 3.0f);
    [SerializeField] private float MouseSpeed = 100.0f;
    [SerializeField] private float LerpSpeed = 5.0f;

    private CameraTypes CameraType;

    void Start()
    {
        CameraType = CameraTypes.Follow;

        // ���콺 Ŀ�� ����
        // Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        switch (CameraType)
        {
            case CameraTypes.Follow:
                Follow_Camera();
                break;

            case CameraTypes.Cutscene:
                break;
        }
    }

    private void Follow_Camera()
    {
        float MouseX = Input.GetAxis("Mouse X") * MouseSpeed * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * MouseSpeed * Time.deltaTime;

        Vector3 TargetPos = CameraTarget.transform.position;
        TargetPos.x += Offset.x;
        TargetPos.y += Offset.y;

        // ī�޶� ȸ��
        MainCamera.transform.RotateAround(TargetPos, Vector3.up, MouseX);                  // ���� ȸ��
        //MainCamera.transform.RotateAround(TargetPos, MainCamera.transform.right, -MouseY); // ���� ȸ��

        // Ÿ�� ���󰡱�
        Vector3 Position = TargetPos - MainCamera.transform.forward * Offset.z; // Ÿ�� ������ ī�޶� �̵�
        MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, Position, Time.deltaTime * LerpSpeed); // �̵� ����
    }

    private void Change_Camera(CameraTypes type)
    {
        // ī�޶� Ÿ�Ը��� Ŭ������ ����� �����ϱ�
        // ����/ ����/ Ż��
        // Cursor.lockState = CursorLockMode.Locked; ���Կ� ���� �߰��ϱ�
    }
}
