using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Vector3 m_targetPosition;

    private bool m_active = true;

    private GameObject m_player;

    private void Start()
    {
        m_player = GameObject.FindWithTag("Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_active == false || collision.gameObject.CompareTag("Player") == false || m_player == null)
            return;

        // ���̵� �ƿ� -> �̵� -> ��
        GameManager.Instance.UI.Start_FadeOut(1f, Color.black, () => Teleport_Player(), 1f, false);

    }

    private void Teleport_Player()
    {
        m_player.transform.position = m_targetPosition;
        GameManager.Instance.UI.Start_FadeIn(1f, Color.black);
    }
}
