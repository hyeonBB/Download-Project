using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Folder : Panel_Popup
{
    // ���ã�� ��ư
    [SerializeField] private GameObject[] m_bookmarkPopups;
    private bool m_bookmarkActive = false;
    //*

    // �ּ� �Է�


    // ���� ����


    private void Start()
    {
        
    }

    private void Update()
    {
        
    }


    public void Button_Bookmark() // ���ã�� ��ư
    {
        for(int i = 0; i < m_bookmarkPopups.Length; ++i)
        {
            m_bookmarkPopups[i].SetActive(!m_bookmarkActive);
        }
        m_bookmarkActive = !m_bookmarkActive;
    }

    public void Button_InputFieldDown()
    {

    }
}
