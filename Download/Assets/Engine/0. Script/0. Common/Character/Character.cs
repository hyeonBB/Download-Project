using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Collider[] m_Colliders;

    private void Awake()
    {
        // �浹 ����
        Physics.IgnoreCollision(m_Colliders[0], m_Colliders[1], true);
    }
}
