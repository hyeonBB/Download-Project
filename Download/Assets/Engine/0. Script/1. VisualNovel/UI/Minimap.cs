using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Transform m_MinimapCamera;
    [SerializeField] private Transform m_Owner;
    //[SerializeField] private float m_lerpSpeed = 5.0f;

    public void Update_Minimap()
    {
        m_MinimapCamera.position = new Vector3(m_Owner.position.x, 500.0f, m_Owner.position.z);
        m_MinimapCamera.rotation = Quaternion.Euler(90.0f, m_Owner.eulerAngles.y, 0.0f); // �������� ȸ���� �����ͼ� �̴ϸ� ī�޶��� y �� ȸ�������� ����
        //m_MinimapCamera.rotation = Quaternion.Lerp(m_MinimapCamera.rotation, Quaternion.Euler(90.0f, m_Owner.eulerAngles.y, 0.0f), Time.deltaTime * m_lerpSpeed);
    }
}
