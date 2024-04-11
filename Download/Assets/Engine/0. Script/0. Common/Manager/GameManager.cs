using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null;
    public static GameManager Instance
    {
        get //���� �Ŵ��� �ν��Ͻ��� ������ �� �ִ� ������Ƽ. static�̹Ƿ� �ٸ� Ŭ�������� ȣ�� ����
        {
            if (null == m_instance)
                return null;
            return m_instance;
        }
    }


    private string m_playerName = null;
    public string PlayerName
    {
        get 
        { 
            return m_playerName;
        }
        set
        {
            if(value.Length > 0)
                m_playerName = value;
        }
    }

    private void Awake()
    {
        if (null == m_instance)
        {
            m_instance = this;
            //�� ��ȯ�� �Ǵ��� �ı����� ����
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //�̹� ���������� instance�� �ν��Ͻ��� �����Ѵٸ� �ڽ��� ����
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}
