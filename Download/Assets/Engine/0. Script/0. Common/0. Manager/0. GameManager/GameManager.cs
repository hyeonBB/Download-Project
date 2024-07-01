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
        TextAsset jsonAsset = Resources.Load<TextAsset>(filePath);

        if (jsonAsset != null)
            return JsonConvert.DeserializeObject<List<T>>(jsonAsset.text);
        else
            Debug.LogError($"Failed to load JSON data : {filePath}");

        return null;
    }

    public GameObject Create_GameObject(string path, Transform transform)
    {
        return Instantiate(Resources.Load<GameObject>(path), transform);
    }
}