using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Western
{
    public class Western_PlayLv1 : Western_Play
    {
        enum STATETYPE { TYPE_DIALOGSTART, TYPE_TUTORIALPLAY, TYPE_DIALOGFINISH, TYPE_GAMESTART, TYPE_END };

        private STATETYPE m_stateType = STATETYPE.TYPE_END;
        private List<GameObject> m_tutorialTarget = new List<GameObject>();
        private bool m_isTutorial = true;
        private int m_tutorialIndex = -1;

        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);

            m_criminalText.Add("ļ��!!!");
            m_criminalText.Add("����!");
            m_criminalText.Add("ũ��...���� �� �־��µ�");
            m_criminalText.Add("���ϴٿ�...!");
            m_criminalText.Add("�߿���...");
            m_criminalText.Add("����, ���״ٿ�");
            m_criminalText.Add("������� ��ġ�ٳ�...");
            m_criminalText.Add("�� ������� ������!");
            m_criminalText.Add("�� ������ �����ϴٴ�...");
            m_criminalText.Add("����� ������...!");

            m_citizenText.Add("������...");
            m_citizenText.Add("������ �ù��� ���̱�");
            m_citizenText.Add("����! ��û�� �༮�̴ٿ�");
            m_citizenText.Add("���� �������̶� ����Ŀ�?");
            m_citizenText.Add("�� �� �ȹٷ� �߶��");
            m_citizenText.Add("����! �� �����ִٿ�");
            m_citizenText.Add("�� �������� �溻 �밡�ٿ�");
            m_citizenText.Add("��, ���ž���");
            m_citizenText.Add("����� �տ��� �� ���� �ȴٴ�");
            m_citizenText.Add("����� ����� ����� �˾�?");
        }

        public override void Enter_Level()
        {
            base.Enter_Level();

            // �������� ����
            m_stage = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/1Stage/1Stage"));
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

        public override void Play_Level() // Ʃ�丮�� ���� �� Ready Go UI ��� �� �ش� �Լ� ȣ��
        {
            m_tutorialTarget.Clear();
            m_isTutorial = false;

            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_WALK);
            m_camera = (CameraWalk)CameraManager.Instance.Get_CurCamera();
            m_camera.Change_Rotation(new Vector3(2.43f, 0f, 0f));
            m_camera.Set_Height(0.62f);

            Proceed_Next();

            // BGM ����
            Camera.main.GetComponent<AudioSource>().clip = Instantiate(Resources.Load<AudioClip>("2. Sound/BGM/Silencios de Los Angeles - Cumbia Deli"));
            Camera.main.GetComponent<AudioSource>().Play();
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
            base.Exit_Level();
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
            else if (m_stateType == STATETYPE.TYPE_TUTORIALPLAY)
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

                    m_groups.LayDown_Group();
                }
            }
            else if (m_stateType == STATETYPE.TYPE_GAMESTART) // ���� �� UI ��� �� ���� ����
            {
                StartCoroutine(Update_ReadyGo());
                m_stateType = STATETYPE.TYPE_END;
            }
        }

        private bool Check_Person(GameObject target)
        {
            for (int i = 0; i < m_tutorialTarget.Count; ++i)
            {
                if (m_tutorialTarget[i] == target)
                    return true;
            }

            return false;
        }

        public override void Play_Finish()
        {
            /*
             * - �������� ���� óġ�ϰ� �����ϸ� ���� �� �������� ��ҿ��� ���缭 ȯȣ�� �޴´�. ���� ���ڱ� �ִϸ��̼� �ٲ� ���ߴ°ɷ� ��
             * - 1����� ���ٴ��� �ֺ� ������� �뷡�� ���� �����.
             * - ������ ��ƼŬ ����
            */

            // BGM ����
            Camera.main.GetComponent<AudioSource>().clip = Instantiate(Resources.Load<AudioClip>("2. Sound/BGM/La Docerola - Quincas Moreira"));
            Camera.main.GetComponent<AudioSource>().Play();
        }
    }
}

