using Horror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGetItem : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;

    private NoteItem m_itemType;

    public void Initialize_UI(NoteItem noteItem)
    {
        m_itemType = noteItem;
        switch (m_itemType.m_itemType)
        {
            case NoteItem.ITEMTYPE.TYPE_PIPE:
                m_text.text = "������ ȹ��";
                HorrorManager.Instance.Player.WeaponManagement.Add_Weapon(GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/Character/Weapon/Melee", HorrorManager.Instance.Player.transform).GetComponent<Weapon<HorrorPlayer>>());
                break;
            case NoteItem.ITEMTYPE.TYPE_FLASHLIGHT:
                m_text.text = "������ ȹ��";
                HorrorManager.Instance.Player.WeaponManagement.Add_Weapon(GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/Character/Weapon/Flashlight", HorrorManager.Instance.Player.transform).GetComponent<Weapon<HorrorPlayer>>());
                break;
            case NoteItem.ITEMTYPE.TYPE_GUN:
                m_text.text = "�� ȹ��";
                HorrorManager.Instance.Player.WeaponManagement.Add_Weapon(GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/Character/Weapon/Gun", HorrorManager.Instance.Player.transform).GetComponent<Weapon<HorrorPlayer>>());
                break;

            case NoteItem.ITEMTYPE.TYPE_NOTE:
                m_text.text = "������ ��ø ȹ��";
                HorrorManager.Instance.Player.Acquire_Note();
                break;

            default:
                m_text.text = m_itemType.m_name + " ȹ��";
                HorrorManager.Instance.Player.Note.Add_Proviso(m_itemType);
                break;
        }
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            HorrorManager.Instance.Set_Pause(false);

            GameObject ui = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_Popup", GameObject.Find("Canvas").transform.GetChild(2));
            if (ui == null)
                return;

            UIPopup.Expendables info = new UIPopup.Expendables();

            switch (m_itemType.m_itemType)
            {
                case NoteItem.ITEMTYPE.TYPE_PIPE:
                    info.text = "������ ������������ �����.\n[TAB]���� ��� ����";
                    break;
                case NoteItem.ITEMTYPE.TYPE_GUN:
                    info.text = "[Ctrl]�� ���ü ����";
                    break;
                case NoteItem.ITEMTYPE.TYPE_FLASHLIGHT:
                    info.text = "[Ctrl]�� ���ü ����";
                    break;

                case NoteItem.ITEMTYPE.TYPE_NOTE:
                    info.text = "������ �������� ��ø���� �����.\n[ESC]�� ��� ����";
                    break;
            }

            ui.GetComponent<UIPopup>().Initialize_UI(UIPopup.TYPE.T_EXPENDABLES, info, m_itemType);

            Destroy(gameObject);
        }
    }
}