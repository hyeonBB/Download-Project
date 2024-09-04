using UnityEngine;
using UnityEngine.UI;

public class NoteItemMagnifyingGlass : MonoBehaviour
{
    [SerializeField] private Image m_image;
    private float m_lastClickTime = 0f;
    private float m_doubleClickThreshold = 1f;

    private void Update()
    {
        // ȭ�� �ι� Ŭ���ϸ� ��Ȱ��ȭ
        if (Input.GetMouseButtonDown(0))
        {
            float timeSinceLastClick = Time.time - m_lastClickTime; // ���� �ð��� ������ Ŭ�� �ð� ���� ���
            if (timeSinceLastClick <= m_doubleClickThreshold)       // �� �� Ŭ��
                gameObject.SetActive(false);
            
            m_lastClickTime = Time.time; // ������ Ŭ�� �ð� ������Ʈ
        }
    }

    public void Update_UIInfo(NoteItem item)
    {
        m_image.sprite = HorrorManager.Instance.NoteElementIcon[item.m_imageName + "_3"];
    }
}
