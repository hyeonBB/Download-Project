using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraCutscene : CameraBase
{
    private bool m_isCutscene = false;
    private bool m_isFov = false;

    private Vector3 m_targetPosition = Vector3.zero; // ī�޶� �̵��� ��ǥ ��ġ
    private Vector3 m_targetRotation = Vector3.zero; // ī�޶� ȸ���� ��ǥ ����
    private float m_targetFOV = 0f; // ��ǥ �þ߰�
    private float m_moveSpeed     = 0f; // ī�޶� �̵� �ӵ�
    private float m_rotationSpeed = 0f; // ī�޶� ȸ�� �ӵ�
    private float m_fovSpeed = 0f; // �þ߰� ��ȯ �ӵ�

    public bool IsCutscene => m_isCutscene;
    public bool IsFov => m_isFov;

    public override void Initialize_Camera()
    {
    }

    public override void Enter_Camera()
    {
        base.Enter_Camera();
        m_isCutscene = false;
        m_isFov = false;
    }

    public override void Update_Camera()
    {
        base.Update_Camera();

        if (m_isCutscene == true)
        {
            m_mainCamera.position = Vector3.MoveTowards(m_mainCamera.position, m_targetPosition, m_moveSpeed * Time.deltaTime);
            m_mainCamera.rotation = Quaternion.Slerp(m_mainCamera.rotation, Quaternion.Euler(m_targetRotation), m_rotationSpeed * Time.deltaTime);

            if (m_mainCamera.position == m_targetPosition)
                m_isCutscene = false;
        }

        if (m_isFov == true)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, m_targetFOV, m_fovSpeed * Time.deltaTime);
            if (Camera.main.fieldOfView == m_targetFOV)
            {
                m_isFov = false;
            }
        }
    }

    public override void Exit_Camera()
    {
        // �ʱ�ȭ
        m_mainCamera.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }

    public void Start_Cutscene(Vector3 targetPosition, Vector3 targetRotation, float moveSpeed, float rotationSpeed)
    {
        m_isCutscene = true;
        m_targetPosition = targetPosition;
        m_targetRotation = targetRotation;
        m_moveSpeed      = moveSpeed;
        m_rotationSpeed  = rotationSpeed;
    }

    public void Start_FOV(float targetFov, float speedFov)
    {
        m_isFov = true;
        m_targetFOV = targetFov;
        m_fovSpeed = speedFov;
    }
}
