using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
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

    public void Save_Data(string filePath, DialogData[] saveData)
    {
        var Result = JsonConvert.SerializeObject(saveData);
        File.WriteAllText(filePath, Result);
    }

    public DialogData[] Load_Data(string filePath)
    {
        string Result = File.ReadAllText(filePath); // JSON ���� �б�
        return JsonConvert.DeserializeObject<DialogData[]>(Result); // JSON ���ڿ��� DialogData �迭�� ������ȭ
    }
}
