using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class Minimap : MonoBehaviour
    {
        [SerializeField] private Transform m_MinimapCamera;
        private Transform m_PlayerTr;

        private void Start()
        {
            m_PlayerTr = VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Chase>().PlayerTr;
        }

        private void LateUpdate()
        {
            if (m_PlayerTr == null)
                return;

            m_MinimapCamera.position = new Vector3(m_PlayerTr.position.x, 500.0f, m_PlayerTr.position.z);
            m_MinimapCamera.rotation = Quaternion.Euler(90.0f, m_PlayerTr.eulerAngles.y, 0.0f); // �������� ȸ���� �����ͼ� �̴ϸ� ī�޶��� y �� ȸ�������� ����                                                                               //m_MinimapCamera.rotation = Quaternion.Lerp(m_MinimapCamera.rotation, Quaternion.Euler(90.0f, m_Owner.eulerAngles.y, 0.0f), Time.deltaTime * m_lerpSpeed)
        }
    }
}

