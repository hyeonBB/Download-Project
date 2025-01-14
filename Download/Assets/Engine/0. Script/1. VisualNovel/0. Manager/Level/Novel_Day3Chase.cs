using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using TMPro;
using UnityEngine.UI;

namespace VisualNovel
{
    public class Novel_Day3Chase : Novel_Level
    {
        private Dialogs_Day3Chase m_dialogAsset;

        private GameObject m_stage;
        private Image m_key;

        private GameObject m_playerBodyObj;
        private GameObject m_yandereObj;
        private HallwayYandere m_yandere;
        private HallwayPlayer  m_player;

        private List<HallwayLight> m_Light = new List<HallwayLight>(); // 464
        private List<bool>   m_positionUse = new List<bool>();
        private PositionData m_positionData;

        private int m_CdMaxCount = 5;
        private int m_CdCurrentCount = 0;
        private int m_LeverMaxCount = 2;
        private List<GameObject> m_cds = new List<GameObject>();
        private List<GameObject> m_levers = new List<GameObject>();

        private GameObject m_itemText = null;
        private Coroutine m_ItemTextCoroutine = null;

        public GameObject Stage { get => m_stage; }
        public HallwayPlayer Player { get => m_player; }
        public HallwayYandere Yandere { get => m_yandere; }
        public Animator YandereAnimator { get => m_yandereObj.GetComponentInChildren<Animator>(); }
        public List<HallwayLight> Light { get => m_Light; set => m_Light = value; }
        public GameObject ItemText => m_itemText;

        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);

            // 랜덤 포지션 불러오기
            m_positionData = JsonUtility.FromJson<PositionData>(GameManager.Ins.Resource.Load<TextAsset>("4. Data/1. VisualNovel/Position/ItemPositionData").text);
            for(int i = 0; i < m_positionData.positions.Count; ++i) 
                m_positionUse.Add(false);
        }

        public override void Enter_Level()
        {
            if(GameManager.Ins.Novel.LevelController.Prelevel != (int)VisualNovelManager.LEVELSTATE.LS_DAY3CHASEGAME)
            {
                m_stage = GameManager.Ins.Resource.LoadCreate("5. Prefab/1. VisualNovel/Map/Chase");
                m_player = m_stage.transform.GetChild(2).GetChild(2).GetComponent<HallwayPlayer>();
                m_key = m_stage.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>();

                // 플레이어 바디 생성
                m_playerBodyObj = GameManager.Ins.Resource.LoadCreate("1. Graphic/3D/1. VisualNovel/Character/Mesh/Player/Mesh_VisualNovel_Player_Chair");
                m_playerBodyObj.transform.position = new Vector3(0f, 0f, -1.673f);
                m_playerBodyObj.transform.localScale = new Vector3(1.1273f, 1.1273f, 1.1273f);

                // 얀데레 생성
                m_yandereObj = GameManager.Ins.Resource.LoadCreate("5. Prefab/1. VisualNovel/Character/Yandere");
                m_yandereObj.transform.position = new Vector3(0f, 0f, -0.7f);
                m_yandereObj.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                m_yandere = m_yandereObj.GetComponent<HallwayYandere>();

                // 컷씬 카메라 설정
                GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_BASIC_3D);
                GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_CUTSCENE);
                CameraCutscene camera = (CameraCutscene)GameManager.Ins.Camera.Get_CurCamera();
                camera.Change_Position(new Vector3(0f, 1.33f, -1.73f));
                camera.Change_Rotation(new Vector3(63.23f, 0f, 0f));

                // 지하실 다이얼로그 시작
                m_dialogAsset = GameManager.Ins.Resource.Load<ScriptableObject>("4. Data/1. VisualNovel/Dialogs/Dialogs_Day3Chase") as Dialogs_Day3Chase;

                Dialog_VN dialog = GameManager.Ins.Novel.Dialog.GetComponent<Dialog_VN>();
                dialog.Reset_Skip();
                dialog.Start_Dialog(0);
            }
            else
            {
                GameManager.Ins.Set_Pause(false);
                Play_Level();
            }

            // 지하실 BGM
            GameManager.Ins.Sound.Play_AudioSourceBGM("VisualNovel_CellarBGM", true, 1f);
        }

        public override void Play_Level() // 다이얼로그 끝났을 때 또는 다시하기할 때 호출
        {
            // 초기화
            m_player.transform.position = new Vector3(0f, 0f, 0f);
            m_player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            Camera.main.fieldOfView = 60f;

            m_stage.transform.GetChild(0).gameObject.SetActive(false);             // 미니맵 카메라
            m_stage.transform.GetChild(1).GetChild(0).gameObject.SetActive(false); // 미니맵 UI
            GameObject CD = GameManager.Ins.Resource.LoadCreate("5. Prefab/1. VisualNovel/Object/CD");
            CD.transform.position = new Vector3(0.11f, 0f, 18.8f);

            m_CdCurrentCount = 0;

            m_key.fillAmount = 0f;
            if (m_itemText != null)
                m_itemText.SetActive(false);

            // 조명
            for (int i = 0; i < m_Light.Count; ++i)
            {
                m_Light[i].Blink = false;
                m_Light[i].gameObject.GetComponent<Light>().enabled = true;
            }
            // 문 앞 조명 비활성화
            m_stage.transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<Light>().enabled = false;
            //

            GameManager.Ins.Novel.Dialog.SetActive(false);
            if(m_playerBodyObj != null)
                GameManager.Ins.Resource.Destroy(m_playerBodyObj);
            m_yandereObj.SetActive(false);

            for (int i = 0; i < m_positionData.positions.Count; ++i)
                m_positionUse[i] = false;
            Create_CD();
            Create_Lever(m_LeverMaxCount);

            // 카메라 설정
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_FOLLOW);
            CameraFollow camera = (CameraFollow)GameManager.Ins.Camera.Get_CurCamera();
            camera.Set_FollowInfo(m_player.transform, m_player.transform, true, true, new Vector3(0.0f, 1.3f, 0.0f), 80.0f, 100.0f, new Vector2(-20f, 20f), true, true, true);
            camera.IsRock = true;

            // 방법창 생성
            GameManager.Ins.Camera.Set_CursorLock(false);
            GameObject methodObj = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Common/Panel_Method", m_stage.transform.GetChild(1));
            if (methodObj == null) return;
            methodObj.GetComponent<Image>().sprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/Method/Method_Chase");
            methodObj.GetComponent<MethodWindow>().DeleteAction = Move_Player;

            GameManager.Ins.UI.Start_FadeIn(1f, Color.black);
        }

        private void Move_Player()
        {
            m_player.Set_Lock(false);
            GameManager.Ins.Camera.Set_CursorLock(true);
            CameraFollow camera = (CameraFollow)GameManager.Ins.Camera.Get_CurCamera();
            camera.IsRock = false;
        }

        public override void Update_Level()
        {
        }

        public override void Exit_Level()
        {
            m_dialogAsset = null;
        }

        public override void OnDrawGizmos()
        {
        }

        private void Clear_ChaseGame() // 비상구에 충돌 시 호출
        {
            // 게임 클리어 : CD 5개 다 모을 시 컷씬 진행 후 전환
            GameManager.Ins.StartCoroutine(Clear_Game());
        }

        private IEnumerator Clear_Game()
        {
            ////* 임시
            GameManager.Ins.Resource.LoadCreate("5. Prefab/1. VisualNovel/UI/Panel_Clear", GameObject.Find("Canvas").transform);
            float time = 0f;
            while (time < 1f)
            {
                time += Time.deltaTime;
                yield return null;
            }

            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_END);
            GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.Change_Scene(StageManager.STAGE.LEVEL_WINDOW), 1f, false);
            yield break;
        }

        public void Fail_ChaseGame()
        {
            // 얀데레 메시 비활성화
            m_yandereObj.transform.GetChild(0).gameObject.SetActive(false);

            // 게임 오버 이벤트 발생
            GameManager.Ins.StartCoroutine(Fail_GameEvent());
        }

        private IEnumerator Fail_GameEvent()
        {
            Animator handAnimator = null;
            CameraCutscene camera = null;
            GameObject redPanel = null;

            //float time = 0f;
            //bool fadeOut = false;

            // 게임 실패 : 얀데레한테 잡힐 시 컷씬 진행 후 복도 시작부터 다시 시작 (재도전 UI 출력)
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_CUTSCENE);
            camera = (CameraCutscene)GameManager.Ins.Camera.Get_CurCamera();
            camera.Start_FOV(10f, 20f);

            // 손 생성
            GameObject handObject = GameManager.Ins.Resource.LoadCreate("5. Prefab/1. VisualNovel/Character/YandereHand", Camera.main.transform);
            handObject.transform.localPosition = new Vector3(0f, -0.01f, 9.05f);
            handAnimator = handObject.transform.GetChild(0).GetComponent<Animator>();

            while (true)//fadeOut == false)
            {
                if(GameManager.Ins.IsGame == false)
                    yield return null;

                //if (redPanel == null)
                //{
                    if (camera != null && Camera.main.fieldOfView <= 25f && handAnimator != null && handAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !handAnimator.IsInTransition(0))
                    {
                        m_yandere.GetComponent<AudioSource>().enabled = false;
                        redPanel = GameManager.Ins.Resource.LoadCreate("5. Prefab/1. VisualNovel/UI/Panel_Fail", GameObject.Find("Canvas").transform);
                        redPanel.GetComponent<PanelFail>().Start_PanelFail(handObject);
                        break;
                    }
                //}
                //else
                //{
                    //time += Time.deltaTime;
                    //if (fadeOut == false && time > 1f)
                    //{
                    //    fadeOut = true;
                    //    GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.Change_Scene(StageManager.STAGE.LEVEL_WINDOW), 1f, false);
                    //}
                //}
                yield return null;
            }

            yield break;
        }

        public void Appear_Monster()
        {
            // 등장 컷씬 사운드
            GameManager.Ins.Sound.Play_AudioSourceBGM("VisualNovel_YandereAppearBGM", true, 1f);

            // 문 앞 조명 활성화
            GameObject light = m_stage.transform.GetChild(2).GetChild(0).GetChild(1).gameObject;
            light.GetComponent<Light>().enabled = true;
            light.GetComponent<HallwayLight>().Blink = true;

            // 캐릭터 락
            m_player.Set_Lock(true);

            // 카메라 교체 및 설정
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_CUTSCENE);
            CameraCutscene camera = (CameraCutscene)GameManager.Ins.Camera.Get_CurCamera();
            camera.Change_Position(new Vector3(0f, 1.2f, 20f));
            camera.Change_Rotation(new Vector3(0f, -180f, 25f));

            // 얀데레 등장 애니메이션으로 전환
            m_yandereObj.transform.position = new Vector3(0f, 0f, 2.8f);
            m_yandereObj.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            m_yandereObj.transform.GetChild(2).GetComponent<Light>().color = Color.red;

            m_yandereObj.transform.GetChild(0).gameObject.SetActive(true); // 얀데레 메시 활성화
            m_yandereObj.SetActive(true);

            m_yandere.StateMachine.Change_State((int)HallwayYandere.YandereState.ST_APPEAR);
            Debug.Log("얀데레 초기화");

            // 페이드 인
            GameManager.Ins.UI.Start_FadeIn(1f, Color.black);
            // 돌면서 특정거리까지 줌인
            camera.Start_Position(new Vector3(0f, 1.2f, 5.5f), 2f);
            camera.Start_Rotation(new Vector3(0f, 180f, -16f), 0.5f);
            // 캐릭터가 말을 할때 얀데레 얼굴 클로즈업
            // 
        }

        private void Create_CD()
        {
            // 초기화
            for(int i = 0; i < m_cds.Count; ++i)
                GameManager.Ins.Resource.Destroy(m_cds[i]);
            m_cds.Clear();

            //List<Vector3> beforePosition = new List<Vector3>();
            //beforePosition.Add(new Vector3(0f, 0f, 0f));

            for (int i = 0; i < m_CdMaxCount - 1; i++) // 1개는 맵상 직접 배치
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

                GameObject CD = GameManager.Ins.Resource.LoadCreate("5. Prefab/1. VisualNovel/Object/CD");
                CD.GetComponent<HallwayCD>().PositionIndex = index;
                CD.transform.position = NewPosition;
                m_cds.Add(CD);
            }
        }

        private void Create_Lever(int count, int beforIndex = -1)
        {
            // 초기화
            for (int i = 0; i < m_levers.Count; ++i)
                GameManager.Ins.Resource.Destroy(m_levers[i]);
            m_levers.Clear();

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

                GameObject level = GameManager.Ins.Resource.LoadCreate("5. Prefab/1. VisualNovel/Object/Lever");
                level.GetComponent<HallwayLever>().PositionIndex = index;
                level.transform.position = NewPosition;
                m_levers.Add(level);
            }
        }

        public void Get_CD(int positionIndex)
        {
            if(positionIndex != -1)
                m_positionUse[positionIndex] = false;

            m_CdCurrentCount++;
            m_key.fillAmount = (float)m_CdCurrentCount / m_CdMaxCount;

            Update_Light();

            // 대사 출력
            if (m_itemText == null)
            {
                m_itemText = GameManager.Ins.Resource.LoadCreate("5. Prefab/1. VisualNovel/UI/UI_ItemText", m_stage.transform.GetChild(1));
                m_itemText.SetActive(false);
            }

            switch (m_CdCurrentCount)
            {
                case 1:
                    // 속도 감소 및 컷씬 재생
                    m_player.MoveSpeed = 200f;
                    GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => Appear_Monster(), 1f, false);
                    break;

                case 2:
                    Change_Text("나에게서 멀어지려하지마...");
                    break;

                case 3:
                    Change_Text("이제 그만 돌아와요!");
                    break;
            }
        }

        public void Use_Lever(int positionIndex)
        {
            m_positionUse[positionIndex] = false;

            // 아이템 효과 적용
            if (m_yandereObj != null)
                m_yandere.Used_Lever();

            // 추가 생성
            Create_Lever(1, positionIndex);
        }

        public void Change_Text(string str)
        {
            if (m_ItemTextCoroutine != null)
                GameManager.Ins.StopCoroutine(m_ItemTextCoroutine);
            GameManager.Ins.StartCoroutine(Wait_Text(str));
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

        private void Update_Light()
        {
            // 조명 업데이트 Max 464
            m_Light.Shuffle();
            int OnCount = (int)(464 / (m_CdMaxCount)) * m_CdCurrentCount;
            for (int i = 0; i < OnCount; ++i)
                m_Light[i].Blink = true;
        }

        public override List<ExcelData> Get_DialogData(int sheetIndex)
        {
            List<ExcelData> sheetList = null;
            switch (sheetIndex)
            {
                case 0:
                    sheetList = m_dialogAsset.S00_1_AyakaHome;
                    break;
                case 1:
                    sheetList = m_dialogAsset.S01_11_Minatsu;
                    break;
                case 2:
                    sheetList = m_dialogAsset.S02_12_Hina;
                    break;
                case 3:
                    sheetList = m_dialogAsset.S03_13_Ayaka;
                    break;
                case 4:
                    sheetList = m_dialogAsset.S04_1_AyakaHome2;
                    break;
            }
            return sheetList;
        }

        //private Vector3 Get_RandomPositionOnNavMesh(List<Vector3> beforePos)
        //{
        //    Vector3 position = new Vector3();
        //    bool select = false;

        //    int loopNum = 0;
        //    while (!select)
        //    {
        //        Vector3 randomPos = m_playerTr.position + Random.insideUnitSphere * m_CdMaxDistance; // 원하는 범위 내의 랜덤 방향 벡터 생성
        //        randomPos.y = 0.0f;
        //        NavMeshHit hit;

        //        // SamplePosition((Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask)
        //        // areaMask 에 해당하는 NavMesh 중에서 maxDistance 반경 내에서 sourcePosition에 가장 가까운 위치를 찾아서 그 결과를 hit에 담음
        //        if (NavMesh.SamplePosition(randomPos, out hit, m_CdMaxDistance, NavMesh.AllAreas)) // 위치 샘플링을 성공하면 참을 반환
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

        //        if (loopNum++ > 10000) // 고정 위치로 지정할 지 고민하기
        //            throw new System.Exception("Infinite Loop");
        //    }

        //    return position;
        //}
    }
}

