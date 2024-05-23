using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class Western_PlayLv3 : Western_Play
    {
        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);

            m_criminalText.Add("3ļ��!!!");
            m_criminalText.Add("3����!");
            m_criminalText.Add("3ũ��...���� �� �־��µ�");
            m_criminalText.Add("3���ϴٿ�...!");
            m_criminalText.Add("3�߿���...");
            m_criminalText.Add("3����, ���״ٿ�");
            m_criminalText.Add("3������� ��ġ�ٳ�...");
            m_criminalText.Add("3�� ������� ������!");
            m_criminalText.Add("3�� ������ �����ϴٴ�...");
            m_criminalText.Add("3����� ������...!");

            m_citizenText.Add("3������...");
            m_citizenText.Add("3������ �ù��� ���̱�");
            m_citizenText.Add("3����! ��û�� �༮�̴ٿ�");
            m_citizenText.Add("3���� �������̶� ����Ŀ�?");
            m_citizenText.Add("3�� �� �ȹٷ� �߶��");
            m_citizenText.Add("3����! �� �����ִٿ�");
            m_citizenText.Add("3�� �������� �溻 �밡�ٿ�");
            m_citizenText.Add("3��, ���ž���");
            m_citizenText.Add("3����� �տ��� �� ���� �ȴٴ�");
            m_citizenText.Add("3����� ����� ����� �˾�?");
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
            Camera.main.GetComponent<AudioSource>().clip = Instantiate(Resources.Load<AudioClip>("2. Sound/BGM/Silencios de Los Angeles - Cumbia Deli"));
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

