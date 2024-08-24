using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIKeypad : MonoBehaviour
{
    [SerializeField] private TMP_Text[] m_numberTxt;
    [SerializeField] private Image  m_resultImg;
    [SerializeField] private Sprite m_resultSpr;

    private Interaction_Door m_interactionDoor = null;
    private bool  m_correct = false;
    private int[] m_correctNumber = new int[4];
    private int[] m_inputNumber = new int[4];
    private int m_inputCount = 0;
    private AudioSource m_audioSource;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();

        // ���� ��ȣ
        m_correctNumber[0] = 1;
        m_correctNumber[1] = 2;
        m_correctNumber[2] = 3;
        m_correctNumber[3] = 4;

        // �Է� ��ȣ �ʱ�ȭ
        m_inputNumber[0] = -1;
        m_inputNumber[1] = -1;
        m_inputNumber[2] = -1;
        m_inputNumber[3] = -1;

        gameObject.SetActive(false);
    }

    public void OnEnable_Keypad(Interaction_Door interaction_Door) // Ȱ��ȭ�� ��
    {
        m_interactionDoor = interaction_Door;

        gameObject.SetActive(true);
        HorrorManager.Instance.Set_Pause(true); // ���� �Ͻ�����
    }

    private void OnDisable_Keypad() // ��Ȱ��ȭ�� ��
    {
        gameObject.SetActive(false);
        HorrorManager.Instance.Set_Pause(false); // ���� �Ͻ�����
        Reset_Number();
    }
    
    private void Update()
    {
        if (m_correct == true)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            OnDisable_Keypad();
    }

    private void Reset_Number()
    {
        m_inputCount = 0;
        for (int i = 0; i < m_inputNumber.Length; ++i)
        {
            m_inputNumber[i] = -1;
            m_numberTxt[i].text = "";
        }
    }

    public void Button_Number(int number)
    {
        if (m_inputCount == 4)
            return;

        m_inputNumber[m_inputCount] = number;
        m_numberTxt[m_inputCount].text = m_inputNumber[m_inputCount].ToString();
        m_inputCount++;

        if (m_inputCount == 4)
            Check_Correct();
    }

    public void Button_Backspace()
    {
        // �ֱٿ� ���� ���ں��� �ٽ� ���� �� �ִ�.
        if (m_correct == true || m_inputCount <= 0)
            return;

        m_inputCount--;
        m_inputNumber[m_inputCount] = -1;
        m_numberTxt[m_inputCount].text = "";
    }

    private void Check_Correct()
    {
        bool Correct = true;
        for (int i = 0; i < m_inputNumber.Length; ++i)
        {
            if (m_inputNumber[i] != m_correctNumber[i])
                Correct = false;
        }

        if(Correct == true)
        { // ���� : 0.30�� �ڿ� �ڵ�����, 1��Ī �÷��� ȭ������ ��ȯ�Ǹ� ���� ������.
            m_correct = true;
            m_resultImg.sprite = m_resultSpr; // �ʷϺ� �̹����� ����
            StartCoroutine(Is_Correct());
        }
        else
        { // ���� : ������ ��ȣ�� �Է����� �� Ʋ�ȴٴ� ���� �˷��ִ� ������������ ����.
            // ������ ���
            //m_audioSource.clip = null;
            //m_audioSource.loop = false;
            //m_audioSource.Play();
            Reset_Number();
        }
    }

    private IEnumerator Is_Correct()
    {
        float time = 0;
        while(true)
        {
            time += Time.deltaTime;
            if (time > 1f)
                break;
            yield return null;
        }

        // 1��Ī �÷��� ȭ������ ��ȯ �� ���� ������.
        OnDisable_Keypad();
        m_interactionDoor.Open_Door();
        Destroy(gameObject);

        yield break;
    }
}
