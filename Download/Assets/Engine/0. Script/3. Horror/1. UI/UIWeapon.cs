using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Horror;

public class UIWeapon : MonoBehaviour
{
    public enum POSITION { PT_FRONT, PT_MIDDLE, PT_BACK, PT_END };
    private enum SPRITNAME { ST_GUN_ON, ST_GUN_OFF, ST_LANTERN_ON, ST_LANTERN_OFF, ST_PIPE_ON, ST_PIPE_OFF, ST_END };

    [SerializeField] private Image    m_imageIcon;

    [SerializeField] private Image    m_textIcon;
    [SerializeField] private TMP_Text m_textTxt;

    private WeaponId m_weaponId;
    private Vector2[] m_position;
    private Dictionary<string, Sprite> m_WeaponSpr = new Dictionary<string, Sprite>();

    public void Initialize_UI(WeaponId weaponId, WeaponInfo weaponInfo)
    {
        m_WeaponSpr.Add("Gun_OFF", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_WeaponSlot/UI_horror_WeaponSlot_Gun_OFF"));
        m_WeaponSpr.Add("Gun_ON", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_WeaponSlot/UI_horror_WeaponSlot_Gun_ON"));
        m_WeaponSpr.Add("Lantern_OFF", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_WeaponSlot/UI_horror_WeaponSlot_Lantern_OFF"));
        m_WeaponSpr.Add("Lantern_ON", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_WeaponSlot/UI_horror_WeaponSlot_Lantern_ON"));
        m_WeaponSpr.Add("Pipe_OFF", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_WeaponSlot/UI_horror_WeaponSlot_Pipe_OFF"));
        m_WeaponSpr.Add("Pipe_ON", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_WeaponSlot/UI_horror_WeaponSlot_Pipe_ON"));

        m_weaponId = weaponId;

        m_position = new Vector2[(int)POSITION.PT_END];
        m_position[(int)POSITION.PT_FRONT]  = new Vector2(499f, -409f);
        m_position[(int)POSITION.PT_MIDDLE] = new Vector2(512f, -417f);
        m_position[(int)POSITION.PT_BACK]   = new Vector2(525f, -425f);

        Update_Image(true);
        Update_Info(weaponInfo);
    }

    private void Update_Image(bool active)
    {
        switch (m_weaponId)
        {
            case WeaponId.WP_MELEE:
                // ���� ������ ����(�̹���)
                m_imageIcon.sprite = active ? m_WeaponSpr["Pipe_ON"] : m_WeaponSpr["Pipe_OFF"]; 
                // ���Ѵ� ǥ��(�̹���)
                m_textTxt.gameObject.SetActive(false);
                m_textIcon.gameObject.SetActive(active);
                break;

            case WeaponId.WP_FLASHLIGHT:
                // ���� ������ ����(�̹���)
                m_imageIcon.sprite = active ? m_WeaponSpr["Lantern_ON"] : m_WeaponSpr["Lantern_OFF"];
                // ���Ѵ� ǥ��(�̹���)
                m_textTxt.gameObject.SetActive(false);
                m_textIcon.gameObject.SetActive(active);
                break;

            case WeaponId.WP_GUN:
                // ���� ������ ����(�̹���)
                m_imageIcon.sprite = active ? m_WeaponSpr["Gun_ON"] : m_WeaponSpr["Gun_OFF"];
                // �Ѿ� ����/ �ִ� ����(��Ʈ)
                m_textIcon.gameObject.SetActive(false);
                m_textTxt.gameObject.SetActive(active);
                break;
        }
    }

    public void Update_UI(POSITION position, bool active)
    {
        // ���� ����
        int index = 0;
        switch(position)
        {
            case POSITION.PT_FRONT:
                index = (int)POSITION.PT_BACK;
                break;
            case POSITION.PT_MIDDLE:
                index = (int)POSITION.PT_MIDDLE;
                break;
            case POSITION.PT_BACK:
                index = (int)POSITION.PT_FRONT;
                break;
        }
        transform.SetSiblingIndex(index);

        // ��ġ ������Ʈ
        GetComponent<RectTransform>().anchoredPosition = m_position[(int)position];

        // ��Ȱ��ȭ�Ͻ� ���� ����
        if(active == false)
            Update_Image(false);
        else
            Update_Image(true);
    }

    public void Update_Info(WeaponInfo weaponInfo)
    {
        switch (m_weaponId)
        {
            case WeaponId.WP_GUN:
                Weapon_Gun.GunInfo gunInfo = (Weapon_Gun.GunInfo)weaponInfo;
                m_textTxt.text = gunInfo.m_bulletCount.ToString() + "/ " + gunInfo.m_bulletMax.ToString();
                break;
        }
    }
}
