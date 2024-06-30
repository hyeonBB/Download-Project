using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class Western_PlayLv2 : Western_Play
    {
        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);

            m_criminalText.Add("2ļ��!!!");
            m_criminalText.Add("2����!");
            m_criminalText.Add("2ũ��...���� �� �־��µ�");
            m_criminalText.Add("2���ϴٿ�...!");
            m_criminalText.Add("2�߿���...");
            m_criminalText.Add("2����, ���״ٿ�");
            m_criminalText.Add("2�������� ��ġ�ٳ�...");
            m_criminalText.Add("2�� ������� ������!");
            m_criminalText.Add("2�� ������ �����ϴٴ�...");
            m_criminalText.Add("2����� ������...!");

            m_citizenText.Add("2������...");
            m_citizenText.Add("2������ �ù��� �����̱�");
            m_citizenText.Add("2����! ��û�� �༮�̴ٿ�");
            m_citizenText.Add("2���� �������̶� ����Ŀ�?");
            m_citizenText.Add("2�� �� �ȹٷ� �߶��");
            m_citizenText.Add("2����! �� �����ִٿ�");
            m_citizenText.Add("2�� �������� �溻 �밡�ٿ�");
            m_citizenText.Add("2��, ���ž���");
            m_citizenText.Add("2������ �տ��� �� ���� �ȴٴ�");
            m_citizenText.Add("2������ ����� ����� �˾�?");

            m_eventIndex = new List<int>();
            m_eventCount = Random.Range(3, 6); // �ּ� 3 - 5��
            List<int> availableNumbers = new List<int>();
            for (int i = 2; i <= 10; i++) // 1�� �׷� ���� 2���� 10���� �� ����
                availableNumbers.Add(i);
            for (int i = 0; i < m_eventCount; ++i)
            {
                int randomIndex = Random.Range(0, availableNumbers.Count);
                m_eventIndex.Add(availableNumbers[randomIndex]);
                availableNumbers.RemoveAt(randomIndex);
            }
            m_eventIndex.Sort();
        }

        public override void Enter_Level()
        {
            base.Enter_Level();

            // �������� ����
            m_stage = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/2Stage/2Stage"));
            m_groups = m_stage.transform.Find("Group").GetComponent<Groups>();

            // ī�޶� ����
            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_BASIC_3D);
            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_CUTSCENE);
            CameraCutscene camera = (CameraCutscene)CameraManager.Instance.Get_CurCamera();
            camera.Change_Position(new Vector3(0f, 0.62f, -55.65f));
            camera.Change_Rotation(new Vector3(2.43f, 0f, 0f));

            UIManager.Instance.Start_FadeIn(1f, Color.black, () => StartCoroutine(Update_ReadyGo()));
        }

        public override void Play_Level()
        {
            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_WALK);
            m_camera = (CameraWalk)CameraManager.Instance.Get_CurCamera();
            m_camera.Change_Rotation(new Vector3(2.43f, 0f, 0f));
            m_camera.Set_Height(0.62f);

            Proceed_Next();

            // BGM ����
            Camera.main.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("2. Sound/BGM/Silencios de Los Angeles - Cumbia Deli");
            Camera.main.GetComponent<AudioSource>().Play();
        }

        public override void Update_Level()
        {
            base.Update_Level();
        }

        public override void LateUpdate_Level()
        {
        }

        public override void Exit_Level()
        {
            base.Exit_Level();
        }

        public override void Play_Finish()
        {
            // BGM ����
            Camera.main.GetComponent<AudioSource>().clip = Instantiate(Resources.Load<AudioClip>("2. Sound/BGM/La Docerola - Quincas Moreira"));
            Camera.main.GetComponent<AudioSource>().Play();
        }
    }
}

