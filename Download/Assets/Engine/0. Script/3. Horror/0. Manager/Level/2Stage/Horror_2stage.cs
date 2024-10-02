using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horror_2stage : Horror_Base
{
    public enum LEVEL2 { LV_END };

    public override void Initialize_Level(LevelController levelController)
    {
        base.Initialize_Level(levelController);

        m_levelIndex = (int)HorrorManager.LEVEL.LV_2STAGE;
        m_playerSpeedAdd = 100;
    }

    public override bool Check_Clear(Interaction_Door interaction_Door, ref float[] activeTimes, ref string[] texts)
    {
        return true;
    }

    public override void Enter_Level()
    {
        if (m_IsVisit == false)
        {
            m_IsVisit = true;
            m_stage = GameObject.Find("World").transform.GetChild(2).gameObject;
        }

        m_stage.SetActive(true);

        // �÷��̾� ��ġ �� ȸ�� ����
        Transform playerTransform = GameManager.Ins.Horror.Player.gameObject.transform;
        playerTransform.position = new Vector3(-37.97f, 135.66f, 127.5f);//new Vector3(0f, 134.8f, -90.16f);
        playerTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
        CameraFollow camera = (CameraFollow)GameManager.Ins.Camera.Get_CurCamera();
        Camera.main.transform.position = camera.CameraPositionTarget.position;
        Camera.main.transform.rotation = camera.CameraRotationTarget.rotation;

        GameManager.Ins.UI.EventUpdate = true;
        GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Play_Level());
    }

    public override void Play_Level()
    {
        GameManager.Ins.Set_Pause(false); // �Ͻ����� ����
    }

    public override void Update_Level()
    {
        // m_levels.Update_Level();
    }

    public override void LateUpdate_Level()
    {
    }

    public override void Exit_Level()
    {
        m_stage.SetActive(false);
    }

    public override void OnDrawGizmos()
    {
    }
}