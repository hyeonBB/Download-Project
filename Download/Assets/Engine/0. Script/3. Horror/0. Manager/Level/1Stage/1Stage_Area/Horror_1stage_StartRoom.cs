using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horror_1stage_StartRoom : Area
{
    public override void Initialize_Level(LevelController levelController)
    {
        base.Initialize_Level(levelController);

        m_levelIndex = (int)Horror_1stage.LEVEL1.LV_STARTROOM;
    }


    public override bool Check_Clear(Interaction_Door interaction_Door, ref float[] activeTimes, ref string[] texts)
    {
        // �������� ȹ���ߴ°�?
        if (HorrorManager.Instance.Player.WeaponManagement.Get_WeaponIndex(NoteItem.ITEMTYPE.TYPE_PIPE) != -1)
            return true;

        activeTimes = new float[1];
        texts = new string[1];
        activeTimes[0] = 1f;
        texts[0] = "�� �ʸӸ� Ȯ���� �غ� ���� ���� �� ����.";
        return false;
    }


    public override void Enter_Level()
    {
    }

    public override void Play_Level()
    {
    }

    public override void Update_Level()
    {
    }

    public override void LateUpdate_Level()
    {
    }

    public override void Exit_Level()
    {
    }

    public override void OnDrawGizmos()
    {
    }
}
