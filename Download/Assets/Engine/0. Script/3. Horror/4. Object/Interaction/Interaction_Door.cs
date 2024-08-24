using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Door : Interaction
{
    public enum OPENTYPE { OT_BASICONE, OT_BASICTWO, OT_ANIMATION, OT_END };
    public enum EVENTTYPE { ET_CLEAR, ET_EVENT, ET_END };

    [SerializeField] private OPENTYPE  m_openType;
    [SerializeField] private EVENTTYPE m_eventType;
    [SerializeField] private Vector3 m_openOffset; // y 150
    [SerializeField] private float m_duration = 2f;

    private void Start()
    {
        GameObject gameObject = HorrorManager.Instance.Create_WorldHintUI(UIWorldHint.HINTTYPE.HT_OPENDOOR, transform.GetChild(0), m_uiOffset);
        m_interactionUI = gameObject.GetComponent<UIWorldHint>();
    }

    private void Update()
    {
        //Update_InteractionUI();
    }

    public override void Click_Interaction()
    {
        if (m_interactionUI == null || m_interactionUI.gameObject.activeSelf == false || m_interact == true)
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
        // �ش� ������ Ư�� ���� ���� �� ������
        // �ƴ� �� ���� ���
        string text = "";

        LevelController levelController = HorrorManager.Instance.LevelController.Get_CurrentLevel<Horror_Base>().Levels;
        if(levelController == null) // �� �������� Ŭ���� ���� �Ǻ�
            m_interact = HorrorManager.Instance.LevelController.Get_CurrentLevel<Horror_Base>().Check_Clear(this, ref text);
        else                        // ���� �������� Ŭ���� ���� �Ǻ�
            m_interact = levelController.Get_CurrentLevel<Horror_Base>().Check_Clear(this, ref text);

        if (m_interact == true)
            Open_Door();
        else
        {
            // < �� ��ũ��Ʈ�� 1.5�ʵ��� �����ȴ�. (���̵���X, ���̵� �ƿ� O)
            // �ش� ��ũ��Ʈ�� ���� ������� �ʾҴٸ� �ٽ� ��Ŭ���� �ص� �߰��� ��ũ��Ʈ�� ���� �ʴ´�.
            HorrorManager.Instance.Active_InstructionUI(text);
        }
    }

    private void Check_Event()
    {
        // �� ����
        // ������ �̺�Ʈ �߰� �߻�

        Debug.Log("�� ���� �̺�Ʈ �߻� �Ǻ�");
    }

    public void Open_Door()
    {
        Destroy(m_interactionUI.gameObject);

        switch(m_openType)
        {
            case OPENTYPE.OT_BASICONE:
                StartCoroutine(Open_OneMove());
                break;
            case OPENTYPE.OT_BASICTWO:
                break;
            case OPENTYPE.OT_ANIMATION:
                Open_Animation();
                break;
        }
    }

    private IEnumerator Open_OneMove()
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

    private void Open_Animation()
    {
        // Temp
        Destroy(gameObject);

        // ������ �� �ִϸ��̼� ���
        //
    }
}
