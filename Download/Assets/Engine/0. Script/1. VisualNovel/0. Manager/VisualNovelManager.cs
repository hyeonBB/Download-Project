using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

using VisualNovel;
public class VisualNovelManager : MonoBehaviour
{
    public enum LEVELSTATE { LS_NOVELBEGIN, LS_SHOOTGAME, LS_NOVELEND, LS_CHASEGAME, LS_END }; // �̿��� -> ��� -> �̿��� -> �߰� => ���� ����
    public enum NPCTYPE { OT_WHITE, OT_BLUE, OT_YELLOW, OT_PINK, OT_END };


    private static VisualNovelManager m_instance = null;

    [SerializeField] private LEVELSTATE m_startState = LEVELSTATE.LS_NOVELBEGIN;

    [Header("[ LS_START ]")]
    [SerializeField] GameObject m_StartPanel;

    [Header("[ LS_NOVEL ]")]
    [SerializeField] private GameObject m_dialog;
    [SerializeField] private GameObject m_likeabilityPanel;
    [SerializeField] private NpcLike[]  m_likeabilityHeartPanel;

    private int[] m_npcHeart;
    private LevelController m_levelController = null;

    public static VisualNovelManager Instance => m_instance;
    public int[] NpcHeart
    {
        get => m_npcHeart;
        set => m_npcHeart = value;
    }
    public LevelController LevelController => m_levelController; // �б�
    public GameObject Dialog => m_dialog;
    public GameObject LikeabilityPanel => m_likeabilityPanel;

    #region Resource
    private Dictionary<string, Sprite> m_backgroundSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_standingSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_portraitSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_boxISpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_ellipseSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_arrawSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_choiceButtonSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_heartSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, TMP_FontAsset> m_fontAst = new Dictionary<string, TMP_FontAsset>();

    public Dictionary<string, Sprite> BackgroundSpr { get { return m_backgroundSpr; }}
    public Dictionary<string, Sprite> StandingSpr { get { return m_standingSpr; }}
    public Dictionary<string, Sprite> PortraitSpr { get { return m_portraitSpr; }}
    public Dictionary<string, Sprite> BoxISpr { get { return m_boxISpr; }}
    public Dictionary<string, Sprite> EllipseSpr { get { return m_ellipseSpr; }}
    public Dictionary<string, Sprite> ArrawSpr { get { return m_arrawSpr; } }
    public Dictionary<string, Sprite> ChoiceButtonSpr { get { return m_choiceButtonSpr; }}
    public Dictionary<string, Sprite> HeartSpr { get { return m_heartSpr; } }
    public Dictionary<string, TMP_FontAsset> FontAst { get { return m_fontAst; } }
    #endregion

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;

        Create_NpcHeart();
        Load_Resource();

        m_levelController = gameObject.AddComponent<LevelController>();

        List<Level> levels = new List<Level>
        { 
            gameObject.AddComponent<Novel_Begin>(),
            gameObject.AddComponent<Novel_Shoot>(),
            gameObject.AddComponent<Novel_End>(),
            gameObject.AddComponent<Novel_Chase>()
        };

        gameObject.GetComponent<Novel_Begin>().Initialize_Level(m_levelController);
        gameObject.GetComponent<Novel_Shoot>().Initialize_Level(m_levelController);
        gameObject.GetComponent<Novel_End>().Initialize_Level(m_levelController);
        gameObject.GetComponent<Novel_Chase>().Initialize_Level(m_levelController);

        m_levelController.Initialize_Level(levels);
    }

    private void Start()
    {
        GameManager.Ins.UI.Start_FadeIn(1f, Color.black);
    }

    private void Update()
    {
        m_levelController.Update_Level();
    }

    private void Create_NpcHeart()
    {
        m_npcHeart = new int[(int)NPCTYPE.OT_END];
        for (int i = 0; i < (int)NPCTYPE.OT_END; i++)
            m_npcHeart[i] = 0;
    }

    private void Load_Resource()
    {
        // ��� �̹��� �Ҵ�
        m_backgroundSpr.Add("BackGround_SchoolWay", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_SchoolWay"));
        m_backgroundSpr.Add("BackGround_School", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_School"));
        m_backgroundSpr.Add("BackGround_NightMarket", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_NightMarket"));
        m_backgroundSpr.Add("BackGround_Festival", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_Festival"));
        m_backgroundSpr.Add("BackGround_PlayerHome", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_PlayerHome"));
        m_backgroundSpr.Add("BackGround_PinkHome", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_PinkHome"));
        m_backgroundSpr.Add("BackGround_Cellar", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_Cellar"));
        
        // ���ĵ� �̹��� �Ҵ�
        m_standingSpr.Add("Blue", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/Character/Blue/Sprite3"));
        m_standingSpr.Add("Yellow", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/Character/Yellow/Sprite1"));
        m_standingSpr.Add("Pink", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/Character/Pink/Sprite2"));

        // ������ �̹��� �Ҵ�
        m_portraitSpr.Add("Blue", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/Character/Blue/Sprite3_crop"));
        m_portraitSpr.Add("Yellow", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/Character/Yellow/Sprite1_crop"));
        m_portraitSpr.Add("Pink", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/Character/Pink/Sprite2_crop"));
        m_portraitSpr.Add("White", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/Character/White/Sprite4_crop"));

        // �ڽ� �̹��� �Ҵ�
        m_boxISpr.Add("UI_VisualNovel_Blue_ChatBox", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/ChatBox/UI_VisualNovel_Blue_ChatBox"));
        m_boxISpr.Add("UI_VisualNovel_Pink_ChatBox", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/ChatBox/UI_VisualNovel_Pink_ChatBox"));
        m_boxISpr.Add("UI_VisualNovel_White_ChatBox", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/ChatBox/UI_VisualNovel_White_ChatBox"));
        m_boxISpr.Add("UI_VisualNovel_Yellow_ChatBox", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/ChatBox/UI_VisualNovel_Yellow_ChatBox"));
        m_boxISpr.Add("UI_VisualNovel_Blue_NarrationBox", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/NarrationBox/UI_VisualNovel_Blue_NarrationBox"));
        m_boxISpr.Add("UI_VisualNovel_Pink_NarrationBox", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/NarrationBox/UI_VisualNovel_Pink_NarrationBox"));
        m_boxISpr.Add("UI_VisualNovel_White_NarrationBox", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/NarrationBox/UI_VisualNovel_White_NarrationBox"));
        m_boxISpr.Add("UI_VisualNovel_Yellow_NarrationBox", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/NarrationBox/UI_VisualNovel_Yellow_NarrationBox"));

        // �� ������ �̹��� �Ҵ�
        m_ellipseSpr.Add("UI_VisualNovel_Blue_Ellipse", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_Blue_Ellipse"));
        m_ellipseSpr.Add("UI_VisualNovel_Pink_Ellipse", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_Pink_Ellipse"));
        m_ellipseSpr.Add("UI_VisualNovel_White_Ellipse", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_White_Ellipse"));
        m_ellipseSpr.Add("UI_VisualNovel_Yellow_Ellipse", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_Yellow_Ellipse"));

        // �ѱ�ǥ�� �̹��� �Ҵ�
        m_arrawSpr.Add("UI_VisualNovel_Blue_Ellipse", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_Blue_Ellipse"));
        m_arrawSpr.Add("UI_VisualNovel_Pink_Ellipse", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_Pink_Ellipse"));
        m_arrawSpr.Add("UI_VisualNovel_White_Ellipse", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_White_Ellipse"));
        m_arrawSpr.Add("UI_VisualNovel_Yellow_Ellipse", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_Yellow_Ellipse"));

        // ��ư �̹��� �Ҵ�
        m_choiceButtonSpr.Add("UI_VisualNovel_White_ButtonOFF", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Button/UI_VisualNovel_White_ButtonOFF"));
        m_choiceButtonSpr.Add("UI_VisualNovel_White_ButtonON", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Button/UI_VisualNovel_White_ButtonON"));

        // ��Ʈ �̹��� �Ҵ�
        m_heartSpr.Add("UI_VisualNovel_Blue_FriendshipHeartOFF", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Blue_FriendshipHeartOFF"));
        m_heartSpr.Add("UI_VisualNovel_Blue_FriendshipHeartON", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Blue_FriendshipHeartON"));
        m_heartSpr.Add("UI_VisualNovel_Pink_FriendshipHeartOFF", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Pink_FriendshipHeartOFF"));
        m_heartSpr.Add("UI_VisualNovel_Pink_FriendshipHeartON", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Pink_FriendshipHeartON"));
        m_heartSpr.Add("UI_VisualNovel_Yellow_FriendshipHeartOFF", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Yellow_FriendshipHeartOFF"));
        m_heartSpr.Add("UI_VisualNovel_Yellow_FriendshipHeartON", GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Yellow_FriendshipHeartON"));

        // ��Ʈ ���� �Ҵ�
        m_fontAst.Add("VN_Basic_Blue", GameManager.Ins.Resource.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/Blue/VN_Basic_Blue"));
        m_fontAst.Add("VN_Basic_RBlue", GameManager.Ins.Resource.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/Blue/VN_Basic_RBlue"));
        m_fontAst.Add("VN_Basic_Pink", GameManager.Ins.Resource.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/Pink/VN_Basic_Pink"));
        m_fontAst.Add("VN_Basic_RPink", GameManager.Ins.Resource.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/Pink/VN_Basic_RPink"));
        m_fontAst.Add("VN_Basic_White", GameManager.Ins.Resource.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/White/VN_Basic_White"));
        m_fontAst.Add("VN_Basic_RWhite", GameManager.Ins.Resource.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/White/VN_Basic_RWhite"));
        m_fontAst.Add("VN_Basic_Yellow", GameManager.Ins.Resource.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/Yellow/VN_Basic_Yellow"));
        m_fontAst.Add("VN_Basic_RYellow", GameManager.Ins.Resource.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/Yellow/VN_Basic_RYellow"));
    }

    public void Button_Start()
    {
        Destroy(m_StartPanel);
        m_levelController.Change_Level((int)m_startState);
    }

    public void Button_Exit()
    {
        SceneManager.LoadScene("Window");
    }

    public void Active_Popup()
    {
        if (m_likeabilityPanel == null || m_levelController.Curlevel == (int)LEVELSTATE.LS_CHASEGAME)
            return;

        // ȣ����â ��/ Ȱ��ȭ
        m_likeabilityPanel.SetActive(!m_likeabilityPanel.activeSelf);
        if (true == m_likeabilityPanel.activeSelf)
        {
            // ȣ����â NPC���� ������Ʈ
            for (int i = 0; i < m_likeabilityHeartPanel.Length; i++)
                m_likeabilityHeartPanel[i].Update_Heart();
        }
    }
}