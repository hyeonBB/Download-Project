using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    private AudioSource m_audioSource;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void Fall_Dummy()
    {
        // ��Ȳ�� �ڽ�ǥ�ÿ� �׿��ִ� ���ǵ��� �������� ������ �� �ְ� �ȴ�.
        // [��] �����ϱ� > ��ũ��Ʈ(: �׿��ִ� ���ǵ��� ��������... ��� �� ���� ?)
        transform.GetChild(1).gameObject.SetActive(true);
        Destroy(transform.GetChild(0).gameObject);

        // ���� �ִϸ��̼� ��� : �ܿ� ������ ���� ������.
        //Animator animator = transform.GetChild(1).GetComponent<Animator>();
        //if (animator != null)
        //    animator.SetBool("", true);

        // ���� �μ����� �Ҹ� �� ���ڱ� �Ҹ��� �鸰��.
    }
}
