using UnityEngine;
using UnityEngine.EventSystems;

public class Button_Choice : MonoBehaviour, IPointerEnterHandler
{
    public int m_buttonIndex;
    public int ButtonIndex
    {
        get { return m_buttonIndex; }
        set { m_buttonIndex = value; }
    }

    public Dialog m_ownerDialog;
    public Dialog Ownerdialog
    {
        set { m_ownerDialog = value; }
    }

    // Ŀ���� �浹���� ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        m_ownerDialog.Enter_Button(m_buttonIndex);
    }
}
