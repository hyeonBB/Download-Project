using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Western_PlayLv1 : Level
{
    enum STATETYPE { TYPE_DIALOGSTART, TYPE_TUTORIALPLAY, TYPE_DIALOGFINISH, TYPE_GAMESTART, TYPE_END };

    private GameObject m_stage;
    private Groups     m_groups;
    private GameObject m_targetUI;

    private List<GameObject> m_tutorialTarget;
    GameObject UIObject = null;

    private bool m_isTutorial = true;
    private STATETYPE m_stateType = STATETYPE.TYPE_END;

    private int m_tutorialIndex = -1;
    private float m_time = 0;

    public Western_PlayLv1(LevelController levelController) : base(levelController)
    {
        m_tutorialTarget = new List<GameObject>();
    }

    public override void Enter_Level()
    {
        // �������� ����
        m_stage  = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/1Stage/1Stage"));
        m_groups = m_stage.transform.Find("Group").GetComponent<Groups>();

        // ī�޶� ����
        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_BASIC_3D);
        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_CUTSCENE);
        CameraCutscene camera = (CameraCutscene)CameraManager.Instance.Get_CurCamera();
        camera.Change_Position(new Vector3(0f, 0.62f, -55.65f));
        camera.Change_Rotation(new Vector3(2.43f, 0f, 0f));

        // ���̾�α� ����
        UIManager.Instance.Start_FadeIn(1f, Color.black, () => Start_Dialog());
    }

    public override void Play_Level()
    {
        m_isTutorial = false;
        m_tutorialTarget.Clear();

        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_WALK);
    }

    public override void Update_Level()
    {
        if (m_isTutorial)
            Update_Tutorial();
        else
            Update_Play();
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
        if (m_stateType == STATETYPE.TYPE_DIALOGSTART) // ������ ���̾�αװ� ���� ��µǸ� �ǳ� Ȱ��ȭ �� Ʃ�丮�� ����
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
                            Vector3 position = new Vector3(hit.point.x, hit.point.y, hit.point.z - 0.01f);
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

                    // Ʃ�丮�󿡼� �̹� ���� ����� �� ��鸲.
                    if (Check_Person(m_targetUI.GetComponent<TargetUI>().Target))
                    {
                        m_targetUI.GetComponent<TargetUI>().Target.GetComponent<Person>().Start_Shake();
                    }
                    else
                    {
                        if (m_targetUI.GetComponent<TargetUI>().Target.GetComponent<Person>().PersonType == Person.PERSONTYPE.PT_CRIMINAL) // ������ ��
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

                        // �Ѿ��ڱ� ������Ʈ ����.
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
            }
        }
        else if(m_stateType == STATETYPE.TYPE_GAMESTART) // ���� �� UI ��� �� ���� ����
        {
            m_time += Time.deltaTime;
            if (m_time >= 1f)
            {
                m_time = 0f;
                switch (m_tutorialIndex)
                {
                    case 0:
                        UIObject = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/UI_ReadyGo"), GameObject.Find("Canvas").transform);
                        UIObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("1. Graphic/2D/2. Western/UI/Play/Start/Ready");
                        break;

                    case 1:
                        UIObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("1. Graphic/2D/2. Western/UI/Play/Start/Go");
                        break;

                    case 2:
                        Destroy(UIObject);
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


    private void Update_Play()
    {

    }
}
