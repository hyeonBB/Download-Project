using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Text : Interaction
{
    [SerializeField] UIWorldHint.HINTTYPE m_hintType; // ���� UI ��Ʈ Ÿ��
    [SerializeField] private UIInstruction.ACTIVETYPE m_uiOpenType;
    [SerializeField] private UIInstruction.ACTIVETYPE m_uiCloseType;

    [SerializeField] private float[] m_activeTimes;
    [SerializeField] private string[] m_texts;
    [SerializeField] private bool m_repeat = false; // �ݺ� ����

    private void Start()
    {
        GameObject gameObject = GameManager.Ins.Horror.Create_WorldHintUI(m_hintType, transform.GetChild(0), m_uiOffset);
        m_interactionUI = gameObject.GetComponent<UIWorldHint>();

        for (int i = 0; i < m_texts.Length; ++i)
            m_texts[i] = m_texts[i].Replace("\\n", "\n"); // �ν����Ϳ��� �Էµ� ���ڿ��� �⺻������ �̽������� ���ڷ� �νĵ��� �ʰ� �׳� �Ϲ� ���ڿ��� ó��
    }

    private void Update()
    {
        //Update_InteractionUI();
    }

    public override void Click_Interaction()
    {
        if (No_Click())
            return;

        GameManager.Ins.Horror.Active_InstructionUI(m_uiOpenType, m_uiCloseType, m_activeTimes, m_texts);

        if(m_repeat == false)
            Destroy_Interaction();
    }
}