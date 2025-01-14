using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PositionData
{
    public List<Vector3> positions;
}

public class SerializingPosition : MonoBehaviour
{
    private string     m_path = "4. Data/1. VisualNovel/Position/ItemPositionData";
    private GameObject m_prefab;

    private void Start()
    {
        m_prefab = GameManager.Ins.Resource.Load<GameObject>("5. Prefab/0. Common/PositionTemp");
    }

    private void Update()
    {
#if UNITY_EDITOR
        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    SavePositions();
        //}
        //if (Input.GetKeyDown(KeyCode.F2))
        //{
        //    LoadPositions();
        //}
#endif
    }

    private void SavePositions() // ItemPositionData.json
    {
        PositionData data = new PositionData();
        data.positions = new List<Vector3>();

        foreach (Transform child in transform) { data.positions.Add(child.position); }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(m_path, json);

        Debug.Log("Positions saved to " + m_path);
    }

    private void LoadPositions() // 4. Data/1. VisualNovel/Position/ItemPositionData
    {
        string json = GameManager.Ins.Resource.Load<TextAsset>(m_path).text;
        PositionData data = JsonUtility.FromJson<PositionData>(json);

        foreach (Transform child in transform) { Destroy(child.gameObject); }
        foreach (Vector3 position in data.positions) { GameManager.Ins.Resource.Create(m_prefab, position, Quaternion.identity, transform); }

        Debug.Log("Positions loaded to " + m_path);
    }
}