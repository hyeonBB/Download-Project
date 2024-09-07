using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Collider[] m_Colliders;

    private void Awake()
    {
        // ��� �ݶ��̴� ���� �浹�� ����
        for (int i = 0; i < m_Colliders.Length; i++)
        {
            for (int j = i + 1; j < m_Colliders.Length; j++)
                Physics.IgnoreCollision(m_Colliders[i], m_Colliders[j], true);
        }
    }
}
