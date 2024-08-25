using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraCutscene : CameraBase
{
    private bool m_isPosition = false;
    private bool m_isRotation = false;
    private bool m_isFov = false;

    private Vector3 m_targetPosition = Vector3.zero; // ī�޶� �̵��� ��ǥ ��ġ
    private Quaternion m_targetRotation = Quaternion.identity; // ī�޶� ȸ���� ��ǥ ����
    private float m_targetFOV = 0f; // ��ǥ �þ߰�
    private float m_moveSpeed     = 0f; // ī�޶� �̵� �ӵ�
    private float m_rotationSpeed = 0f; // ī�޶� ȸ�� �ӵ�
    private float m_fovSpeed = 0f; // �þ߰� ��ȯ �ӵ�

    public bool IsPosition => m_isPosition;
    public bool IsRotation => m_isRotation;
    public bool IsFov => m_isFov;

    public override void Initialize_Camera()
    {
    }

    public override void Enter_Camera()
    {
        base.Enter_Camera();
        m_isPosition = false;
        m_isRotation = false;
        m_isFov = false;
    }

    public override void Update_Camera()
    {
        base.Update_Camera();

        if (m_isPosition == true)
        {
            m_mainCamera.position = Vector3.MoveTowards(m_mainCamera.position, m_targetPosition, m_moveSpeed * Time.deltaTime);
            if (m_mainCamera.position == m_targetPosition)
                m_isPosition = false;
        }

        if(m_isRotation == true)
        {
            m_mainCamera.rotation = Quaternion.Slerp(m_mainCamera.rotation, m_targetRotation, m_rotationSpeed * Time.deltaTime);
            if (m_mainCamera.rotation == m_targetRotation)
                m_isRotation = false;
        }

        if (m_isFov == true)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, m_targetFOV, m_fovSpeed * Time.deltaTime);
            if (Camera.main.fieldOfView == m_targetFOV)
                m_isFov = false;
        }
    }

    public override void Exit_Camera()
    {
        if (m_mainCamera == null)
            return;

        // �ʱ�ȭ
        m_mainCamera.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }

    public void Start_Position(Vector3 targetPosition, float moveSpeed)
    {
        m_isPosition = true;
        m_targetPosition = targetPosition;
        m_moveSpeed      = moveSpeed;
    }

    public void Start_Rotation(Vector3 targetRotation, float rotationSpeed)
    {
        m_isRotation = true;
        m_targetRotation = Quaternion.Euler(targetRotation);
        m_rotationSpeed = rotationSpeed;
    }

    public void Start_FOV(float targetFov, float speedFov)
    {
        m_isFov = true;
        m_targetFOV = targetFov;
        m_fovSpeed = speedFov;
    }
}
