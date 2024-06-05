using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Western
{
    public abstract class Western_Play : Level
    {
        protected GameObject m_stage     = null;
        protected Groups     m_groups    = null;
        protected GameObject m_targetUI  = null;
        protected CameraWalk m_camera    = null;
        protected GameObject m_readyGoUI = null;

        protected int   m_life = 5;
        protected bool  m_startGroup = false;
        protected float m_uiTime  = 0f;
        protected int   m_uiIndex = 0;

        protected List<string> m_criminalText = new List<string>();
        protected List<string> m_citizenText  = new List<string>();

        private bool m_finishGroup = false;
        private bool m_fadeOut     = false;

        protected int m_eventCount;
        protected List<int> m_eventIndex;

        public Groups Groups => m_groups;
        public bool finishGroup
        {
            get => m_finishGroup;
            set => m_finishGroup = value;
        }
        public GameObject TargetUI
        {
            get => m_targetUI;
            set => m_targetUI = value;
        }

        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            WesternManager.Instance.HeartUI.Reset_Heart();
        }

        public override void Play_Level()
        {
        }

        public override void Update_Level()
        {
            if(m_finishGroup != true)
            {
                // �ش� ������ �������� �� �ǳ� �����
                if (m_startGroup == true && m_camera.IsMove == false)
                {
                    m_startGroup = false;
                    m_groups.WakeUp_Next(ref m_eventIndex, true, 0.4f);
                }
                else if (WesternManager.Instance.IsShoot == true)
                {
                    if (Input.GetMouseButtonDown(0)) { Click_Panel(); }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (m_targetUI == null)
                            return;

                        WesternManager.Instance.IsShoot = false;
                        if (m_targetUI.GetComponent<TargetUI>().Target.GetComponent<Person>().PersonType == Person.PERSONTYPE.PT_CRIMINAL) // ������ ��
                            Create_SpeechBubble(Person.PERSONTYPE.PT_CRIMINAL, m_groups.Get_Criminal().transform.position, ref m_criminalText, Random.Range(0, m_criminalText.Count));
                        else
                            Create_SpeechBubble(Person.PERSONTYPE.PT_CITIZEN, m_groups.Get_Criminal().transform.position, ref m_citizenText, Random.Range(0, m_citizenText.Count));

                        Space_Panel();
                        Destroy(m_targetUI);
                    }
                }
            }
            else
            {
                if(m_fadeOut == false && Camera.main.GetComponent<AudioSource>().isPlaying == false)
                {
                    // ���̵� �ƿ�
                    m_fadeOut = true;
                    UIManager.Instance.Start_FadeOut(1f, Color.black, () => WesternManager.Instance.LevelController.Change_NextLevel(), 0f, false);
                }
            }

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.N)) {
                UIManager.Instance.Start_FadeOut(1f, Color.black, () => WesternManager.Instance.LevelController.Change_NextLevel(), 0f, false);
            }
#endif
        }

        public abstract void Play_Finish();

        public override void Exit_Level()
        {
            Destroy(m_stage);
        }


        protected IEnumerator Update_ReadyGo()
        {
            m_uiIndex = 0;
            while (m_uiIndex < 3)
            {
                m_uiTime += Time.deltaTime;
                if (m_uiTime >= 1f)
                {
                    m_uiTime = 0f;
                    switch (m_uiIndex)
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

                    m_uiIndex++;
                }

                yield return null;
            }

            yield break;
        }

        protected void Click_Panel()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Person"))
                {
                    Vector3 position = new Vector3(hit.point.x, hit.point.y, hit.collider.gameObject.GetComponent<Person>().Get_GroupZ() - 0.005f); // �� ��
                    if (m_targetUI == null)
                        m_targetUI = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/TargetUI"), position, Quaternion.identity);
                    else
                        m_targetUI.transform.position = position;

                    m_targetUI.GetComponent<TargetUI>().Target = hit.collider.gameObject;
                }
            }
        }

        protected void Space_Panel()
        {
            // Ÿ�� ����ũ
            m_targetUI.GetComponent<TargetUI>().Target.GetComponent<Person>().Start_Shake();

            // �Ͼ�� ȭ������ ��½ ȿ�� ���� (������ �ѹ��� ������°� ������ ������)
            UIManager.Instance.Start_FadeIn(0.3f, Color.white);

            // ����Ʈ ����
            Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/1Stage/Effect/Person_Effect"), m_targetUI.transform.position, Quaternion.identity);

            // �Ѿ��ڱ� ������Ʈ ����.
            Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/BulletMarkUI"), m_targetUI.transform.position, Quaternion.identity, m_targetUI.GetComponent<TargetUI>().Target.transform);
        }

        private void Create_SpeechBubble(Person.PERSONTYPE type, Vector3 position, ref List<string> textlist, int index)
        {
            // Ÿ�̸� ����
            m_groups.Destroy_Timer();

            // ��ǳ�� UI
            GameObject uiObject = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/UI_SpeechBubble"), GameObject.Find("Canvas").transform);
            uiObject.GetComponent<SpeechBubble>().PersonType = type;
            uiObject.GetComponent<Transform>().position = Camera.main.WorldToScreenPoint(position + new Vector3(0f, 0.5f, 0f));
            uiObject.transform.GetChild(0).GetComponent<TMP_Text>().text = textlist[index];
            textlist.RemoveAt(index);
        }


        public void Proceed_Next()
        {
            m_startGroup = true;

            // ī�޶� ������Ʈ
            m_camera.Start_Move(m_groups.Next_Position());

            // UI ������Ʈ
            WesternManager.Instance.StatusBarUI.Start_UpdateValue(m_groups.CurrentIndex, m_groups.CurrentIndex + 1,
                Camera.main.transform.position, m_groups.Next_Position());
        }

        public void Fail_Group()
        {
            if (m_targetUI != null)
                Destroy(m_targetUI);

            m_groups.Destroy_Timer();

            WesternManager.Instance.IsShoot = false;
            m_groups.Get_Criminal().GetComponent<Criminal>().Change_Attack();
        }

        public void Attacked_Player(bool laydown = true)
        {
            m_life--;
            WesternManager.Instance.HeartUI.Start_Update(m_life, laydown);
        }

        public void LayDown_Group(bool nextMove = false)
        {
            m_groups.LayDown_Group(nextMove);
        }
    }
}

