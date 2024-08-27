using Horror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopup : MonoBehaviour
{
    public enum TYPE { T_NOTE, T_WEAPON, T_QUESTITEM, T_EXPENITEM, T_CLUE, T_END };

    private TYPE      m_type = TYPE.T_END;
    private NoteItem  m_itemInfo;

    public void Initialize_UI(TYPE type, NoteItem itemInfo)
    {
        m_type      = type;
        m_itemInfo  = itemInfo;

        HorrorManager.Instance.Set_Pause(true); // ���� �Ͻ�����
        if (m_type == TYPE.T_QUESTITEM) // ����Ʈ ���� ������ (��������/ �ΰ���)
            transform.GetChild(0).GetChild(1).gameObject.SetActive(true); // �ΰ��� ��ư
        else // ��Ʈ, ���, �Ҹ�ǰ ������, �ܼ� (��������)
            transform.GetChild(0).GetChild(1).gameObject.SetActive(false); // �ΰ��� ��ư
        transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = HorrorManager.Instance.NoteElementIcon[m_itemInfo.m_imageName];

        gameObject.SetActive(true);
    }

    public void Button_Acquire()
    {
        switch (m_type)
        {
            case TYPE.T_QUESTITEM: // ����Ʈ ���� ������
                Type_QuestItem();
                break;
            case TYPE.T_NOTE: // ��Ʈ
                Type_Note(); 
                break;
            case TYPE.T_WEAPON: // ���
                Type_Weapon(); 
                break;
            case TYPE.T_EXPENITEM: // �Ҹ�ǰ ������
                Type_Expenitem(); 
                break;
            case TYPE.T_CLUE: // �ܼ�
                Type_Clue(); 
                break;
        }

        HorrorManager.Instance.Set_Pause(false); // �Ͻ����� ����
        gameObject.SetActive(false);
    }

    public void Button_Leave()
    {
        HorrorManager.Instance.Set_Pause(false); // �Ͻ����� ����
        gameObject.SetActive(false);
    }


    public void Type_QuestItem()
    {
        // ����Ʈ ���տ� ������ �߰�
        Note playerNote = HorrorManager.Instance.Player.Note;
        if (playerNote == null)
            return;
        playerNote.Add_Item(m_itemInfo);
    }

    public void Type_Note()
    {
        HorrorManager.Instance.Player.Acquire_Note(); // ��Ʈ �߰�

        float[] activeTimes = new float[2];
        string[] texts = new string[2];
        activeTimes[0] = 2f;
        texts[0] = "���ᰡ ����ϴ� ��ø�� �� �ϴ�...\n������ ���캸��.";
        activeTimes[1] = 3f;
        texts[1] = "[TAB]���� ��ø ��� ����";

        HorrorManager.Instance.Active_InstructionUI(UIInstruction.ACTIVETYPE.TYPE_FADE, UIInstruction.ACTIVETYPE.TYPE_FADE, activeTimes, texts); // ���� ���
    }

    public void Type_Weapon()
    {
        // ������ �߰�
        HorrorManager.Instance.Player.WeaponManagement.Add_Weapon(m_itemInfo);

        // ���� ���
        float[] activeTimes = new float[1];
        string[] texts = new string[1];
        switch (m_itemInfo.m_itemType)
        {
            case NoteItem.ITEMTYPE.TYPE_PIPE:
                activeTimes[0] = 1f;
                texts[0] = "������ ������������ �����.\n[CTRL]���� ���� ��� ����";
                break;
            case NoteItem.ITEMTYPE.TYPE_GUN:
                activeTimes[0] = 1f;
                texts[0] = "[Ctrl]�� ���ü ����";
                break;
            case NoteItem.ITEMTYPE.TYPE_FLASHLIGHT:
                activeTimes[0] = 1f;
                texts[0] = "[Ctrl]�� ���ü ����";
                break;
        }
        HorrorManager.Instance.Active_InstructionUI(UIInstruction.ACTIVETYPE.TYPE_FADE, UIInstruction.ACTIVETYPE.TYPE_FADE, activeTimes, texts);
    }

    public void Type_Expenitem()
    {
        // �Ҹ�ǰ ������ �߰�
        Note playerNote = HorrorManager.Instance.Player.Note;
        if (playerNote == null)
            return;
        playerNote.Add_Item(m_itemInfo);
    }

    public void Type_Clue()
    {
        // �ܼ� �߰�
        Note playerNote = HorrorManager.Instance.Player.Note;
        if (playerNote == null)
            return;
        playerNote.Add_Clue(m_itemInfo);
    }
}
