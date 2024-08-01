using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Door : Interaction
{
    public enum EVENTTYPE { ET_CLEAR, ET_EVENT, ET_END };

    [SerializeField] private EVENTTYPE m_eventType;
    [SerializeField] private Vector3 m_openOffset; // y 150
    [SerializeField] private float m_duration = 2f;

    private void Start()
    {
        m_interactionUI = HorrorManager.Instance.Create_WorldHintUI(UIWorldHint.HINTTYPE.HT_OPENDOOR, transform, m_uiOffset);
        m_interactionUI.SetActive(false);
    }

    private void Update()
    {
        Update_InteractionUI();
    }

    public override void Click_Interaction()
    {
        if (m_interactionUI.activeSelf == false)
            return;

        switch (m_eventType)
        {
            case EVENTTYPE.ET_CLEAR:
                Check_Clear();
                break;

            case EVENTTYPE.ET_EVENT:
                Check_Event();
                break;
        }
    }

    private void Check_Clear()
    {
        if (m_interact == true)
            return;

        // �ش� ������ Ư�� ���� ���� �� ������
        // �ƴ� �� ���� ���
        LevelController levelController = HorrorManager.Instance.LevelController.Get_CurrentLevel<Horror_Base>().Levels;
        if(levelController == null) // �� �������� Ŭ���� ���� �Ǻ�
            m_interact = HorrorManager.Instance.LevelController.Get_CurrentLevel<Horror_Base>().Check_Clear();
        else                        // ���� �������� Ŭ���� ���� �Ǻ�
            m_interact = levelController.Get_CurrentLevel<Horror_Base>().Check_Clear();

        if (m_interact == true)
            Open_Door();
        else
            Print_Text();
    }

    private void Check_Event()
    {
        // �� ����
        // ������ �̺�Ʈ �߰� �߻�

        Debug.Log("�� ���� �̺�Ʈ �߻� �Ǻ�");
    }

    private void Open_Door()
    {
        m_interactionUI.SetActive(false);
        StartCoroutine(Open_Move());
    }

    IEnumerator Open_Move()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation   = startRotation * Quaternion.Euler(m_openOffset.x, m_openOffset.y, m_openOffset.z);
        
        float elapsedTime = 0;
        while (elapsedTime < m_duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / m_duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.rotation = endRotation;
    }

    private void Print_Text()
    {
        // < �� ��ũ��Ʈ�� 1.5�ʵ��� �����ȴ�. (���̵���X, ���̵� �ƿ� O)
        // �ش� ��ũ��Ʈ�� ���� ������� �ʾҴٸ� �ٽ� ��Ŭ���� �ص� �߰��� ��ũ��Ʈ�� ���� �ʴ´�.
    }
}
