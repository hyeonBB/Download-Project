using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
public class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null;
    public static GameManager Instance => m_instance;

    private void Awake()
    {
        if (null == m_instance)
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject); //�� ��ȯ�� �Ǵ��� �ı����� ����
        }
        else
            Destroy(this.gameObject); //�̹� ���������� instance�� �ν��Ͻ��� �����Ѵٸ� �ڽ��� ����
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void Save_JsonData<T>(string filePath, List<T> saveData)
    {
        var Result = JsonConvert.SerializeObject(saveData);
        File.WriteAllText(filePath, Result);
    }

    public List<T> Load_JsonData<T>(string filePath)
    {
        string Result = File.ReadAllText(filePath);        // JSON ���� �б�
        return JsonConvert.DeserializeObject<List<T>>(Result); // JSON ���ڿ��� ���ʸ� Ÿ�� �迭�� ������ȭ
    }
}