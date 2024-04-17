using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public enum LEVELSTATE { LS_NOVEL, LS_SHOOT, LS_CHASE, LS_END };

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

    [SerializeField] private LEVELSTATE m_LevelState;

#region LS_NOVEL
    [Header("[ LS_NOVEL ]")]
    [SerializeField] private GameObject m_likeability;
    [SerializeField] private DialogHeart[] m_dialogHeart;

    private int[] m_npcHeart;
    public int[] NpcHeart
    {
        get { return m_npcHeart; }
        set { m_npcHeart = value; }
    }
#endregion

#region LS_SHOOT
    [Header("[ LS_SHOOT ]")]
    [SerializeField] private GameObject m_shootGame;
    [SerializeField] private TMP_Text m_countTxt;
    private bool m_shootGameStart = false;
    public bool ShootGameStart
    {
        get { return m_shootGameStart; }
        set { m_shootGameStart = value; }
    }
    private float m_time    = 0f;
    private float m_maxTime = 30f;
#endregion

#region LS_CHASE
    [Header("[ LS_CHASE ]")]
    [SerializeField] private GameObject m_chaseGame;
    [SerializeField] private GameObject m_Cd;
    [SerializeField] private TMP_Text m_CdTextCount;
    [SerializeField] private int m_CdMaxCount = 5;
    [SerializeField] private int m_CdCurrentCount = 0;
    [SerializeField] private float m_CdMinDistance = 20.0f;
    [SerializeField] private float m_CdMaxDistance = 200.0f;

    [SerializeField] private GameObject m_Lever;
    [SerializeField] private int m_LeverMaxCount = 2;
    [SerializeField] private Transform[] m_RandomPos;

    [SerializeField] private List<HallwayLight> m_Light; // 464
    public List<HallwayLight> Light
    {
        get { return m_Light; }
        set { m_Light = value; }
    }

    private List<GameObject> m_Levers = new List<GameObject>();
    private Transform m_playerTr;
    private GameObject m_boss;
#endregion

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;
    }

    private void Start()
    {
        // Temp
        Change_Level(LEVELSTATE.LS_SHOOT);
    }

    private void Update()
    {
        Update_Level(m_LevelState);
    }

    public void Change_Level(LEVELSTATE level)
    {
        Finish_Level(m_LevelState);

        m_LevelState = level;

        Start_Level(m_LevelState);
    }

    private void Start_Level(LEVELSTATE level)
    {
        switch (level)
        {
            case LEVELSTATE.LS_NOVEL:
                Start_NovelGame();
                break;

            case LEVELSTATE.LS_SHOOT:
                Start_ShootGame();
                break;

            case LEVELSTATE.LS_CHASE:
                Start_ChaseGame();
                break;
        }
    }

    private void Update_Level(LEVELSTATE level)
    {
        switch (level)
        {
            case LEVELSTATE.LS_NOVEL:
                Update_NovelGame();
                break;

            case LEVELSTATE.LS_SHOOT:
                Update_ShootGame();
                break;

            case LEVELSTATE.LS_CHASE:
                Update_ChaseGame();
                break;
        }
    }

    private void Finish_Level(LEVELSTATE level)
    {
        switch (level)
        {
            case LEVELSTATE.LS_NOVEL:
                Finish_NovelGame();
                break;

            case LEVELSTATE.LS_SHOOT:
                Finish_ShootGame();
                break;

            case LEVELSTATE.LS_CHASE:
                Finish_ChaseGame();
                break;
        }
    }

#region LS_NOVEL
    private void Start_NovelGame()
    {
        m_npcHeart = new int[(int)OWNER_TYPE.OT_END];
        for (int i = 0; i < (int)OWNER_TYPE.OT_END; i++)
            m_npcHeart[i] = 7;
    }

    private void Update_NovelGame()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            Active_Popup();
    }

    private void Finish_NovelGame()
    {
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
#endregion

#region LS_SHOOT
    private void Start_ShootGame()
    {
        m_chaseGame.SetActive(false);
        m_shootGame.SetActive(true);

        m_time = m_maxTime;
    }

    private void Update_ShootGame()
    {
        if (!m_shootGameStart)
            return;

        // ���� ����
        Update_Count();
    }

    private void Finish_ShootGame()
    {
        // Ŀ�� �̹��� �ʱ�ȭ
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        Destroy(m_shootGame);
    }

    private void Update_Count()
    {
        m_time -= Time.deltaTime;
        if(m_time <= 0f)
        {
            int Count = 0;
            m_countTxt.text = Count.ToString();

            Finish_ShootGame();
        }
        else
        {
            int Count = (int)m_time;
            m_countTxt.text = Count.ToString();
        }
    }
#endregion

#region LS_CHASE
    private void Start_ChaseGame()
    {
        m_shootGame.SetActive(false);
        m_chaseGame.SetActive(true);

        m_playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        m_boss = GameObject.FindWithTag("Boss");

        Create_CD();
        Create_Lever(m_LeverMaxCount);

        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_FOLLOW);
    }

    private void Update_ChaseGame()
    {
    }

    private void Finish_ChaseGame()
    {
        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_END);
    }

    private void Create_CD()
    {
        List<Vector3> beforePosition = new List<Vector3>();
        beforePosition.Add(new Vector3(0f, 0f, 0f));

        for (int i = 0; i < m_CdMaxCount; i++)
        {
            Vector3 newPosition = Get_RandomPositionOnNavMesh(beforePosition);
            Instantiate(m_Cd, newPosition, Quaternion.identity);
            beforePosition.Add(newPosition);
        }
    }

    private void Create_Lever(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            Vector3 NewPosition = Vector3.zero;
            while (true)
            {
                NewPosition = m_RandomPos[Random.Range(0, 20)].position;

                bool Same = false;
                for (int j = 0; j < m_Levers.Count; j++)
                {
                    if (NewPosition == m_Levers[j].transform.position)
                        Same = true;
                }

                if (!Same)
                    break;
            }

            GameObject level = Instantiate(m_Lever, NewPosition, Quaternion.identity);
            m_Levers.Add(level);
        }
    }

    public void Get_CD()
    {
        m_CdCurrentCount++;
        if (m_CdCurrentCount >= m_CdMaxCount)
        {
            // �߰� ���� ����
            Finish_ChaseGame();
        }
        else
        {
            // UI ������Ʈ
            m_CdTextCount.text = m_CdCurrentCount.ToString();

            // ���� ������Ʈ Max 464
            m_Light.Shuffle();
            int OnCount = (int)(464 / (m_CdMaxCount - 1)) * m_CdCurrentCount;
            for (int i = 0; i < OnCount; ++i)
                m_Light[i].Blink = true;

            // ��� ���
        }

    }

    public void Use_Lever(GameObject self)
    {
        // ������ ȿ�� ����
        m_boss.GetComponent<HallwayYandere>().Used_Lever();

        // ���� ������ ����
        for (int i = 0; i < m_Levers.Count; i++)
        {
            if (self == m_Levers[i])
            {
                m_Levers.RemoveAt(i);
                Destroy(self);
                break;
            }
        }

        // �߰� ����
        Create_Lever(1);
    }

    private Vector3 Get_RandomPositionOnNavMesh(List<Vector3> beforePos)
    {
        Vector3 position = new Vector3();
        bool select = false;

        int loopNum = 0;
        while (!select)
        {
            Vector3 randomPos = m_playerTr.position + Random.insideUnitSphere * m_CdMaxDistance; // ���ϴ� ���� ���� ���� ���� ���� ����
            randomPos.y = 0.0f;
            NavMeshHit hit;

            // SamplePosition((Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask)
            // areaMask �� �ش��ϴ� NavMesh �߿��� maxDistance �ݰ� ������ sourcePosition�� ���� ����� ��ġ�� ã�Ƽ� �� ����� hit�� ����
            if (NavMesh.SamplePosition(randomPos, out hit, m_CdMaxDistance, NavMesh.AllAreas)) // ��ġ ���ø��� �����ϸ� ���� ��ȯ
            {
                bool distMin = false;
                foreach (Vector3 pos in beforePos)
                {
                    float distX = Mathf.Abs(hit.position.x - pos.x);
                    float distZ = Mathf.Abs(hit.position.z - pos.z);
                    if (distX <= m_CdMinDistance || distZ <= m_CdMinDistance)
                        distMin = true;
                }

                if (!distMin)
                {
                    position = hit.position;
                    select = true;
                }
            }

            if (loopNum++ > 10000)
                throw new System.Exception("Infinite Loop");
        }

        return position;
    }
#endregion
}