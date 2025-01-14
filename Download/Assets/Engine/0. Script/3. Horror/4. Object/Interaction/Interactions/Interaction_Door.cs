using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Door : Interaction
{
    public enum OPENTYPE { OT_BASICONE, OT_BASICTWO, OT_ANIMATION, OT_END };
    public enum EVENTTYPE { ET_NONE, ET_CLEAR, ET_ACTIVE, ET_END };

    [SerializeField] private int m_doorIndex;

    [SerializeField] private bool m_isOpen;

    [SerializeField] private OPENTYPE  m_openType;
    [SerializeField] private EVENTTYPE m_eventType;

    [SerializeField] private Vector3[] m_openOffset;
    [SerializeField] private Vector3[] m_closeOffset;
    [SerializeField] private float m_openDuration = 2f;
    [SerializeField] private float m_closeDuration = 2f;

    [SerializeField] private GameObject m_activeObject;

    private AudioSource m_audioSource;
    //[SerializeField] private Portal m_portal;

    public int DoorIndex => m_doorIndex;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();

        GameObject gameObject = GameManager.Ins.Horror.Create_WorldHintUI(UIWorldHint.HINTTYPE.HT_OPENDOOR, transform.GetChild(0), m_uiOffset);
        m_interactionUI = gameObject.GetComponent<UIWorldHint>();
    }

    private void Update()
    {
        //Update_InteractionUI();
    }

    public override void Click_Interaction()
    {
        if (No_Click())
            return;

        switch (m_eventType)
        {
            case EVENTTYPE.ET_NONE:
                Move_Door();
                break;

            case EVENTTYPE.ET_CLEAR:
                Check_Clear();
                break;

            case EVENTTYPE.ET_ACTIVE:
                Move_Door();
                Check_Event();
                break;
        }
    }

    private void Check_Clear()
    {
        // 해당 구역의 특정 조건 성립 시 문열림
        // 아닐 시 문구 출력
        float[] activeTimes = new float[1];
        string[] texts = new string[1];

        LevelController levelController = GameManager.Ins.Horror.LevelController.Get_CurrentLevel<Horror_Base>().Levels;
        if(levelController == null) // 본 스테이지 클리어 여부 판별
            m_interact = GameManager.Ins.Horror.LevelController.Get_CurrentLevel<Horror_Base>().Check_Clear(this, ref activeTimes, ref texts);
        else                        // 세부 스테이지 클리어 여부 판별
            m_interact = levelController.Get_CurrentLevel<Horror_Base>().Check_Clear(this, ref activeTimes, ref texts);

        if (m_interact == true)
            Move_Door();
        else
            GameManager.Ins.Horror.Active_InstructionUI(UIInstruction.ACTIVETYPE.TYPE_FADE, UIInstruction.ACTIVETYPE.TYPE_FADE, activeTimes, texts);
    }

    private void Check_Event()
    {
        // 선택적 이벤트 추가 발생 (몬스터 생성 등)
        m_activeObject.SetActive(true);
    }

    public void Move_Door(bool delete = true)
    {
        if(delete == true)
            Destroy(m_interactionUI.gameObject);

        GameManager.Ins.Sound.Play_AudioSource(m_audioSource, "Horror_Open_Door", false, 1f);

        switch (m_openType)
        {
            case OPENTYPE.OT_BASICONE:
                StartCoroutine(Move_OneMove());
                break;
            case OPENTYPE.OT_BASICTWO:
                StartCoroutine(Move_TwoMove());
                break;
            case OPENTYPE.OT_ANIMATION:
                StartCoroutine(Open_Animation());
                break;
        }
    }

    private IEnumerator Move_OneMove()
    {
        float duration;

        Quaternion startRotation = transform.rotation;

        Quaternion endRotation;
        if(m_isOpen == false) // 닫혀있음.
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

    private IEnumerator Open_Animation() // 열리는 문 애니메이션 재생
    {
        Animator[] animators = transform.GetComponentsInChildren<Animator>();
        if (animators != null)
        {
            foreach (Animator animator in animators)
                animator.SetTrigger("IsOpen");

            int count = transform.GetChild(1).childCount;
            bool isPlay = true;
            while (isPlay)
            {
                foreach (Animator animator in animators)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        isPlay = false;
                        break;
                    }
                }

                yield return null;
            }

            // Bounds 수정 (커튼 기준)
            for (int i = 0; i < count; ++i)
            {
                SkinnedMeshRenderer SMR = transform.GetChild(1).GetChild(i).GetChild(1).GetComponent<SkinnedMeshRenderer>();
                if (SMR != null)
                {
                    Bounds bounds = SMR.bounds;
                    bounds.center = new Vector3(bounds.center.x, 0.021f, bounds.center.z);
                    SMR.bounds = bounds;
                }
            }

            transform.GetChild(2).GetChild(0).gameObject.SetActive(false); // 첫 번째 콜라이더 비활성화
            transform.GetChild(2).GetChild(1).gameObject.SetActive(true);  // 두 번째 콜라이더 활성화
            this.enabled = false; // 스크립트 비활성화
        }

        yield break;
    }
}
