using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Door : Interaction
{
    public enum OPENTYPE { OT_BASICONE, OT_BASICTWO, OT_ANIMATION, OT_END };
    public enum EVENTTYPE { ET_NONE, ET_CLEAR, ET_EVENT, ET_END };

    [SerializeField] private int m_doorIndex;

    [SerializeField] private bool m_isOpen;

    [SerializeField] private OPENTYPE  m_openType;
    [SerializeField] private EVENTTYPE m_eventType;

    [SerializeField] private Vector3[] m_openOffset;
    [SerializeField] private Vector3[] m_closeOffset;
    [SerializeField] private float m_openDuration = 2f;
    [SerializeField] private float m_closeDuration = 2f;

    //[SerializeField] private Portal m_portal;

    public int DoorIndex => m_doorIndex;

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
            case EVENTTYPE.ET_NONE:
                Move_Door();
                break;

            case EVENTTYPE.ET_CLEAR:
                Check_Clear();
                break;

            case EVENTTYPE.ET_EVENT:
                Move_Door();
                Check_Event();
                break;
        }
    }

    private void Check_Clear()
    {
        // �ش� ������ Ư�� ���� ���� �� ������
        // �ƴ� �� ���� ���
        float[] activeTimes = new float[1];
        string[] texts = new string[1];

        LevelController levelController = HorrorManager.Instance.LevelController.Get_CurrentLevel<Horror_Base>().Levels;
        if(levelController == null) // �� �������� Ŭ���� ���� �Ǻ�
            m_interact = HorrorManager.Instance.LevelController.Get_CurrentLevel<Horror_Base>().Check_Clear(this, ref activeTimes, ref texts);
        else                        // ���� �������� Ŭ���� ���� �Ǻ�
            m_interact = levelController.Get_CurrentLevel<Horror_Base>().Check_Clear(this, ref activeTimes, ref texts);

        if (m_interact == true)
            Move_Door();
        else
            HorrorManager.Instance.Active_InstructionUI(UIInstruction.ACTIVETYPE.TYPE_BASIC, UIInstruction.ACTIVETYPE.TYPE_FADE, activeTimes, texts);
    }

    private void Check_Event()
    {
        // �� ����
        // ������ �̺�Ʈ �߰� �߻� (���� ���� ��)

        Debug.Log("�� ���� �̺�Ʈ �߻� �Ǻ�");
    }

    public void Move_Door(bool delete = true)
    {
        if(delete == true)
            Destroy(m_interactionUI.gameObject);

        switch(m_openType)
        {
            case OPENTYPE.OT_BASICONE:
                StartCoroutine(Move_OneMove());
                break;
            case OPENTYPE.OT_BASICTWO:
                StartCoroutine(Move_TwoMove());
                break;
            case OPENTYPE.OT_ANIMATION:
                Open_Animation();
                break;
        }
    }

    private IEnumerator Move_OneMove()
    {
        float duration;

        Quaternion startRotation = transform.rotation;

        Quaternion endRotation;
        if(m_isOpen == false) // ��������.
        {
            duration = m_openDuration;
            endRotation = startRotation * Quaternion.Euler(m_openOffset[0].x, m_openOffset[0].y, m_openOffset[0].z);
        }
        else
        {
            duration = m_closeDuration;
            endRotation = startRotation * Quaternion.Euler(m_closeOffset[0].x, m_closeOffset[0].y, m_closeOffset[0].z);
        }

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.rotation = endRotation;

        if (m_isOpen == true)
        {
            m_isOpen = false;
            m_possible = !m_possible;
        }

        yield break;
    }

    private IEnumerator Move_TwoMove()
    {
        float duration;

        Transform leftDoor = transform.GetChild(1);
        Transform rightDoor = transform.GetChild(2);

        Quaternion startRotation_1 = leftDoor.rotation;
        Quaternion endRotation_1; 
        if(m_isOpen == false)
        {
            duration = m_openDuration;
            endRotation_1 = startRotation_1 * Quaternion.Euler(m_openOffset[0].x, m_openOffset[0].y, m_openOffset[0].z);
        }
        else
        {
            duration = m_closeDuration;
            endRotation_1 = startRotation_1 * Quaternion.Euler(m_closeOffset[0].x, m_closeOffset[0].y, m_closeOffset[0].z);
        }

        Quaternion startRotation_2 = rightDoor.rotation;
        Quaternion endRotation_2;
        if (m_isOpen == false)
            endRotation_2 = startRotation_2 * Quaternion.Euler(m_openOffset[1].x, m_openOffset[1].y, m_openOffset[1].z);
        else
            endRotation_2 = startRotation_2 * Quaternion.Euler(m_closeOffset[1].x, m_closeOffset[1].y, m_closeOffset[1].z);

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            leftDoor.rotation = Quaternion.Lerp(startRotation_1, endRotation_1, elapsedTime / duration);
            rightDoor.rotation = Quaternion.Lerp(startRotation_2, endRotation_2, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        leftDoor.rotation = endRotation_1;
        rightDoor.rotation = endRotation_2;

        if (m_isOpen == true)
        {
            m_isOpen = false;
            m_possible = !m_possible;
        }

        yield break;
    }

    private void Open_Animation()
    {
        // Temp
        Destroy(gameObject);

        // ������ �� �ִϸ��̼� ���
        //
    }
}
