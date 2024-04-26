using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Transform m_MinimapCamera;
    //[SerializeField] private float m_lerpSpeed = 5.0f;

    public void LateUpdate()
    {
        if(VisualNovelManager.Instance.PlayerTr != null)
        {
            m_MinimapCamera.position = new Vector3(VisualNovelManager.Instance.PlayerTr.position.x, 500.0f, VisualNovelManager.Instance.PlayerTr.position.z);
            m_MinimapCamera.rotation = Quaternion.Euler(90.0f, VisualNovelManager.Instance.PlayerTr.eulerAngles.y, 0.0f); // �������� ȸ���� �����ͼ� �̴ϸ� ī�޶��� y �� ȸ�������� ����                                                                               //m_MinimapCamera.rotation = Quaternion.Lerp(m_MinimapCamera.rotation, Quaternion.Euler(90.0f, m_Owner.eulerAngles.y, 0.0f), Time.deltaTime * m_lerpSpeed)
        }
    }
}
