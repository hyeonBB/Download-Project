using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Event : Interaction
{
    public enum EVENTTYPE { TYPE_CUTSCENE_1FBOSS, TYPE_END };

    [SerializeField] UIWorldHint.HINTTYPE m_hintType; // ���� UI ��Ʈ Ÿ��
    [SerializeField] EVENTTYPE m_eventType;


    private void Start()
    {
        GameObject gameObject = HorrorManager.Instance.Create_WorldHintUI(m_hintType, transform.GetChild(0), m_uiOffset);
        m_interactionUI = gameObject.GetComponent<UIWorldHint>();
    }

    private void Update()
    {
        //Update_InteractionUI();
    }

    public override void Click_Interaction()
    {
        if (No_Click())
            return;

        switch(m_eventType)
        {
            case EVENTTYPE.TYPE_CUTSCENE_1FBOSS:
                Debug.Log("1�� ���� ���� �ƾ� ���");
                break;
        }

        Check_Delete();
    }
}