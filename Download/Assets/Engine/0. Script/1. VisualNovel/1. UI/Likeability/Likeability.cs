using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class Likeability : MonoBehaviour
    {
        [SerializeField] private NpcLike[]  m_npcLike;
        [SerializeField] private GameObject m_button;
        [SerializeField] private GameObject m_panel;
        private bool  m_shake = false;
        private float m_time  = 0f;

        private void Start()
        {

        }

        private void Update()
        {
            //if(Input.GetKeyDown(KeyCode.S))
            //{
            //    Shake_Heart();
            //}
        }

        private void LateUpdate()
        {
            // ��Ʈ���� �� �������� ���� ȣ���� ��ư�� â ��ư�� ���� ���������� ������ ȭ�� ������ �������.
            if(m_button != null && m_panel != null && m_shake == true)
            {
                //int count = 0;
                //for (int i = 0; i < m_npcLike.Length; ++i) { if (m_npcLike[i].HeartClear == true) count++; }
                //if (count == m_npcLike.Length) { // }
                m_time += Time.deltaTime;
                if(m_time > 1f)
                {
                    m_shake = false;
                    m_button.GetComponent<ParabolaUI>().Shake_Object();
                    m_panel.GetComponent<ParabolaUI>().Shake_Object();
                }
            }
            else if(m_button == null && m_panel == null)
            {
                GameManager.Ins.Novel.Dialog.GetComponent<Dialog_VN>().CutScene = false;
                Destroy(gameObject);
            }
        }

        public void Shake_Heart()
        {
            m_shake = true;
            // ����Ʈ ���� : �ٸ��� 2����, ���κ��� ��ġ�� �ȵȴ�.
            List<int> availableNumbers = new List<int>();
            for (int i = 0; i <= 6; i++) // 0���� 6���� �� ����
                availableNumbers.Add(i);
            availableNumbers.Shuffle();

            for (int i = 0; i < m_npcLike.Length; ++i) { 
                m_npcLike[i].Shake_Heart(ref availableNumbers); 
            }
        }
    }
}