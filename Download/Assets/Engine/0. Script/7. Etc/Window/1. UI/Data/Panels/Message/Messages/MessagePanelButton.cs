using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagePanelButton : MonoBehaviour
{
    public void Button_Chatting() // ä��
    {
        GameManager.Ins.Window.MESSAGE.Change_Page(Panel_Message.TYPE.TYPE_CHATT);
    }

    public void Button_Call() // ��ȭ
    {
        GameManager.Ins.Window.MESSAGE.Change_Page(Panel_Message.TYPE.TYPE_CALL);
    }

    public void Button_Contact() // ����ó
    {
        GameManager.Ins.Window.MESSAGE.Change_Page(Panel_Message.TYPE.TYPE_CONTACT);
    }

    public void Button_Alam() // �˸�
    {
        GameManager.Ins.Window.MESSAGE.Change_Page(Panel_Message.TYPE.TYPE_ALAM);
    }
}
