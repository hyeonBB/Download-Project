using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class Interaction_Research_Item : Interaction
    {
        [SerializeField] private NoteItem m_noteItem;
        public NoteItem NoteItem { set => m_noteItem = value; }
        
        private void Start()
        {
            GameObject gameObject = HorrorManager.Instance.Create_WorldHintUI(UIWorldHint.HINTTYPE.HT_RESEARCH, transform.GetChild(0), m_uiOffset);
            m_interactionUI = gameObject.GetComponent<UIWorldHint>();
        }

        private void Update()
        {
            //Update_InteractionUI();
        }

        public override void Click_Interaction()
        {
            if (m_interactionUI.gameObject.activeSelf == false || m_interact == true)
                return;

            UIPopup.TYPE type = UIPopup.TYPE.T_END;
            if (m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_END) // ����Ʈ ���� ������ (��������/ �ΰ���)
                type = UIPopup.TYPE.T_QUESTITEM;
            else // ��Ʈ, ���, �Ҹ�ǰ ������, �ܼ� (��������)
            {
                if (m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_NOTE) // ��Ʈ
                    type = UIPopup.TYPE.T_NOTE;
                else if (m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_PIPE || m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_GUN || m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_FLASHLIGHT) // ���
                    type = UIPopup.TYPE.T_WEAPON;
                else
                {
                    Note playerNote = HorrorManager.Instance.Player.Note; // ��Ʈ ���� ���� �˻�
                    if (playerNote == null)
                        return;

                    if (m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_BULLET || m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_DRUG || m_noteItem.m_itemType == NoteItem.ITEMTYPE.TYPE_1KEY) // �Ҹ�ǰ ������
                        type = UIPopup.TYPE.T_EXPENITEM;
                    else // �ܼ�
                        type = UIPopup.TYPE.T_CLUE;
                }
            }

            HorrorManager.Instance.Active_Popup(type, m_noteItem);
            Destroy_Interaction();
        }
    }
}
