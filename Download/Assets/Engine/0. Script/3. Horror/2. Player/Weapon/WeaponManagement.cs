using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManagement<T> where T : class
{
    private GameObject m_owner;
    private int m_curWeapon = -1;
    private int m_preWeapon = -1;
    private List<Weapon<T>> m_weapons = new List<Weapon<T>>();
    private GameObject m_uiParent;
    private List<UIWeapon> m_uis = new List<UIWeapon>();

    public GameObject Owner { get { return m_owner; } }
    public int CurWeapon { get { return m_curWeapon; } }
    public int PreWeapon { get { return m_preWeapon; } }
    public List<Weapon<T>> Weapons { get { return m_weapons; } }

    public WeaponManagement(GameObject owner)
    {
        m_owner = owner;

        m_uiParent = new GameObject("WeaponUIParent");
        m_uiParent.transform.SetParent(GameObject.Find("Canvas").transform.Find("Panel_Basic"), false);
    }

    public void Add_Weapon(Weapon<T> weapons)
    {
        UIWeapon ui = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_Weapon", m_uiParent.transform).GetComponent<UIWeapon>();
        if (ui == null)
            return; 

        weapons.Initialize_Weapon(this, ui);
        m_weapons.Add(weapons);

        ui.Initialize_UI(weapons.ItemInfo.m_itemType, weapons.ItemInfo.m_itemInfo);
        m_uis.Add(ui);


        // ���� �����ϰ� �ִ� ���Ⱑ ���ٸ� �ڵ� ����
        if (m_curWeapon == -1)
            Change_Weapon(0);
        else
            Update_UIWeapons();

        // ��Ʈ�� �ִٸ� ���� ���� �߰�
        if(HorrorManager.Instance.Player.Note != null)
            HorrorManager.Instance.Player.Note.Add_Weapon(weapons.ItemInfo);
    }

    public void Update_Weapon()
    {
        if (m_curWeapon == -1)
            return;

        m_weapons[(int)m_curWeapon].Update_Weapon();
    }

    public void Change_Weapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= m_weapons.Count)
            return;

        if (m_curWeapon >= 0)
            m_weapons[(int)m_curWeapon].Exit_Weapon();

        m_preWeapon = m_curWeapon;
        m_curWeapon = weaponIndex;

        m_weapons[(int)m_curWeapon].Enter_Weapon();

        // UI ��ġ ������Ʈ
        Update_UIWeapons();
    }

    public void Next_Weapon(int value)
    {
        int index = m_curWeapon + value;
        if (index >= m_weapons.Count)
            index = 0;
        else if (index < 0)
            index = m_weapons.Count - 1;

        Change_Weapon(index);
    }

    public void Attack_Weapon()
    {
        if (m_curWeapon == -1 || m_curWeapon >= m_weapons.Count)
            return;

        m_weapons[(int)m_curWeapon].Attack_Weapon();
    }

    public int Get_WeaponIndex(NoteItem.ITEMTYPE weaponId)
    {
        for(int i = 0; i < m_weapons.Count; ++i)
        {
            if(m_weapons[i].ItemInfo.m_itemType == weaponId)
            {
                return i;
            }
        }

        return -1;
    }

    public void Update_WeaponUI(NoteItem.ITEMTYPE weaponId)
    {
        for (int i = 0; i < m_weapons.Count; ++i)
        {
            if (m_weapons[i].ItemInfo.m_itemType == weaponId)
                m_weapons[i].Update_WeaponUI();
        }
    }

    public void OnDrawGizmos()
    {
        m_weapons[(int)m_curWeapon].OnDrawGizmos();
    }

    private void Update_UIWeapons()
    {
        if(m_weapons.Count == 1)
        {
            // ������ 1����� ���� ��
            m_uis[m_curWeapon].Update_UI(UIWeapon.POSITION.PT_BACK, true);
        }
        else if (m_weapons.Count == 2)
        {
            // ������ 2����� �߰�
            m_uis[m_curWeapon].Update_UI(UIWeapon.POSITION.PT_MIDDLE, true);

            // �ٸ� �Ѱ��� �ǵ�
            int NextIndex = (m_curWeapon == 0) ? 1 : 0;
            m_uis[NextIndex].Update_UI(UIWeapon.POSITION.PT_BACK, false);
        }
        else if (m_weapons.Count == 3)
        {
            // ������ 3����� ����
            m_uis[m_curWeapon].Update_UI(UIWeapon.POSITION.PT_FRONT, true);

            int NextIndex = m_curWeapon + 1;
            if (NextIndex >= m_weapons.Count) // ������ �ε����� ó������
                NextIndex = 0;
            m_uis[NextIndex].Update_UI(UIWeapon.POSITION.PT_MIDDLE, false);

            NextIndex += 1;
            if (NextIndex >= m_weapons.Count) // ������ �ε����� ó������
                NextIndex = 0;
            m_uis[NextIndex].Update_UI(UIWeapon.POSITION.PT_BACK, false);
        }
    }
}