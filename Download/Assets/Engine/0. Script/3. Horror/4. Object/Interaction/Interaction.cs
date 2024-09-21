using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interaction : MonoBehaviour
{
    [SerializeField] protected float m_dist;
    [SerializeField] protected Vector3 m_uiOffset; // UI ��ġ ������
    protected UIWorldHint m_interactionUI = null;

    [SerializeField] protected bool m_possible;     // UI Ȱ��ȭ ���� ����
    [SerializeField] protected bool m_objectDelete; // ������Ʈ ���� ����
    protected bool  m_interact = false;             // ��ȣ�ۿ� ���� ����

    [SerializeField] protected GameObject m_deleteObject;

    public float Dist => m_dist;
    public UIWorldHint InteractionUI => m_interactionUI;
    public bool Possible => m_possible;

    public abstract void Click_Interaction();

    protected bool No_Click()
    {
        if (m_interactionUI == null || m_interactionUI.gameObject.activeSelf == false || m_interact == true)
            return true;

        return false;
    }

    protected void Destroy_Interaction()
    {
        Destroy(m_interactionUI.gameObject);
        Destroy(gameObject);
    }

    protected void Check_Delete()
    {
        if (m_objectDelete == true)
            Destroy_Interaction();
        else
        {
            Destroy(m_interactionUI.gameObject);
            Destroy(transform.GetChild(0).gameObject);
        }

        if(m_deleteObject != null)
            Destroy(m_deleteObject);
    }
}
