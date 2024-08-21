using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Horror
{
    public class NoteSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField] private NoteItem.NOTETYPE m_slotType = NoteItem.NOTETYPE.TYPE_END;

        private Note m_note = null;
        private NoteItem m_item = null;
        private UINoteIcon m_uIItem = null;

        private bool m_drag = false;

        public NoteItem.NOTETYPE SlotType { get => m_slotType; }
        public NoteItem Item { get => m_item; set => m_item = value; }

        public void Initialize_Slot(Note note)
        {
            m_note = note;

            GameObject gameObject = Instantiate(Resources.Load<GameObject>("5. Prefab/3. Horror/UI/UI_NoteIcon"), transform);
            m_uIItem = gameObject.GetComponent<UINoteIcon>();
            m_uIItem.gameObject.SetActive(false);
        }

        public void Add_Item(NoteItem noteItem, bool reset)
        {
            if (m_item == null || reset)
                m_item = noteItem;
            else
            {
                // ���� ����
                if (m_item.m_itemType == noteItem.m_itemType)
                    m_item.m_count += noteItem.m_count;
            }

            if (noteItem.m_count <= 0) // �ʱ�ȭ
            {
                Reset_Slot();
                return;
            }

            m_uIItem.Initialize_Icon(m_item);
        }

        public void Use_Item()
        {
        }

        public void Reset_Slot()
        {
            m_uIItem.gameObject.SetActive(false);
            m_item = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // ����â Ȱ��ȭ

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (m_slotType == NoteItem.NOTETYPE.TYPE_ITEM)
                m_drag = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (m_drag == false)
                return;

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, eventData.position, eventData.pressEventCamera, out localPoint);
            transform.GetChild(0).localPosition = localPoint;
        }

        public void OnEndDrag(PointerEventData eventData) // �巡�� ������ ���� �� ȣ��
        {
            if (m_drag == false)
                return;

            m_drag = false;
            transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);

            GameObject draggedObject = eventData.pointerEnter; // ���� ���� UI�� �����ϴ���
            if (draggedObject == null)
            {
                Create_Item();
                return;
            }

            NoteSlot slot = draggedObject.GetComponent<NoteSlot>();
            if (slot == null) // ���� ���� ��������
                return;

            if (slot == this || slot.SlotType != NoteItem.NOTETYPE.TYPE_ITEM || slot.Item != Item)
                return;

            Reset_Slot(); // �ʱ�ȭ
            m_note.Sort_Item(); // ����
        }

        public void OnDrop(PointerEventData eventData) // OnEndDrag ���� ���� ȣ��/ �������� ���� ȣ��
        {
            if (m_slotType != NoteItem.NOTETYPE.TYPE_ITEM || m_item != null)
                return;

            GameObject draggedObject = eventData.pointerDrag; // �巡�� �ߴ� ������Ʈ
            if (draggedObject == null)
                return;
            NoteSlot item = draggedObject.GetComponent<NoteSlot>();
            if (item == null)
                return;

            Add_Item(item.Item, true);
        }

        private void Create_Item()
        {
            if (m_note.BaseCamera == null) // Output �ؽ�ó�� ����� ī�޶�� ���x
                return;

            // ���� ������ ����
            GameObject worldObject = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/Item/Research_Item");

#region  ��ġ ����
            Ray ray = m_note.BaseCamera.ScreenPointToRay(Input.mousePosition);

            // �ش� �� �ٴ���ġ �̵�
            RaycastHit wallHit;
            RaycastHit groundHit;
            if (Physics.Raycast(ray, out wallHit, Mathf.Infinity, LayerMask.GetMask("Wall")))
            {
                if (Physics.Raycast(wallHit.point, Vector3.down, out groundHit, Mathf.Infinity, LayerMask.GetMask("Ground")))
                    worldObject.transform.position = groundHit.point;
                else // !
                    worldObject.transform.position = HorrorManager.Instance.Player.transform.position;
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
                    worldObject.transform.position = hit.point;
                else
                { // �ٴ��� �ƴ� ��� Ex. ��� ���� ��

                    float dist = Random.Range(2f, 3f);
                    RaycastHit playerHit = GameManager.Instance.Start_Raycast(HorrorManager.Instance.Player.transform.position, HorrorManager.Instance.Player.transform.forward, dist, LayerMask.GetMask("Wall"));
                    if (playerHit.collider == null) // ���� �ƴ� ��� �ش� �������� �̵� ����
                        worldObject.transform.position = HorrorManager.Instance.Player.transform.position + (HorrorManager.Instance.Player.transform.forward * dist);
                    else // �� ������ �̵�
                    {
                        RaycastHit wallOutHit = GameManager.Instance.Start_Raycast(HorrorManager.Instance.Player.transform.position, HorrorManager.Instance.Player.transform.forward, Mathf.Infinity, LayerMask.GetMask("Wall"));
                        if (wallOutHit.collider != null)
                        {
                            worldObject.transform.position = wallOutHit.point;
                            RaycastHit groundOutHit;
                            if (Physics.Raycast(wallOutHit.point, Vector3.down, out groundOutHit, Mathf.Infinity, LayerMask.GetMask("Ground")))
                                worldObject.transform.position = groundOutHit.point;
                            else
                                worldObject.transform.position = wallOutHit.point;//HorrorManager.Instance.Player.transform.position;
                        }
                        else
                            worldObject.transform.position = HorrorManager.Instance.Player.transform.position;
                    }
                }
            }
#endregion

            Interaction_Research_Item interaction = worldObject.GetComponent<Interaction_Research_Item>();
            if (interaction == null)
                return;
            interaction.NoteItem = Item;

            Reset_Slot(); // �ʱ�ȭ
            m_note.Sort_Item(); // ����
        }
    }
}

