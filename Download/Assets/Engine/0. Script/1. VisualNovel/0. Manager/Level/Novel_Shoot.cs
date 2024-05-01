using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Novel_Shoot : Level
{
    private DOLLTYPE m_dollType = DOLLTYPE.DT_END;

    private bool m_shootGameStart = false;
    private bool m_shootGameStop = false;
    private bool m_shootGameOver = false;
    private bool m_shootGameNext = false;
    private float m_time    = 0f;
    private float m_maxTime = 60f;
    private float m_overTime = 0f;

    public DOLLTYPE DollType
    {
        get => m_dollType;
        set => m_dollType = value;
    }
    public bool ShootGameStart
    {
        get => m_shootGameStart;
        set => m_shootGameStart = value;
    }
    public bool ShootGameStop
    {
        get => m_shootGameStop;
        set => m_shootGameStop = value;
    }
    public bool ShootGameOver
    {
        get => m_shootGameOver;
        set => m_shootGameOver = value;
    }

    public Novel_Shoot(LevelController levelController) : base(levelController)
    {
    }

    public override void Enter_Level()
    {
        VisualNovelManager.Instance.Dialog.SetActive(false);
        VisualNovelManager.Instance.ChaseGame.SetActive(false);
        VisualNovelManager.Instance.ShootGame.SetActive(true);

        m_time = m_maxTime;
        VisualNovelManager.Instance.CountTxt.text = m_time.ToString();

        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_BASIC_2D);
        UIManager.Instance.Start_FadeIn(1f, Color.black);
    }

    public override void Play_Level()
    {
        m_shootGameStart = true;
        CursorManager.Instance.Change_Cursor(CURSORTYPE.CT_NOVELSHOOT);
        VisualNovelManager.Instance.Container.Start_Belt();
    }

    public override void Update_Level()
    {
        if (!m_shootGameStart || m_shootGameStop)
            return;

        if (!m_shootGameOver)
            Update_Count();
        else
            GameOver_ShootGame();
    }

    public override void Exit_Level()
    {
        CursorManager.Instance.Change_Cursor(CURSORTYPE.CT_ORIGIN);
        Destroy(VisualNovelManager.Instance.ShootGame); // ��������� ���� �� ����
    }

    public override void OnDrawGizmos()
    {
    }

    private void Update_Count()
    {
        m_time -= Time.deltaTime;
        if (m_time <= 0.5f)
        {
            int Count = 0;
            VisualNovelManager.Instance.CountTxt.text = Count.ToString();

            m_shootGameOver = true;
            VisualNovelManager.Instance.Container.UseBelt = false; // 1) ���� �Ͻ� ����

            m_dollType = DOLLTYPE.DT_FAIL;

        }
        else
        {
            int Count = (int)m_time;
            VisualNovelManager.Instance.CountTxt.text = Count.ToString();
        }
    }

    private void GameOver_ShootGame()
    {
        if (m_shootGameNext)
            return;

        // ���� �Ǵ� ������ 1�� �̻� ȹ���ص� ��� ���� ����/ �̿��� ���� : ���� ������ ���� ���� ��簡 �ٸ�.
        m_overTime += Time.deltaTime;
        if (m_overTime > 1.5f)
        {
            if (!VisualNovelManager.Instance.Container.OverEffect)
                VisualNovelManager.Instance.Container.Over_Game(); // 2) 1.5�� �� ���� ���� ����
            else if (!m_shootGameNext && m_overTime > 3) // 3) 1.5�� �� ���̵� �ƿ����� ��ȯ
            {
                m_shootGameNext = true;
                UIManager.Instance.Start_FadeOut(1f, Color.black, () => m_levelController.Change_Level((int)VisualNovelManager.LEVELSTATE.LS_NOVELEND), 0.5f, false);
            }
        }
    }
}
