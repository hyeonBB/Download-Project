using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManagement<T> where T : class
{
    private GameObject m_owner;
    private int m_curWeapon = -1;
    private int m_preWeapon = -1;
    private List<Weapon<T>> m_weapons = new List<Weapon<T>>();
    private UIWeapon m_uIWeapon = null;

    public GameObject Owner { get { return m_owner; } }
    public int CurWeapon { get { return m_curWeapon; } }
    public int PreWeapon { get { return m_preWeapon; } }
    public List<Weapon<T>> Weapons { get { return m_weapons; } }

    public WeaponManagement(GameObject owner)
    {
        m_owner = owner;
    }

    public void Initialize_WeaponManagement()
    {
        //m_uiParent = new GameObject("WeaponUIParent");
        //m_uiParent.transform.SetParent(GameObject.Find("Canvas").transform.Find("Panel_Play"), false);

        m_uIWeapon = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/UI/UI_Weapon", GameObject.Find("Canvas").transform.Find("Panel_Play")).GetComponent<UIWeapon>();
        if (m_uIWeapon == null)
            return;
        m_uIWeapon.Initialize_UI();
    }

    public void Add_Weapon(NoteItem noteItem)
    {
        Weapon<T> weapon = null;
        Transform handTransfom = HorrorManager.Instance.Player.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(1);
        switch (noteItem.m_itemType)
        {
            case NoteItem.ITEMTYPE.TYPE_PIPE:
                weapon = handTransfom.GetChild(2).GetComponent<Weapon<T>>();
                break;
            case NoteItem.ITEMTYPE.TYPE_GUN:
                weapon = handTransfom.GetChild(3).GetComponent<Weapon<T>>();
                break;
            case NoteItem.ITEMTYPE.TYPE_FLASHLIGHT:
                weapon = handTransfom.GetChild(4).GetComponent<Weapon<T>>();
                break;
        }
        weapon.Initialize_Weapon(this, noteItem, m_uIWeapon);
        m_weapons.Add(weapon);

        // ���� �����ϰ� �ִ� ���Ⱑ ���ٸ� �ڵ� ����
        if (m_curWeapon == -1)
        {
            m_owner.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(1).gameObject.SetActive(true); // �� Ȱ��ȭ
            m_uIWeapon.gameObject.SetActive(true);
            Change_Weapon(0);
        }
        else
            Update_UIWeapons();

        // ��Ʈ�� �ִٸ� ���� ���� �߰�
        if(HorrorManager.Instance.Player.Note != null)
            HorrorManager.Instance.Player.Note.Add_Weapon(weapon.ItemInfo);
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
        if (m_weapons.Count <= 1)
            return;

        int index = m_curWeapon + value;
        if (index >= m_weapons.Count)
            index = 0;
        else if (index < 0)
            index = m_weapons.Count - 1;

        Change_Weapon(index);
    }

    public bool Attack_Weapon()
    {
        if (m_curWeapon == -1 || m_curWeapon >= m_weapons.Count)
            return false;

        return m_weapons[m_curWeapon].Attack_Weapon();
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
        if (m_curWeapon == -1)
            return;

        m_uIWeapon.Update_UI(m_weapons.Count, (UIWeapon.POSITION)m_curWeapon, m_weapons[m_curWeapon].ItemInfo.m_itemType, m_weapons[m_curWeapon].ItemInfo.m_itemInfo);
        /*if (m_weapons.Count == 1)
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
        }*/
    }

    public NoteItem Get_CurrentWeaoponType()
    {
        if (m_curWeapon == -1 || m_curWeapon >= m_weapons.Count)
            return null;

        return m_weapons[m_curWeapon].ItemInfo;
    }
}