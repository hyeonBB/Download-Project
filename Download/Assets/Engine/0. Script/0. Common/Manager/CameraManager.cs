using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraTypes { Follow, Cutscene };

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform cameraTr; // ���� ī�޶�
    [SerializeField] private Transform targetTr; // ī�޶� Ÿ��

    [SerializeField] private float positionDistance;
    [SerializeField] private float positionHeight;
    [SerializeField] private float lookAtHeight;

    [SerializeField] private float damping = 10.0f;

    private Vector3 velocity = Vector3.zero;
    private CameraTypes CameraType;

    void Start()
    {
        //CameraType = CameraTypes.Follow;

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
        Vector3 pos = targetTr.position
            + (-targetTr.forward * positionDistance)
            + (Vector3.up * positionHeight);
        cameraTr.position = Vector3.SmoothDamp(cameraTr.position, pos, ref velocity, damping);

        cameraTr.LookAt(targetTr.position + (targetTr.up * lookAtHeight));
    }

    private void Change_Camera(CameraTypes type)
    {
        // ī�޶� Ÿ�Ը��� Ŭ������ ����� �����ϱ�
        // ����/ ����/ Ż��
        // Cursor.lockState = CursorLockMode.Locked; ���Կ� ���� �߰��ϱ�
    }
}
