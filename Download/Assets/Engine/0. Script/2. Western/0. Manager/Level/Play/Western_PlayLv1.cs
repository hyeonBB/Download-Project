using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Western_PlayLv1 : Western_Play
{
    enum STATETYPE { TYPE_DIALOGSTART, TYPE_TUTORIALPLAY, TYPE_DIALOGFINISH, TYPE_GAMESTART, TYPE_END };

    private STATETYPE m_stateType = STATETYPE.TYPE_END;
    private List<GameObject> m_tutorialTarget = new List<GameObject>();
    private bool m_isTutorial    = true;
    private int  m_tutorialIndex = -1;

    public Western_PlayLv1(LevelController levelController) : base(levelController)
    {
        m_criminalText.Add("캬옹!!!");
        m_criminalText.Add("으악!");
        m_criminalText.Add("크흑...숨길 수 있었는데");
        m_criminalText.Add("분하다옹...!");
        m_criminalText.Add("야오옹...");
        m_criminalText.Add("젠장, 들켰다옹");
        m_criminalText.Add("고양이의 수치다냥...");
        m_criminalText.Add("내 사랑스런 수염이!");
        m_criminalText.Add("내 은신을 간파하다니...");
        m_criminalText.Add("당신은 전설의...!");

        m_citizenText.Add("후후후...");
        m_citizenText.Add("선량한 시민을 고르셨군");
        m_citizenText.Add("하하! 멍청한 녀석이다옹");
        m_citizenText.Add("없던 수전증이라도 생겼냐옹?");
        m_citizenText.Add("두 눈 똑바로 뜨라옹");
        m_citizenText.Add("어이! 나 여기있다옹");
        m_citizenText.Add("내 움직임을 얏본 대가다옹");
        m_citizenText.Add("흥, 별거없군");
        m_citizenText.Add("고양이 앞에서 한 눈을 팔다니");
        m_citizenText.Add("고양이 목숨이 몇개인지 알아?");
    }

    public override void Enter_Level()
    {
        // 스테이지 생성
        m_stage  = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/1Stage/1Stage"));
        m_groups = m_stage.transform.Find("Group").GetComponent<Groups>();

        // 카메라 설정
        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_BASIC_3D);
        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_CUTSCENE);
        CameraCutscene camera = (CameraCutscene)CameraManager.Instance.Get_CurCamera();
        camera.Change_Position(new Vector3(0f, 0.62f, -55.65f));
        camera.Change_Rotation(new Vector3(2.43f, 0f, 0f));

        // 다이얼로그 시작
        UIManager.Instance.Start_FadeIn(1f, Color.black, () => Start_Dialog());
    }

    public override void Play_Level() // 튜토리얼 진행 후 Ready Go UI 출력 후 해당 함수 호출
    {
        m_tutorialTarget.Clear();
        m_isTutorial = false;

        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_WALK);
        m_camera = (CameraWalk)CameraManager.Instance.Get_CurCamera();
        m_camera.Change_Rotation(new Vector3(2.43f, 0f, 0f));
        m_camera.Set_Height(0.62f);

        Proceed_Next();
    }

    public override void Update_Level()
    {
        if (m_isTutorial)
            Update_Tutorial();
        else
            base.Update_Level();
    }

    public override void LateUpdate_Level()
    {
    }

    public override void Exit_Level()
    {
        Destroy(m_stage);
    }


    private void Start_Dialog()
    {
        m_stateType = STATETYPE.TYPE_DIALOGSTART;
        WesternManager.Instance.DialogPlay.GetComponent<Dialog_PlayWT>().Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_PlayWT>("Assets/Resources/4. Data/2. Western/Dialog/Play/Round1/Dialog1_Play.json"));
    }

    private void Update_Tutorial()
    {
        if (m_stateType == STATETYPE.TYPE_DIALOGSTART) // 마지막 다이얼로그가 전부 출력되면 판넬 활성화 및 튜토리얼 진행
        {
            if (WesternManager.Instance.DialogPlay.LastIndex == true)
            {
                m_stateType = STATETYPE.TYPE_TUTORIALPLAY;
                m_groups.WakeUp_Next(false);
            }
        }
        else if(m_stateType == STATETYPE.TYPE_TUTORIALPLAY)
        {
            if (WesternManager.Instance.IsShoot == true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.CompareTag("Person"))
                        {
                            Vector3 position = new Vector3(hit.point.x, hit.point.y, hit.point.z - 0.001f);
                            if (m_targetUI == null)
                                m_targetUI = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/TargetUI"), position, Quaternion.identity);
                            else
                                m_targetUI.transform.position = position;

                            m_targetUI.GetComponent<TargetUI>().Target = hit.collider.gameObject;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (m_targetUI == null)
                        return;

                    // 튜토리얼에서 이미 쐈던 사람일 시 흔들림.
                    if (Check_Person(m_targetUI.GetComponent<TargetUI>().Target))
                    {
                        m_targetUI.GetComponent<TargetUI>().Target.GetComponent<Person>().Start_Shake();
                    }
                    else
                    {
                        if (m_targetUI.GetComponent<TargetUI>().Target.GetComponent<Person>().PersonType == Person.PERSONTYPE.PT_CRIMINAL) // 범인일 때
                        {
                            WesternManager.Instance.IsShoot = false;
                            WesternManager.Instance.DialogPlay.GetComponent<Dialog_PlayWT>().Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_PlayWT>("Assets/Resources/4. Data/2. Western/Dialog/Play/Round1/Dialog1_Tutorial_Criminal.json"));
                            m_stateType = STATETYPE.TYPE_DIALOGFINISH;
                        }
                        else
                        {
                            m_tutorialIndex++;
                            switch (m_tutorialIndex)
                            {
                                case 0:
                                    WesternManager.Instance.DialogPlay.GetComponent<Dialog_PlayWT>().Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_PlayWT>("Assets/Resources/4. Data/2. Western/Dialog/Play/Round1/Dialog1_Tutorial_Citizen1.json"));
                                    break;

                                case 1:
                                    WesternManager.Instance.DialogPlay.GetComponent<Dialog_PlayWT>().Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_PlayWT>("Assets/Resources/4. Data/2. Western/Dialog/Play/Round1/Dialog1_Tutorial_Citizen2.json"));
                                    break;
                            }

                            m_tutorialTarget.Add(m_targetUI.GetComponent<TargetUI>().Target);
                        }

                        // 총알자국 오브젝트 생성.
                        Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/BulletMarkUI"), m_targetUI.transform.position, Quaternion.identity, m_targetUI.GetComponent<TargetUI>().Target.transform);
                    }

                    Destroy(m_targetUI);
                }
            }
        }
        else if (m_stateType == STATETYPE.TYPE_DIALOGFINISH)
        {
            if (WesternManager.Instance.DialogPlay.Active == false)
            {
                m_tutorialIndex = 0;
                m_stateType = STATETYPE.TYPE_GAMESTART;

                m_groups.LayDown_Group();
            }
        }
        else if(m_stateType == STATETYPE.TYPE_GAMESTART) // 레디 고 UI 출력 후 게임 시작
        {
            m_uiTime += Time.deltaTime;
            if (m_uiTime >= 1f)
            {
                m_uiTime = 0f;
                switch (m_tutorialIndex)
                {
                    case 0:
                        m_readyGoUI = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/UI_ReadyGo"), GameObject.Find("Canvas").transform);
                        m_readyGoUI.GetComponent<Image>().sprite = Resources.Load<Sprite>("1. Graphic/2D/2. Western/UI/Play/Start/Ready");
                        break;

                    case 1:
                        m_readyGoUI.GetComponent<Image>().sprite = Resources.Load<Sprite>("1. Graphic/2D/2. Western/UI/Play/Start/Go");
                        break;

                    case 2:
                        Destroy(m_readyGoUI);
                        Play_Level();
                        break;
                }

                m_tutorialIndex++;
            }
        }
    }

    private bool Check_Person(GameObject target)
    {
        for(int i = 0; i < m_tutorialTarget.Count; ++i)
        {
            if (m_tutorialTarget[i] == target)
                return true;
        }

        return false;
    }
}
