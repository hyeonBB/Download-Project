using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private AudioSource m_audioSource;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void Click_Gun()
    {
        m_audioSource.Stop();
        m_audioSource.clip = GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/Effect/UI/���� ������ ��");
        m_audioSource.Play();
    }

    public void Shoot_Gun()
    {
        m_audioSource.Stop();
        m_audioSource.clip = GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/Effect/UI/�ѼҸ�");
        m_audioSource.Play();
    }
}
