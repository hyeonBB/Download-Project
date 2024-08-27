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
        // 주황색 박스표시에 쌓여있던 물건들이 무너져서 지나갈 수 있게 된다.
        // [◑] 조사하기 > 스크립트(: 쌓여있던 물건들이 무너졌다... 어떻게 된 거지 ?)
        transform.GetChild(1).gameObject.SetActive(true);
        Destroy(transform.GetChild(0).gameObject);

        // 더미 애니메이션 재생 : 잔여 물건이 툭툭 떨어짐.
        //Animator animator = transform.GetChild(1).GetComponent<Animator>();
        //if (animator != null)
        //    animator.SetBool("", true);

        // 무언가 부서지는 소리 및 발자국 소리가 들린다.
    }
}
