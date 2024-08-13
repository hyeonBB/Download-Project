using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class Weapon_Melee : Weapon<HorrorPlayer>
    {
        private BoxCollider m_attackCollider = null;

        public override void Initialize_Weapon(WeaponManagement<HorrorPlayer> weaponManagement, UIWeapon uIWeapon)
        {
            base.Initialize_Weapon(weaponManagement, uIWeapon);

            m_itemInfo.m_name = "������";
            m_itemInfo.m_count = 1;
            m_itemInfo.m_noteType = NoteItem.NOTETYPE.TYPE_WEAPON;
            m_itemInfo.m_itemType = NoteItem.ITEMTYPE.TYPE_PIPE;
            transform.localPosition = new Vector3(0f, 0.023f, 0f);

            m_damage = 1f;

            m_attackCollider = gameObject.transform.GetChild(1).GetComponent<BoxCollider>();
        }

        public override void Enter_Weapon()
        {
            base.Enter_Weapon();
        }

        public override void Update_Weapon()
        {
            base.Update_Weapon();
        }

        public override void Exit_Weapon()
        {
            base.Exit_Weapon();
        }

        public override void Attack_Weapon()
        {
            Debug.Log("��������");

            // �ڽ� �ݶ��̴��� �߽ɰ� ũ��
            Vector3 center      = m_attackCollider.bounds.center;
            Vector3 halfExtents = m_attackCollider.bounds.extents;

            // ���� ���̾ �ִ� ��� ������Ʈ�� �浹 �˻�
            Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity, 1 << LayerMask.NameToLayer("Monster"));
            foreach (var hitCollider in hitColliders)
            {
                Debug.Log($"�������� {hitCollider.gameObject.transform.parent.parent.parent.gameObject.name}");

                Monster monster = hitCollider.gameObject.transform.parent.parent.parent.GetComponent<Monster>();
                if (monster == null)
                    return;
                monster.Damage_Monster(m_damage);
            }
        }
    }
}

