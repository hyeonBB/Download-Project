using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualNovelManager : MonoBehaviour
{
    private static VisualNovelManager m_instance = null;
    public static VisualNovelManager Instance
    {
        get //���� �Ŵ��� �ν��Ͻ��� ������ �� �ִ� ������Ƽ. static�̹Ƿ� �ٸ� Ŭ�������� ȣ�� ����
        {
            if (null == m_instance)
                return null;
            return m_instance;
        }
    }

    [Header("Likeability")]
    [SerializeField] private GameObject m_likeability;
    [SerializeField] private DialogHeart[] m_dialogHeart;

    public int[] npcHeart;
    public int[] NpcHeart
    {
        get { return npcHeart; }
        set { npcHeart = value; }
    }

    // TestCode
    [Header("Test")]
    [SerializeField] private string m_dialogDataPath;

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;
    }

    private void Start()
    {
        npcHeart = new int[(int)OWNER_TYPE.OT_END];
        for(int i = 0; i < (int)OWNER_TYPE.OT_END; i++)
            npcHeart[i] = 5;

        npcHeart[0] = 1;
        npcHeart[1] = 4;
        npcHeart[2] = 6;
    }

    private void Update()
    {
        Update_Input();
    }

    private void Update_Input()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            Active_Popup();

        // TestCode
        if (Input.GetKeyDown(KeyCode.A))
            DialogManager.Instance.Create_Dialog(m_dialogDataPath);
    }

    public void Active_Popup()
    {
        // ȣ����â ��/Ȱ��ȭ
        m_likeability.SetActive(!m_likeability.activeSelf);
        if (true == m_likeability.activeSelf)
        {
            // ȣ����â NPC���� ������Ʈ
            for (int i = 0; i < m_dialogHeart.Length; i++)
                m_dialogHeart[i].Update_Heart();
        }
    }
}
