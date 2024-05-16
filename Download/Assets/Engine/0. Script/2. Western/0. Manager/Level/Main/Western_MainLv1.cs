using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Western_MainLv1 : Western_Main
{
    public Western_MainLv1(LevelController levelController) : base(levelController)
    {
    }

    public override void Enter_Level()
    {
        base.Enter_Level();
    }

    public override void Play_Level()
    {
    }

    public override void Update_Level()
    {
        base.Update_Level();
    }

    public override void LateUpdate_Level()
    {
        base.LateUpdate_Level();
    }

    public override void Exit_Level()
    {
        base.Exit_Level();
    }


    protected override void Start_Dialog()
    {
        m_dialogStart = true;
        WesternManager.Instance.DialogPlay.GetComponent<Dialog_PlayWT>().Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_PlayWT>("Assets/Resources/4. Data/2. Western/Dialog/Play/Round1/Dialog1_Main.json"));
    }
}