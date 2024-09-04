using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Horror;

public class UINoteIcon : MonoBehaviour
{
    public enum TYPE { TYPE_WEAPON, TYPE_ITEM, TYPE_CLUE, TYPE_END }

    [SerializeField] private Image m_Iconimage;
    [SerializeField] private TMP_Text m_nameText;
    [SerializeField] private TMP_Text m_countText;

    public void Initialize_Icon(NoteItem noteItem)
    {
        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);

        m_nameText.text = noteItem.m_name;
        m_Iconimage.sprite = HorrorManager.Instance.NoteElementIcon[noteItem.m_imageName + "_1"];
        switch (noteItem.m_noteType)
        {
            case NoteItem.NOTETYPE.TYPE_WEAPON:
                m_countText.gameObject.SetActive(false);
                break;

            case NoteItem.NOTETYPE.TYPE_ITEM:
                if(noteItem.m_count <= 1)
                    m_countText.gameObject.SetActive(false);
                else
                {
                    m_countText.gameObject.SetActive(true);
                    m_countText.text = noteItem.m_count.ToString();
                }

                switch (noteItem.m_itemType)
                {
                    case NoteItem.ITEMTYPE.TYPE_BULLET:
                        HorrorManager.Instance.Player.WeaponManagement.Update_WeaponUI(NoteItem.ITEMTYPE.TYPE_GUN); // ���� ui ������Ʈ
                        break;
                }
                break;

            case NoteItem.NOTETYPE.TYPE_CLUE:
                if (noteItem.m_count <= 1)
                    m_countText.gameObject.SetActive(false);
                else
                {
                    m_countText.gameObject.SetActive(true);
                    m_countText.text = noteItem.m_count.ToString();
                }
                break;
        }


    }
}
