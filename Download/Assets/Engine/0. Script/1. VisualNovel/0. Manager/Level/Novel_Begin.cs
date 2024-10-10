using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class Novel_Begin : Level
    {
        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            VisualNovelManager manager = GameManager.Ins.Novel;

            manager.Dialog.SetActive(true);
            manager.Dialog.GetComponent<Dialog_VN>().Start_Dialog(GameManager.Ins.Load_JsonData<DialogData_VN>("4. Data/1. VisualNovel/Dialog/Dialog1_School")); // �� ��� �� �� Ȯ���� ����

            GameManager.Ins.Sound.Play_AudioSourceBGM("VisualNovel_ScriptBGM", true, 1f);

            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_BASIC_2D);
        }

        public override void Play_Level()
        {
        }

        public override void Update_Level()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                GameManager.Ins.Novel.Active_Popup();
        }

        public override void Exit_Level()
        {
        }

        public override void OnDrawGizmos()
        {
        }
    }
}
