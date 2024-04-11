using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class VisualNovelManager : MonoBehaviour
{
    enum LEVELSTATE { LS_NOVEL, LS_SHOOT, LS_CHASE, LS_FINISH, LS_END };

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

    [Header("[ Basic ]")]
    [SerializeField] private LEVELSTATE m_LevelState;

    [Header("[ Likeability ]")]
    [SerializeField] private GameObject m_likeability;
    [SerializeField] private DialogHeart[] m_dialogHeart;

    [Header("[ CD ]")]
    [SerializeField] private GameObject m_CD;
    [SerializeField] private TMP_Text m_TextCDCount;
    [SerializeField] private float m_minDistance = 20.0f;
    [SerializeField] private float m_maxDistance = 200.0f;
    [SerializeField] private int m_MaxCount = 5;
    [SerializeField] private int m_CurrentCount = 0;

    private Transform m_playerTr;

    private int[] m_npcHeart;
    public int[] NpcHeart
    {
        get { return m_npcHeart; }
        set { m_npcHeart = value; }
    }

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;
    }

    private void Start()
    {
        m_playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();

        m_npcHeart = new int[(int)OWNER_TYPE.OT_END];
        for(int i = 0; i < (int)OWNER_TYPE.OT_END; i++)
            m_npcHeart[i] = 5;

        Create_CD();
    }

    private void Update()
    {
        Update_Input();
    }

    private void Update_Input()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            Active_Popup();
    }

    private void Active_Popup()
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

    private void Create_CD() // ���Ŀ� �������¿� ���� [����, ����, Ż��] �Լ� �и��ؼ� ���
    {
        List<Vector3> beforePosition = new List<Vector3>();
        beforePosition.Add(new Vector3(0f, 0f, 0f));

        for (int i = 0; i < m_MaxCount; i++)
        {
            Vector3 newPosition = Get_RandomPositionOnNavMesh(beforePosition);
            Instantiate(m_CD, newPosition, Quaternion.identity);
            beforePosition.Add(newPosition);
        }
    }

    private Vector3 Get_RandomPositionOnNavMesh(List<Vector3> beforePos)
    {
        Vector3 position = new Vector3();
        bool select = false;

        int loopNum = 0;
        while (!select)
        {
            Vector3 randomPos = m_playerTr.position + Random.insideUnitSphere * m_maxDistance; // ���ϴ� ���� ���� ���� ���� ���� ����
            randomPos.y = 0.0f;
            NavMeshHit hit;

            // SamplePosition((Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask)
            // areaMask �� �ش��ϴ� NavMesh �߿��� maxDistance �ݰ� ������ sourcePosition�� ���� ����� ��ġ�� ã�Ƽ� �� ����� hit�� ����
            if (NavMesh.SamplePosition(randomPos, out hit, m_maxDistance, NavMesh.AllAreas)) // ��ġ ���ø��� �����ϸ� ���� ��ȯ
            {
                bool distMin = false;
                foreach (Vector3 pos in beforePos)
                {
                    float distX = Mathf.Abs(hit.position.x - pos.x);
                    float distZ = Mathf.Abs(hit.position.z - pos.z);
                    if (distX <= m_minDistance || distZ <= m_minDistance)
                        distMin = true;
                }

                if (!distMin)
                {
                    position = hit.position;
                    select   = true;
                }
            }

            if (loopNum++ > 10000)
                throw new System.Exception("Infinite Loop");
        }

        return position;
    }

    public void Add_CD()
    {
        m_CurrentCount++;
        if (m_CurrentCount >= m_MaxCount)
        {
            // �߰� ���� ����
        }
        else
            m_TextCDCount.text = m_CurrentCount.ToString();
    }
}
