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
            m_stage = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/2Stage/2Stage");
            m_groups = m_stage.transform.Find("Group").GetComponent<Groups>();

            // ī�޶� ����
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_BASIC_3D);
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_CUTSCENE);
            CameraCutscene camera = (CameraCutscene)GameManager.Ins.Camera.Get_CurCamera();
            camera.Change_Position(new Vector3(0f, 0.62f, -55.65f));
            camera.Change_Rotation(new Vector3(2.43f, 0f, 0f));

            GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => GameManager.Ins.StartCoroutine(Update_ReadyGo()));
        }

        public override void Play_Level()
        {
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_WALK);
            m_camera = (CameraWalk)GameManager.Ins.Camera.Get_CurCamera();
            m_camera.Change_Rotation(new Vector3(2.43f, 0f, 0f));
            m_camera.Set_Height(0.62f);

            Proceed_Next();

            // BGM ����
            GameManager.Ins.Sound.Play_AudioSourceBGM("Western_PlayBGM", true, 1f);
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
            GameManager.Ins.Sound.Play_AudioSourceBGM("Western_BarBGM", true, 1f);
        }
    }
}

