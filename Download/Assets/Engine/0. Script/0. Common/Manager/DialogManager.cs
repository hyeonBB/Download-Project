using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;


public class DialogManager : MonoBehaviour
{
    private static DialogManager m_instance = null;
    public static DialogManager Instance
    {
        get //���� �Ŵ��� �ν��Ͻ��� ������ �� �ִ� ������Ƽ. static�̹Ƿ� �ٸ� Ŭ�������� ȣ�� ����
        {
            if (null == m_instance)
                return null;
            return m_instance;
        }
    }

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;
    }

    private void Start()
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