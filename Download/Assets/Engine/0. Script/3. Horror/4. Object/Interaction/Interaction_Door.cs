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
        GameObject gameObject = HorrorManager.Instance.Create_WorldHintUI(UIWorldHint.HINTTYPE.HT_OPENDOOR, transform.GetChild(0), m_uiOffset);
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
            m_interact = HorrorManager.Instance.LevelController.Get_CurrentLevel<Horror_Base>().Check_Clear(ref text);
        else                        // ���� �������� Ŭ���� ���� �Ǻ�
            m_interact = levelController.Get_CurrentLevel<Horror_Base>().Check_Clear(ref text);

        if (m_interact == true)
            Open_Door();
        else
            Print_Text(text);
    }

    private void Check_Event()
    {
        // �� ����
        // ������ �̺�Ʈ �߰� �߻�

        Debug.Log("�� ���� �̺�Ʈ �߻� �Ǻ�");
    }

    private void Open_Door()
    {
        Destroy(m_interactionUI.gameObject);
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

    private void Print_Text(string text)
    {
        // < �� ��ũ��Ʈ�� 1.5�ʵ��� �����ȴ�. (���̵���X, ���̵� �ƿ� O)
        // �ش� ��ũ��Ʈ�� ���� ������� �ʾҴٸ� �ٽ� ��Ŭ���� �ص� �߰��� ��ũ��Ʈ�� ���� �ʴ´�.
        GameObject ui = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_Popup", GameObject.Find("Canvas").transform.GetChild(2));
        if (ui == null)
            return;
        UIPopup.Expendables info = new UIPopup.Expendables();
        info.text = text;
        ui.GetComponent<UIPopup>().Initialize_UI(UIPopup.TYPE.T_EXPENDABLES, info);
    }
}
