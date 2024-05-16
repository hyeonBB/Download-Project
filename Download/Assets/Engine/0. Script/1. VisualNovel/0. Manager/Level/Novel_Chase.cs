using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace VisualNovel
{
    public class Novel_Chase : Level
    {
        private GameObject m_yandereObj;
        private List<HallwayLight> m_Light = new List<HallwayLight>(); // 464
        private List<GameObject> m_Levers = new List<GameObject>();

        private HallwayYandere m_yandere;
        private HallwayPlayer m_player;
        private Transform m_yandereTr;
        private Transform m_playerTr;

        private int m_CdMaxCount = 5;
        private int m_CdCurrentCount = 0;
        private int m_LeverMaxCount = 2;
        private float m_CdMinDistance = 20.0f;
        private float m_CdMaxDistance = 200.0f;

        public HallwayPlayer Player { get => m_player; }
        public Transform PlayerTr { get => m_playerTr; }
        public List<HallwayLight> Light
        {
            get => m_Light;
            set => m_Light = value;
        }

        public Novel_Chase(LevelController levelController) : base(levelController)
        {
        }

        public override void Enter_Level()
        {
            m_player = VisualNovelManager.Instance.PlayerObj.GetComponent<HallwayPlayer>();
            m_playerTr = VisualNovelManager.Instance.PlayerObj.GetComponent<Transform>();

            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_BASIC_3D);
            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_CUTSCENE);
            CameraCutscene camera = (CameraCutscene)CameraManager.Instance.Get_CurCamera();
            camera.Change_Position(new Vector3(0f, 1.4f, -2.8f));
            camera.Change_Rotation(new Vector3(8.5f, 0f, 0f));

            // ���Ͻ� ���̾�α� ���� (���̵� ��)
            Dialog_VN dialog = VisualNovelManager.Instance.Dialog.GetComponent<Dialog_VN>();
            dialog.Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_VN>("Assets/Resources/4. Data/1. VisualNovel/Dialog/Dialog5_Cellar.json"));
            dialog.Close_Background();

            VisualNovelManager.Instance.ChaseGame.SetActive(true);
        }

        public override void Play_Level()
        {
            VisualNovelManager.Instance.Dialog.SetActive(false);

            Create_CD();
            Create_Lever(m_LeverMaxCount);
            m_player.Set_Lock(false);

            UIManager.Instance.Start_FadeIn(1f, Color.black);
            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_FOLLOW);
        }

        public override void Update_Level()
        {
        }

        public override void Exit_Level()
        {
            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_END);
        }

        public override void OnDrawGizmos()
        {
        }

        private void Clear_ChaseGame()
        {
            // ���� Ŭ���� : CD 5�� �� ���� �� �ƾ� ���� �� ��ȯ(���� �� ���η� ��ȯ)
        }

        private void Fail_ChaseGame()
        {
            // ���� ���� : �ᵥ������ ���� �� �ƾ� ���� �� ���� ���ۺ��� �ٽ� ����(�絵�� UI ���)
        }

        public void Create_Monster()
        {
            // ĳ���� ��
            m_player.Set_Lock(true);

            // ī�޶� ��ü �� ����
            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_CUTSCENE);
            CameraCutscene camera = (CameraCutscene)CameraManager.Instance.Get_CurCamera();
            camera.Change_Position(new Vector3(0f, 1.2f, 20f));
            camera.Change_Rotation(new Vector3(0f, -180f, 25f));

            // �ᵥ�� ����
            m_yandereObj = Instantiate(Resources.Load<GameObject>("5. Prefab/1. VisualNovel/Character/Yandere"));
            m_yandere = m_yandereObj.GetComponent<HallwayYandere>();
            m_yandereTr = m_yandereObj.GetComponent<Transform>();
            m_yandereTr.position = new Vector3(0f, 0f, 2.8f);

            // ���̵� ��
            UIManager.Instance.Start_FadeIn(1f, Color.black);
            // ���鼭 Ư���Ÿ����� ����
            camera.Start_Cutscene(new Vector3(0f, 1.2f, 5.5f), new Vector3(0f, 180f, -16f), 2f, 0.5f);
            // ĳ���Ͱ� ���� �Ҷ� �ᵥ�� �� Ŭ�����
            // 
        }

        private void Create_CD()
        {
            List<Vector3> beforePosition = new List<Vector3>();
            beforePosition.Add(new Vector3(0f, 0f, 0f));

            for (int i = 0; i < m_CdMaxCount; i++)
            {
                Vector3 newPosition = Get_RandomPositionOnNavMesh(beforePosition);
                Instantiate(Resources.Load<GameObject>("5. Prefab/1. VisualNovel/Object/CD"), newPosition, Quaternion.identity);
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
                    NewPosition = VisualNovelManager.Instance.RandomPos[Random.Range(0, 20)].position;

                    bool Same = false;
                    for (int j = 0; j < m_Levers.Count; j++)
                    {
                        if (NewPosition == m_Levers[j].transform.position)
                            Same = true;
                    }

                    if (!Same)
                        break;
                }

                GameObject level = Instantiate(Resources.Load<GameObject>("5. Prefab/1. VisualNovel/Object/Lever"), NewPosition, Quaternion.identity);
                m_Levers.Add(level);
            }
        }

        public void Get_CD()
        {
            m_CdCurrentCount++;
            if (m_CdCurrentCount >= m_CdMaxCount)
            {
                // �߰� ���� ����
                Exit_Level();
            }
            else
            {
                // UI ������Ʈ
                VisualNovelManager.Instance.CdTxt.text = m_CdCurrentCount.ToString();

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
            if (m_yandereObj != null)
                m_yandere.Used_Lever();

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

                if (loopNum++ > 10000) // ���� ��ġ�� ������ �� ����ϱ�
                    throw new System.Exception("Infinite Loop");
            }

            return position;
        }
    }
}

