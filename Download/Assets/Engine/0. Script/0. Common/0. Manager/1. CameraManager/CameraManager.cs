using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CAMERATYPE { CT_BASIC_2D, CT_BASIC_3D, CT_CUTSCENE, CT_FOLLOW, CT_END };

public class CameraManager : MonoBehaviour
{
    private static CameraManager m_instance = null;
    public static CameraManager Instance
    {
        get //���� �Ŵ��� �ν��Ͻ��� ������ �� �ִ� ������Ƽ. static�̹Ƿ� �ٸ� Ŭ�������� ȣ�� ����
        {
            if (null == m_instance)
                return null;
            return m_instance;
        }
    }


    private CAMERATYPE m_currentCameraType = CAMERATYPE.CT_END;
    private CameraBase[] m_cameras;


    private void Awake()
    {
        if (null == m_instance)
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject); //�� ��ȯ�� �Ǵ��� �ı����� ����

            m_cameras = new CameraBase[CAMERATYPE.GetValues(typeof(CAMERATYPE)).Length]; // �ʱ�ȭ
            m_cameras[(int)CAMERATYPE.CT_BASIC_2D] = new CameraBasic_2D();
            m_cameras[(int)CAMERATYPE.CT_BASIC_2D].Initialize_Camera();
            m_cameras[(int)CAMERATYPE.CT_BASIC_3D] = new CameraBasic_3D();
            m_cameras[(int)CAMERATYPE.CT_BASIC_3D].Initialize_Camera();
            m_cameras[(int)CAMERATYPE.CT_CUTSCENE] = new CameraCutscene();
            m_cameras[(int)CAMERATYPE.CT_CUTSCENE].Initialize_Camera();
            m_cameras[(int)CAMERATYPE.CT_FOLLOW] = new CameraFollow();
            m_cameras[(int)CAMERATYPE.CT_FOLLOW].Initialize_Camera();
        }
        else
        {
            Destroy(this.gameObject); //�̹� ���������� instance�� �ν��Ͻ��� �����Ѵٸ� �ڽ��� ����
        }
    }

    private void LateUpdate()
    {
        if (m_currentCameraType == CAMERATYPE.CT_END)
            return;

        m_cameras[(int)m_currentCameraType].Update_Camera();
    }

    public void Change_Camera(CAMERATYPE type)
    {
        if (type == m_currentCameraType)
            return;

        // Ż��
        if(m_currentCameraType != CAMERATYPE.CT_END)
            m_cameras[(int)m_currentCameraType].Exit_Camera();

        // ��ü
        m_currentCameraType = type;

        // ����
        m_cameras[(int)m_currentCameraType].Enter_Camera();
    }

    public CameraBase Get_CurCamera()
    {
        return m_cameras[(int)m_currentCameraType];
    }
}
