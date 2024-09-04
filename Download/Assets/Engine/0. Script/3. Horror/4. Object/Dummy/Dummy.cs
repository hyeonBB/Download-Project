using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    [SerializeField] private Jumpscare m_soundJumpscare;
    private AudioSource[] m_audioSources;

    private void Start()
    {
        m_audioSources = GetComponents<AudioSource>();
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
        StartCoroutine(Play_Sound());
    }

    private IEnumerator Play_Sound()
    {
        GameManager.Ins.Sound.Play_AudioSource(ref m_audioSources[0], "Horror_Dummy1", false, 1f);

        float time = 0;
        while(true)
        {
            time += Time.deltaTime;
            if (time >= 1f)
                break;
            yield return null;
        }

        GameManager.Ins.Sound.Play_AudioSource(ref m_audioSources[1], "Horror_Dummy2", false, 1f);
        StartCoroutine(Check_Triger());
        yield break;
    }

    private IEnumerator Check_Triger()
    {
        while(true)
        {
            if (m_soundJumpscare == null || m_soundJumpscare.IsTriger == true)
                break;

            yield return null;
        }

        transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Animator>().enabled = true;
        transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        yield break;
    }
}
