using Horror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopup : MonoBehaviour
{
    public enum TYPE { T_NOTE, T_WEAPON, T_QUESTITEM, T_EXPENITEM, T_CLUE, T_END };
    public enum EVENT { E_NONE, E_FIRSTBULLET, E_END };

    private TYPE      m_type = TYPE.T_END;
    private NoteItem  m_itemInfo;

    private bool      m_closeText = false;
    private float[]   m_activeTimes;
    private string[]  m_texts;

    private bool  m_closeEvent = false;
    private EVENT m_eventType  = EVENT.E_END;

    public void Initialize_UI(TYPE type, NoteItem itemInfo, bool closeText, float[] activeTimes, string[] texts, bool closeEvent, EVENT eventType)
    {
        m_type      = type;
        m_itemInfo  = itemInfo;

        m_closeText   = closeText;
        m_activeTimes = activeTimes;
        m_texts       = texts;

        m_closeEvent = closeEvent;
        m_eventType  = eventType;

        GameManager.Ins.Set_Pause(true); // ���� �Ͻ�����
        if (m_type == TYPE.T_QUESTITEM) // ����Ʈ ���� ������ (��������/ �ΰ���)
        {
            transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200f, -200f);
            transform.GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(200f, -200f);
            transform.GetChild(0).GetChild(1).gameObject.SetActive(true); // �ΰ��� ��ư
        }
        else // ��Ʈ, ���, �Ҹ�ǰ ������, �ܼ� (��������)
        {
            transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -200f);
            transform.GetChild(0).GetChild(1).gameObject.SetActive(false); // �ΰ��� ��ư
        }
        transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = GameManager.Ins.Horror.NoteElementIcon[m_itemInfo.m_imageName + "_1"];

        gameObject.SetActive(true);
    }

    public void Button_Acquire()
    {
        GameManager.Ins.Sound.Play_ManagerAudioSource("Horror_GetItem", false, 1f);

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

        GameManager.Ins.Set_Pause(false); // �Ͻ����� ����
        if(m_closeText == true)
        {
            m_closeText = false;
            GameManager.Ins.Horror.Active_InstructionUI(UIInstruction.ACTIVETYPE.TYPE_FADE, UIInstruction.ACTIVETYPE.TYPE_FADE, m_activeTimes, m_texts);
        }
        if(m_closeEvent == true)
        {
            m_closeEvent = false;
            switch(m_eventType)
            {
                case EVENT.E_FIRSTBULLET:
                    GameManager.Ins.StartCoroutine(Event_Bullet());
                    break;
            }
        }
        gameObject.SetActive(false);
    }

    public void Button_Leave()
    {
        GameManager.Ins.Set_Pause(false); // �Ͻ����� ����
        gameObject.SetActive(false);
    }


    public void Type_QuestItem()
    {
        // ����Ʈ ���տ� ������ �߰�
        Note playerNote = GameManager.Ins.Horror.Player.Note;
        if (playerNote == null)
            return;
        playerNote.Add_Item(m_itemInfo);
    }

    public void Type_Note()
    {
        GameManager.Ins.Horror.Player.Acquire_Note(); // ��Ʈ �߰�

        float[] activeTimes = new float[2];
        string[] texts = new string[2];
        activeTimes[0] = 2f;
        texts[0] = "���ᰡ ����ϴ� ��ø�� �� �ϴ�...\n������ ���캸��.";
        activeTimes[1] = 3f;
        texts[1] = "[TAB]���� ��ø ��� ����";

        GameManager.Ins.Horror.Active_InstructionUI(UIInstruction.ACTIVETYPE.TYPE_FADE, UIInstruction.ACTIVETYPE.TYPE_FADE, activeTimes, texts); // ���� ���
    }

    public void Type_Weapon()
    {
        // ������ �߰�
        GameManager.Ins.Horror.Player.WeaponManagement.Add_Weapon(m_itemInfo);

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
        GameManager.Ins.Horror.Active_InstructionUI(UIInstruction.ACTIVETYPE.TYPE_FADE, UIInstruction.ACTIVETYPE.TYPE_FADE, activeTimes, texts);
    }

    public void Type_Expenitem()
    {
        // �Ҹ�ǰ ������ �߰�
        Note playerNote = GameManager.Ins.Horror.Player.Note;
        if (playerNote == null)
            return;
        playerNote.Add_Item(m_itemInfo);
    }

    public void Type_Clue()
    {
        // �ܼ� �߰�
        Note playerNote = GameManager.Ins.Horror.Player.Note;
        if (playerNote == null)
            return;
        playerNote.Add_Clue(m_itemInfo);
    }


    public IEnumerator Event_Bullet()
    {
        Horror_Base level = GameManager.Ins.Horror.LevelController.Get_CurrentLevel<Horror_Base>();
        HorrorLight light = level.Light.transform.GetChild(0).GetComponent<HorrorLight>();

        float time = 0f;
        while (true)
        {
            if(GameManager.Ins.IsGame == true)
            {
                time += Time.deltaTime;
                if(time >= 0.5f) // �Ѿ� ȹ��â�� �ݰ� 1�� ��...
                {
                    // �ſ��� ������ (�������� ����Ʈ)
                    //

                    // ���� �۾��� ��Ÿ����.���� ����: ��DO YOU RECOGNIZE ME�� (������ �����ſ���� �׳� �ſ� ����)
                    light.Light.enabled = false;

                    GameObject doyou = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/Effect/BloodPont/DoYou/DoYou", level.Stage.transform);
                    doyou.transform.position = new Vector3(-13.29f, 2.045f, 15.52f);
                    doyou.transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
                    doyou.transform.localScale = new Vector3(0.3377168f, 2.045f, 0.1591384f);
                    break;
                }
            }

            yield return null;
        }

        time = 0f;
        while (true)
        {
            if (GameManager.Ins.IsGame == true)
            {
                time += Time.deltaTime;
                if (time >= 0.2f)
                {
                    light.Light.enabled = true;
                    light.Start_Blink(true, 0.4f, 0.8f, true, 3f); // ȭ��� ���� �����Ÿ���.
                    break;
                }
            }

            yield return null;
        }

        yield break;
    }
}
