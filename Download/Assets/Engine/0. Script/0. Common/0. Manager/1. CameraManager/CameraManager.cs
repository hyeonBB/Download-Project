using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CAMERATYPE { CT_FOLLOW, CT_END };

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CAMERATYPE m_currentCameraType;
    private CameraBase[] m_cameras;

    private void Awake()
    {
        m_cameras = new CameraBase[CAMERATYPE.GetValues(typeof(CAMERATYPE)).Length]; // �ʱ�ȭ
        m_cameras[(int)CAMERATYPE.CT_FOLLOW] = new CameraFollow();
        m_cameras[(int)CAMERATYPE.CT_FOLLOW].Initialize_Camera();
    }

    private void Start()
    {
        // Change_Camera(CAMERATYPE.CT_FOLLOW);
    }

    private void LateUpdate()
    {
        if (m_currentCameraType == CAMERATYPE.CT_END)
            return;

        m_cameras[(int)m_currentCameraType].Update_Camera();
    }

    public void Change_Camera(CAMERATYPE type)
    {
        // Ż��
        m_cameras[(int)m_currentCameraType].Exit_Camera();

        // ��ü
        m_currentCameraType = type;

        // ����
        m_cameras[(int)m_currentCameraType].Enter_Camera();
    }
}
