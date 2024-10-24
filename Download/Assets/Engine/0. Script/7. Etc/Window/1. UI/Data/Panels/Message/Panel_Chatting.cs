using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Panel_Chatting : Panel_Popup
{
    private Transform m_chattingTransform;

    public Transform ChattingTransform => m_chattingTransform;

    public Panel_Chatting() : base()
    {
        m_fileType = WindowManager.FILETYPE.TYPE_CHATTING;
    }

    protected override void Active_Event(bool active)
    {
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Panels/Panel_Chatting", canvas.GetChild(3));
        //m_object.SetActive(m_select);
        m_select = false;
        m_object.SetActive(false);

        #region �⺻ ����
        m_chattingTransform = m_object.transform.GetChild(3).GetChild(0).GetChild(0);
        m_object.transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = GameManager.Ins.PlayerName; // �÷��̾� �̸� ����
        #endregion
    }

    public override void Update_Data()
    {

    }

    public override void Unload_Scene()
    {

    }

    public void Set_ChattingsData(List<ChattingData> chattings) // â ���� �� ���� ����
    {
        // Interval ���� �� ����
        int childCount = m_chattingTransform.childCount;
        for (int i = 0; i < childCount - 1; i++)
            GameManager.Ins.Resource.Destroy(m_chattingTransform.GetChild(i).gameObject);

        // ������ �ε�
        GameObject Self  = GameManager.Ins.Resource.Load<GameObject>("5. Prefab/0. Window/UI/ChatingApp/ChatingApp_Self");
        GameObject Other = GameManager.Ins.Resource.Load<GameObject>("5. Prefab/0. Window/UI/ChatingApp/ChatingApp_OtherParty");

        // ä�� ����
        int count = chattings[0].chattings.Count;
        for (int i = 0; i < count; ++i)
        {
            GameObject obj;
            if(chattings[0].chattings[i].type == ChattingData.COMMUNICANTSTYPE.CT_SENDER)
                obj = GameManager.Ins.Resource.Create(Other, m_chattingTransform);
            else
                obj = GameManager.Ins.Resource.Create(Self, m_chattingTransform);
            obj.transform.SetSiblingIndex(m_chattingTransform.childCount - 2);
            obj.GetComponent<ChatBox>().Set_ChattingsData(chattings[0].senderName, chattings[0].chattings[i]);
        }
    }
}
