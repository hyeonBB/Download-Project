using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;

namespace VisualNovel
{
    public class Novel_Chase : Level
    {
        private GameObject m_yandereObj;
        private List<HallwayLight> m_Light = new List<HallwayLight>(); // 464
        private List<bool>   m_positionUse = new List<bool>();
        private PositionData m_positionData;

        private HallwayYandere m_yandere;
        private HallwayPlayer m_player;
        private Transform m_yandereTr;
        private Transform m_playerTr;

        private int m_CdMaxCount = 5;
        private int m_CdCurrentCount = 0;
        private int m_LeverMaxCount = 2;
        GameObject m_itemText = null;
        Coroutine m_ItemTextCoroutine = null;
        //private float m_CdMinDistance = 20.0f;
        //private float m_CdMaxDistance = 200.0f;

        public HallwayPlayer Player { get => m_player; }
        public Transform PlayerTr { get => m_playerTr; }
        public List<HallwayLight> Light
        {
            get => m_Light;
            set => m_Light = value;
        }
        public GameObject ItemText => m_itemText;


        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);

            // ���� ������ �ҷ�����
            m_positionData = JsonUtility.FromJson<PositionData>(File.ReadAllText("Assets/Resources/4. Data/1. VisualNovel/Position/ItemPositionData"));
            for(int i = 0; i < m_positionData.positions.Count; ++i) { m_positionUse.Add(false); }
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
            //CameraManager.Instance.Change_Camera(CAMERATYPE.CT_END);
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
            //List<Vector3> beforePosition = new List<Vector3>();
            //beforePosition.Add(new Vector3(0f, 0f, 0f));

            for (int i = 0; i < m_CdMaxCount - 1; i++) // 1���� �ʻ� ���� ��ġ
            {
                //Vector3 newPosition = Get_RandomPositionOnNavMesh(beforePosition);
                //beforePosition.Add(newPosition);

                int index = -1;
                Vector3 NewPosition = Vector3.zero;
                while (true)
                {
                    index = Random.Range(0, m_positionData.positions.Count);
                    NewPosition = m_positionData.positions[index];
                    if (m_positionUse[index] == false)
                    {
                        m_positionUse[index] = true;
                        break;
                    }
                }

                GameObject CD = Instantiate(Resources.Load<GameObject>("5. Prefab/1. VisualNovel/Object/CD"));
                CD.GetComponent<HallwayCD>().PositionIndex = index;
                CD.transform.position = NewPosition;
            }
        }

        private void Create_Lever(int count, int beforIndex = -1)
        {
            for (int i = 0; i < count; ++i)
            {
                int index = -1;
                Vector3 NewPosition = Vector3.zero;
                while (true)
                {
                    index = Random.Range(0, m_positionData.positions.Count);
                    NewPosition = m_positionData.positions[index];
                    if (index != beforIndex && m_positionUse[index] == false)
                    {
                        m_positionUse[index] = true;
                        break;
                    }
                }

                GameObject level = Instantiate(Resources.Load<GameObject>("5. Prefab/1. VisualNovel/Object/Lever"));
                level.GetComponent<HallwayLever>().PositionIndex = index;
                level.transform.position = NewPosition;
            }
        }

        public void Get_CD(int positionIndex)
        {
            if(positionIndex != -1)
                m_positionUse[positionIndex] = false;

            m_CdCurrentCount++;
            VisualNovelManager.Instance.CdTxt.text = m_CdCurrentCount.ToString(); // UI ������Ʈ

            if (m_CdCurrentCount >= m_CdMaxCount)
            {
                // �߰� ���� ���� �� �ƾ� ���
                Exit_Level();
            }
            else
            {
                // ���� ������Ʈ Max 464
                m_Light.Shuffle();
                int OnCount = (int)(464 / (m_CdMaxCount - 1)) * m_CdCurrentCount;
                for (int i = 0; i < OnCount; ++i)
                    m_Light[i].Blink = true;

                // ��� ���
                if(m_itemText == null)
                {
                    m_itemText = Instantiate(Resources.Load<GameObject>("5. Prefab/1. VisualNovel/UI/UI_ItemText"), GameObject.Find("Chase").transform.GetChild(1));
                    m_itemText.SetActive(false);
                }

                switch(m_CdCurrentCount)
                {
                    case 1:
                        // �Դ� ��� �ٷ� �ӵ� ���� �� �ƾ� ���
                        VisualNovelManager.Instance.PlayerObj.GetComponent<HallwayPlayer>().MoveSpeed = 200f;
                        UIManager.Instance.Start_FadeOut(1f, Color.black, () => VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Chase>().Create_Monster(), 1f, false);
                        break;

                    case 2:
                        if (m_ItemTextCoroutine != null)
                            StopCoroutine(m_ItemTextCoroutine);
                        m_ItemTextCoroutine = StartCoroutine(Wait_Text("�����Լ� �־�����������..."));
                        break;

                    case 3:
                        if (m_ItemTextCoroutine != null)
                            StopCoroutine(m_ItemTextCoroutine);
                        StartCoroutine(Wait_Text("���� �׸� ���ƿͿ�!"));
                        break;

                    case 4:
                        if (m_ItemTextCoroutine != null)
                            StopCoroutine(m_ItemTextCoroutine);
                        StartCoroutine(Wait_Text("�� ���� ������..."));
                        break;
                }
            }

        }

        public void Use_Lever(int positionIndex)
        {
            m_positionUse[positionIndex] = false;

            // ������ ȿ�� ����
            if (m_yandereObj != null)
                m_yandere.Used_Lever();

            // �߰� ����
            Create_Lever(1, positionIndex);
        }

        private IEnumerator Wait_Text(string str)
        {
            float time = 0f;
            while(true)
            {
                time += Time.deltaTime;
                if(time > 1f)
                    break;

                yield return null;
            }

            m_itemText.GetComponent<ItemText>().Start_ItemText(str);
            yield break;
        }

        //private Vector3 Get_RandomPositionOnNavMesh(List<Vector3> beforePos)
        //{
        //    Vector3 position = new Vector3();
        //    bool select = false;

        //    int loopNum = 0;
        //    while (!select)
        //    {
        //        Vector3 randomPos = m_playerTr.position + Random.insideUnitSphere * m_CdMaxDistance; // ���ϴ� ���� ���� ���� ���� ���� ����
        //        randomPos.y = 0.0f;
        //        NavMeshHit hit;

        //        // SamplePosition((Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask)
        //        // areaMask �� �ش��ϴ� NavMesh �߿��� maxDistance �ݰ� ������ sourcePosition�� ���� ����� ��ġ�� ã�Ƽ� �� ����� hit�� ����
        //        if (NavMesh.SamplePosition(randomPos, out hit, m_CdMaxDistance, NavMesh.AllAreas)) // ��ġ ���ø��� �����ϸ� ���� ��ȯ
        //        {
        //            bool distMin = false;
        //            foreach (Vector3 pos in beforePos)
        //            {
        //                float distX = Mathf.Abs(hit.position.x - pos.x);
        //                float distZ = Mathf.Abs(hit.position.z - pos.z);
        //                if (distX <= m_CdMinDistance || distZ <= m_CdMinDistance)
        //                    distMin = true;
        //            }

        //            if (!distMin)
        //            {
        //                position = hit.position;
        //                select = true;
        //            }
        //        }

        //        if (loopNum++ > 10000) // ���� ��ġ�� ������ �� ����ϱ�
        //            throw new System.Exception("Infinite Loop");
        //    }

        //    return position;
        //}
    }
}

