using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraCutscene : CameraBase
{
    private bool m_isCutscene = false;

    public Vector3 m_targetPosition = Vector3.zero; // ī�޶� �̵��� ��ǥ ��ġ
    public Vector3 m_targetRotation = Vector3.zero; // ī�޶� ȸ���� ��ǥ ����
    public float m_moveSpeed     = 0f; // ī�޶� �̵� �ӵ�
    public float m_rotationSpeed = 0f; // ī�޶� ȸ�� �ӵ�

    public override void Initialize_Camera()
    {
    }

    public override void Enter_Camera()
    {
        m_mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
    }

    public override void Update_Camera()
    {
        if (m_isCutscene)
        {
            m_mainCamera.position = Vector3.MoveTowards(m_mainCamera.position, m_targetPosition, m_moveSpeed * Time.deltaTime);
            m_mainCamera.rotation = Quaternion.Slerp(m_mainCamera.rotation, Quaternion.Euler(m_targetRotation), m_rotationSpeed * Time.deltaTime);

            if (m_mainCamera.position == m_targetPosition)
                m_isCutscene = false;
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
}
