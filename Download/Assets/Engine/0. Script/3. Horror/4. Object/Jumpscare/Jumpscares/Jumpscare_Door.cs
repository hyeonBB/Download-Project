using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpscare_Door : Jumpscare
{
    [SerializeField] private Interaction_Door m_door;

    public override void Active_Jumpscare()
    {
        m_isTrigger = true;

        // �ⱸ ���� �� ����
        m_door.Move_Door(false);

        // ���ϴ� ���� ���
        AudioSource audioSource = m_door.GetComponent<AudioSource>();
        if (audioSource == null)
            return;
        //audioSource.clip;
        //audioSource.loop = false;
        //audioSource.Play();
    }
}
