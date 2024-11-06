using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFail : MonoBehaviour
{
    private GameObject m_handObject;

    public void Start_PanelFail(GameObject handObject)
    {
        GameManager.Ins.Set_Pause(true);
        m_handObject = handObject;
    }

    public void Button_Yes()
    {
        GameManager.Ins.UI.EventUpdate = true;
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => Yes_Update(), 1f, false);
    }

    private void Yes_Update()
    {
        // ��, �г� ����
        Destroy(m_handObject);
        Destroy(gameObject);

        // �߰� ���ӹ�� ���� UI���� �ٽ� ����
        GameManager.Ins.Novel.LevelController.Change_Level((int)VisualNovelManager.LEVELSTATE.LS_DAY3CHASEGAME, true);
    }

    public void Button_No()
    {
        GameManager.Ins.UI.EventUpdate = true;
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.Change_Scene(StageManager.STAGE.LEVEL_WINDOW), 1f, false);
    }
}
