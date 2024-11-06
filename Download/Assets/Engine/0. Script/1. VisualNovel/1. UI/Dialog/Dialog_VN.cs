using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;

namespace VisualNovel
{
    public class Dialog_VN : Dialog<DialogData_VN>
    {
        private enum EVENTTYPE { ET_NONE, ET_DIALOGTEXT, ET_BIGMINATS, ET_SWEATMINATS, ET_SCALEMINATS, ET_HINALIKEONE, ET_SCROLLTEXT, ET_AYAKAEYE, ET_AYAKHAND, ET_AYAKSHADOW, ET_END }
        private enum SKIPTYPE { ST_NONE, ST_SPEED1, ST_SPEED2, ST_END }

        [Header("GameObject")]
        [SerializeField] private GameObject m_darkPanelObj;
        [SerializeField] private GameObject m_backgroundObj;
        [SerializeField] private GameObject[] m_standingObj;
        [SerializeField] private GameObject m_dialogBoxObj;
        [SerializeField] private TMP_Text m_nameTxt;
        [SerializeField] private TMP_Text m_dialogTxt;
        [SerializeField] private NpcLike m_heartScr;
        [SerializeField] private TMP_Text m_skipTxt;

        private Image   m_backgroundImg;
        private Image[] m_standingImg;
        private bool m_isEvent = false;

        private List<bool>       m_slectBool;
        private int              m_choiceIndex = 0;
        private List<GameObject> m_choice_Button = new List<GameObject>();

        private SKIPTYPE m_skipType = SKIPTYPE.ST_NONE;
        private Coroutine m_dialogSkip = null;

        private int m_eventBeforeIndex = -1;
        private Coroutine m_eventCoroutines = null;
        private bool m_standingUpdate = true;

        private bool m_cutScene = false;
        public bool CutScene { set => m_cutScene = value; }

        private void Awake()
        {
            m_typeSpeed = 0.045f;

            m_backgroundImg = m_backgroundObj.GetComponent<Image>();

            m_standingImg = new Image[m_standingObj.Length];
            for (int i = 0; i < m_standingObj.Length; i++)
                m_standingImg[i] = m_standingObj[i].GetComponent<Image>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
                Update_Dialogs();

            // ��ư ������Ʈ
            if (0 < m_choice_Button.Count)
                Update_Button();

            /*All Skip
            if(Input.GetKeyDown(KeyCode.F1))
            {
                m_dialogIndex = m_dialogs.Count - 1;
                m_isTyping    = false;
                Update_Dialog();
            }*/
        }

        private void Update_Dialogs(bool IsPonter = true)
        {
            // Ŀ���� UI�� ������ �� ������Ʈ ����
            if (IsPonter && EventSystem.current.IsPointerOverGameObject() || m_isEvent == true)
                return;

            if (m_isTyping)
            {
                m_cancelTyping = true;
            }
            else // ���̾�α� ����
            {
                if (m_dialogIndex < m_dialogs.Count)
                {
                    switch (m_dialogs[m_dialogIndex].dialogType)
                    {
                        case DialogData_VN.DIALOG_TYPE.DT_FADE:
                            Update_Fade();
                            break;

                        case DialogData_VN.DIALOG_TYPE.DT_DIALOG:
                            Update_Dialog(m_dialogIndex);
                            break;

                        case DialogData_VN.DIALOG_TYPE.DT_GAMESTATE:
                            Update_GameState();
                            break;

                        case DialogData_VN.DIALOG_TYPE.DT_CUTSCENE:
                            Update_CutScene();
                            break;
                    }
                }
                else // ���̾�α� ����
                {
                    if (0 < m_choice_Button.Count)
                        return;

                    Close_Dialog();
                }
            }
        }

        #region Fade
        private void Update_Fade()
        {
            FadeData fadeData = (FadeData)m_dialogs[m_dialogIndex].dialogSubData;
            switch(fadeData.fadeType)
            {
                case FadeData.FADETYPE.FT_IN:
                    Update_FadeIn();
                    break;

                case FadeData.FADETYPE.FT_OUT:
                    Update_FadeOut(fadeData);
                    break;

                case FadeData.FADETYPE.FT_INOUT:
                    Update_FadeInOut(fadeData);
                    break;

                case FadeData.FADETYPE.FT_OUTIN:
                    Update_FadeOutIn();
                    break;

                case FadeData.FADETYPE.FT_NONE:
                    Start_Dialog(fadeData.pathIndex);
                    break;
            }
        }

        private void Update_FadeIn() // ���̾�α�, �߰�
        {
            m_isEvent = false;

            // ���� ���̾�α� Ÿ�Կ� ���� ó��
            if (m_dialogs[m_dialogIndex + 1].dialogType == DialogData_VN.DIALOG_TYPE.DT_CUTSCENE)
            {
                m_dialogIndex++;
            }
            else // ���� �����ͷ� �̸� ������Ʈ
            {
                Update_Dialog(m_dialogIndex + 1);
            }

            // �ؽ�Ʈ �ʱ�ȭ
            if (m_dialogTextCoroutine != null)
                StopCoroutine(m_dialogTextCoroutine);
            m_isTyping = false;
            m_dialogTxt.text = "";

            GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Update_Dialogs());
        }

        private void Update_FadeOut(FadeData fadeData)
        {
            if (fadeData.pathIndex < 0)
                GameManager.Ins.UI.Start_FadeOut(1f, Color.black);
            else
                GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => Start_Dialog(fadeData.pathIndex), 0f, false);
        }

        private void Update_FadeInOut(FadeData fadeData)
        {
            GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Update_FadeOut(fadeData), 0.5f, false);
        }

        private void Update_FadeOutIn()
        {
            if (m_isEvent == true)
                return;

            m_isEvent = true;
            GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => Update_FadeIn(), 0.5f, false);
        }
        #endregion

        #region Dialog
        private void Update_DialogBasic(VisualNovelManager.OWNERTYPE ownerType, string name)
        {
            // ���̾�α� �ڽ� Ȱ��ȭ
            m_dialogBoxObj.SetActive(true);

            // ȣ���� ������Ʈ
            m_heartScr.Set_Owner(ownerType);

            // ���̾�α� ���� �̸� ������Ʈ
            if(ownerType == VisualNovelManager.OWNERTYPE.OT_WHITE)
            {
                m_nameTxt.text = GameManager.Ins.PlayerName;
            }
            else
            {
                m_nameTxt.text = name;
            }
        }

        private void Update_Dialog(int dialogIndex, bool nextUpdate = false)
        {
            DialogData dialogData = (DialogData)m_dialogs[dialogIndex].dialogSubData;

            // �⺻ ���� ������Ʈ
            Update_DialogBasic(dialogData.owner, dialogData.dialogName);

            // ���ҽ� ������Ʈ : ���, ���ĵ�
            if (!string.IsNullOrEmpty(dialogData.backgroundSpr))
                m_backgroundImg.sprite = GameManager.Ins.Novel.BackgroundSpr[dialogData.backgroundSpr];
            if(m_standingUpdate == true)
                Update_Standing(dialogData.standingSpr);

            // Ÿ���� ������Ʈ
            if (m_dialogTextCoroutine != null)
                StopCoroutine(m_dialogTextCoroutine);
            m_dialogTextCoroutine = StartCoroutine(Type_Text(dialogData, nextUpdate));

            // �̺�Ʈ ����
            Start_Event(dialogData);

            m_dialogIndex++;
        }

        private void Update_Standing(List<string> standingSpr)
        {
            switch (standingSpr.Count)
            {
                case 0:
                    m_standingObj[0].SetActive(false);
                    m_standingObj[1].SetActive(false);
                    m_standingObj[2].SetActive(false);
                    break;

                case 1:
                    if (!string.IsNullOrEmpty(standingSpr[0]))
                    {
                        m_standingObj[0].SetActive(true);
                        m_standingObj[0].transform.localPosition = new Vector3(0.0f, -147f, 0.0f);
                        m_standingImg[0].sprite = GameManager.Ins.Novel.StandingSpr[standingSpr[0]];
                    }

                    m_standingObj[1].SetActive(false);
                    m_standingObj[2].SetActive(false);
                    break;

                case 2:
                    if (!string.IsNullOrEmpty(standingSpr[0]))
                    {
                        m_standingObj[0].SetActive(true);
                        m_standingObj[0].transform.localPosition = new Vector3(-300.0f, -147f, 0.0f);
                        m_standingImg[0].sprite = GameManager.Ins.Novel.StandingSpr[standingSpr[0]];
                    }

                    if (!string.IsNullOrEmpty(standingSpr[1]))
                    {
                        m_standingObj[1].SetActive(true);
                        m_standingObj[1].transform.localPosition = new Vector3(300.0f, -147f, 0.0f);
                        m_standingImg[1].sprite = GameManager.Ins.Novel.StandingSpr[standingSpr[1]];
                    }
                    m_standingObj[2].SetActive(false);
                    break;

                case 3:
                    if (!string.IsNullOrEmpty(standingSpr[0]))
                    {
                        m_standingObj[0].SetActive(true);
                        m_standingObj[0].transform.localPosition = new Vector3(-555.0f, -147f, 0.0f);
                        m_standingImg[0].sprite = GameManager.Ins.Novel.StandingSpr[standingSpr[0]];
                    }

                    if (!string.IsNullOrEmpty(standingSpr[1]))
                    {
                        m_standingObj[1].SetActive(true);
                        m_standingObj[1].transform.localPosition = new Vector3(0.0f, -147f, 0.0f);
                        m_standingImg[1].sprite = GameManager.Ins.Novel.StandingSpr[standingSpr[1]];
                    }

                    if (!string.IsNullOrEmpty(standingSpr[2]))
                    {
                        m_standingObj[2].SetActive(true);
                        m_standingObj[2].transform.localPosition = new Vector3(555.0f, -147f, 0.0f);
                        m_standingImg[2].sprite = GameManager.Ins.Novel.StandingSpr[standingSpr[2]];
                    }
                    break;
            }
        }

        #region Button
        private void Create_ChoiceButton(ChoiceData choiceData)
        {
            // ���� ��� Ȱ��ȭ
            m_darkPanelObj.SetActive(true);

            // ������ ��ư ����
            float startHeight;
            if (choiceData.choiceText.Count == 3)
                startHeight = 250f;
            else
                startHeight = 150f;
            for (int i = 0; i < choiceData.choiceText.Count; ++i)
            {
                int ButtonIndex = i; // ��ư ���� �ε���

                GameObject Clone = GameManager.Ins.Resource.LoadCreate("5. Prefab/1. VisualNovel/UI/Button_Choice_VN");
                if (Clone != null)
                {
                    Clone.transform.SetParent(gameObject.transform);
                    Clone.transform.localPosition = new Vector3(0f, (startHeight + (i * -130)), 0f);
                    Clone.transform.localScale    = new Vector3(1f, 1f, 1f);

                    ButtonChoice_VN ButtonChoice = Clone.GetComponent<ButtonChoice_VN>();
                    ButtonChoice.ButtonIndex = i;
                    ButtonChoice.Ownerdialog = this;

                    TMP_Text TextCom = Clone.GetComponentInChildren<TMP_Text>();
                    if (TextCom)
                    {
                        TextCom.text = choiceData.choiceText[i];

                        Button button = Clone.GetComponent<Button>();
                        if (button != null) // �̺�Ʈ �ڵ鷯 �߰�
                            button.onClick.AddListener(() => Click_Button(ButtonIndex));

                        m_choice_Button.Add(Clone);
                    }
                }
            }

            // �⺻ ���� ���� ����
            m_choiceIndex = 0;
            m_choice_Button[m_choiceIndex].GetComponent<Image>().sprite = GameManager.Ins.Novel.ChoiceButtonSpr["UI_VisualNovel_White_ButtonON"];

            Reset_Skip();
        }

        private void Update_Button()
        {
            if (m_isTyping == true)
                return;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                Click_Button(m_choiceIndex);
                return;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                m_choiceIndex--;
                if (m_choiceIndex < 0)
                    m_choiceIndex = m_choice_Button.Count - 1;

                Update_ButtonImage();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                m_choiceIndex++;
                if (m_choiceIndex > m_choice_Button.Count - 1)
                    m_choiceIndex = 0;

                Update_ButtonImage();
            }
        }

        private void Click_Button(int index)
        {
            if(m_slectBool != null)
            {
                if (m_slectBool[index] == true)
                    return;

                m_slectBool[index] = true;
            }

            DialogData dialogData = (DialogData)m_dialogs[m_dialogIndex - 1].dialogSubData;
            ChoiceData choiceData = dialogData.choiceData;
            switch (choiceData.choiceEventType[index])
            {
                case ChoiceData.CHOICETYPE.CT_CLOSE: // ���̾�α� ����
                    Close_Dialog();
                    break;

                case ChoiceData.CHOICETYPE.CT_DIALOG: // ���̾�α� �����
                    //Start_Dialog(GameManager.Ins.Load_JsonData<DialogData_VN>(choiceData.choiceDialog[index - 1]));
                    Start_Dialog(choiceData.choiceDialog[index]);
                    break;
            }
        }

        public void Update_ButtonImage()
        {
            // ���� �ε��� ��ư�� ������ ��� ��ư Off �̹����� �ʱ�ȭ
            for (int i = 0; i < m_choice_Button.Count; ++i)
            {
                if (i == m_choiceIndex)
                    m_choice_Button[i].GetComponent<Image>().sprite = GameManager.Ins.Novel.ChoiceButtonSpr["UI_VisualNovel_White_ButtonON"]; // On
                else
                    m_choice_Button[i].GetComponent<Image>().sprite = GameManager.Ins.Novel.ChoiceButtonSpr["UI_VisualNovel_White_ButtonOFF"]; // Off
            }
        }

        public void Enter_Button(int index)
        {
            m_choiceIndex = index;
            Update_ButtonImage();
        }
        #endregion
        #endregion

        #region GameState
        private void Update_GameState()
        {
            GameState gameState = (GameState)m_dialogs[m_dialogIndex].dialogSubData;

            Action action = null;
            switch (gameState.gameType)
            {
                case GameState.GAMETYPE.GT_DAY2:
                    action = () => GameManager.Ins.Novel.LevelController.Change_Level((int)VisualNovelManager.LEVELSTATE.LS_DAY2);
                    break;

                case GameState.GAMETYPE.GT_DAY3:
                    action = () => GameManager.Ins.Novel.LevelController.Change_Level((int)VisualNovelManager.LEVELSTATE.LS_DAY3BEFORE);
                    break;

                case GameState.GAMETYPE.GT_STARTSHOOT:
                    action = () => GameManager.Ins.Novel.LevelController.Change_Level((int)VisualNovelManager.LEVELSTATE.LS_DAY3SHOOTGAME);
                    break;

                case GameState.GAMETYPE.GT_STARTCHASE:
                    action = () => GameManager.Ins.Novel.LevelController.Change_Level((int)VisualNovelManager.LEVELSTATE.LS_DAY3CHASEGAME);
                    break;

                case GameState.GAMETYPE.GT_PLAYCHASE:
                    action = () => GameManager.Ins.Novel.LevelController.Get_CurrentLevel<Novel_Day3Chase>().Play_Level();
                    break;
            }

            GameManager.Ins.UI.Start_FadeOut(1f, Color.black, action, 0.5f, false);
        }
        #endregion

        #region CutScene
        private void Update_CutScene()
        {
            if (m_cutScene == true)
                return;

            CutScene dialogData = (CutScene)m_dialogs[m_dialogIndex].dialogSubData;

            int eventCount = dialogData.cutSceneEvents.Count;
            if (eventCount <= 0)
                return;

            m_cutScene = true;
            for (int i = 0; i < eventCount; ++i)
            {
                CutScene.CUTSCENETYPE cutSceneEvent = dialogData.cutSceneEvents[i];
                switch (cutSceneEvent)
                {
                    case VisualNovel.CutScene.CUTSCENETYPE.CT_BLINK:
                        Update_Blink((BasicValue)dialogData.eventValues[i]);
                        break;

                    case VisualNovel.CutScene.CUTSCENETYPE.CT_CAMERA:
                        Update_Camera((CameraValue)dialogData.eventValues[i]);
                        break;

                    case VisualNovel.CutScene.CUTSCENETYPE.CT_ANIMATION:
                        Update_Animation((AnimationValue)dialogData.eventValues[i]);
                        break;

                    case VisualNovel.CutScene.CUTSCENETYPE.CT_LIKEPANEL:
                        StartCoroutine(Update_LikePanel());
                        break;

                    case VisualNovel.CutScene.CUTSCENETYPE.CT_ACTIVE:
                        Update_Active((ActiveValue)dialogData.eventValues[i]);
                        break;

                    case VisualNovel.CutScene.CUTSCENETYPE.CT_IMAGE:
                        //Update_Image((ImageValue)dialogData.eventValues[i]);
                        break;
                }
            }
        }

        private void Update_Blink(BasicValue basicValue) // �� �ƿ� // �� �ƿ� // ��
        {
            m_dialogBoxObj.SetActive(false);

            GameManager.Ins.UI.Start_FadeIn(3.5f, Color.black,
                () => GameManager.Ins.UI.Start_FadeOut(2f, Color.black,

                () => GameManager.Ins.UI.Start_FadeIn(1f, Color.black,
                () => GameManager.Ins.UI.Start_FadeOut(1f, Color.black,

                () => GameManager.Ins.UI.Start_FadeIn(1f, Color.black,
                () => Finish_CutScene(basicValue.nextIndex)), 0f, false)), 0f, false));
        }

        private void Update_Camera(CameraValue cameraValue)
        {
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_CUTSCENE);

            CameraCutscene camera = (CameraCutscene)GameManager.Ins.Camera.Get_CurCamera();
            if (cameraValue.usePosition == true)
                camera.Start_Position(cameraValue.targetPosition, cameraValue.positionSpeed);
            if (cameraValue.useRotation == true)
                camera.Start_Rotation(cameraValue.targetRotation, cameraValue.rotationSpeed);

            StartCoroutine(Finish_Camera(camera, cameraValue.usePosition, cameraValue.useRotation, cameraValue.nextIndex));
        }

        private void Update_Animation(AnimationValue animationValue)
        {
            switch (animationValue.owner)
            {
                case VisualNovelManager.OWNERTYPE.OT_PINK:
                    // ������Ʈ ���� Ȯ��
                    if (!string.IsNullOrEmpty(animationValue.animatroTriger))
                    {
                        Novel_Day3Chase novel_Chase = GameManager.Ins.Novel.LevelController.Get_CurrentLevel<Novel_Day3Chase>();
                        novel_Chase.YandereAnimator.SetTrigger(animationValue.animatroTriger);
                    }
                    break;
            }

            Update_DialogBasic(animationValue.owner, animationValue.dialogName);
            if (m_dialogTextCoroutine != null)
                StopCoroutine(m_dialogTextCoroutine);
            m_dialogTextCoroutine = StartCoroutine(Type_CutText(animationValue));
            m_dialogIndex++;

            m_cutScene = false;
        }

        private IEnumerator Update_LikePanel()
        {
            float time = 0f;
            while (true)
            {
                time += Time.deltaTime;
                if (time >= 0.5f)
                {
                    GameManager.Ins.Novel.LikeabilityPanel.SetActive(true);
                    break;
                }

                yield return null;
            }

            time = 0f;
            while (true)
            {
                time += Time.deltaTime;
                if (time >= 1.0f)
                {
                    GameManager.Ins.Novel.LikeabilityPanel.GetComponent<Likeability>().Shake_Heart();
                    break;
                }

                yield return null;
            }

            m_dialogIndex++;
            yield break;
        }

        private void Update_Active(ActiveValue activeValue)
        {
            switch (activeValue.objectType)
            {
                case ActiveValue.OBJECT_TYPE.OJ_SAW:
                    GameManager.Ins.Novel.LevelController.Get_CurrentLevel<Novel_Day3Chase>().Yandere.gameObject.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(activeValue.active);
                    break;
            }

            Finish_CutScene(activeValue.nextIndex);
        }

        private void Update_Image(ImageValue iamgeValue)
        {
            // ���ĵ� ��Ȱ��ȭ
            m_standingObj[0].SetActive(false);
            m_standingObj[1].SetActive(false);
            m_standingObj[2].SetActive(false);

            // �̹��� �ڽ�
            m_dialogBoxObj.SetActive(false);

            // �ƾ� �̹��� ����
            //if (!string.IsNullOrEmpty(iamgeValue.imageName))
            //    m_backgroundImg.sprite = GameManager.Ins.Novel.CutScene[iamgeValue.imageName];

            m_cutScene = false;
            m_dialogIndex++;
        }

        private void Finish_CutScene(bool nextIndex)
        {
            m_cutScene = false;
            m_dialogIndex++;

            if (nextIndex == false)
                return;
            Update_Dialogs();
        }

        private IEnumerator Finish_Camera(CameraCutscene camera, bool position, bool rotation, bool nextIndex)
        {
            while (true)
            {
                if (position == true && rotation == true)
                {
                    if (camera.IsPosition == false && camera.IsRotation == false)
                        break;
                }
                else if (position == true)
                {
                    if (camera.IsPosition == false)
                        break;
                }
                else if (rotation == true)
                {
                    if (camera.IsRotation == false)
                        break;
                }

                yield return null;
            }

            Finish_CutScene(nextIndex);
            yield break;
        }
        #endregion


        #region Skip
        public void Button_Skip()
        {
            m_skipType++;
            if (m_skipType > SKIPTYPE.ST_SPEED2)
                m_skipType = SKIPTYPE.ST_NONE;

            switch (m_skipType)
            {
                case SKIPTYPE.ST_NONE:
                    Reset_Skip();
                    break;
                case SKIPTYPE.ST_SPEED1:
                    m_skipTxt.text = "Skipx1";
                    if (m_dialogSkip != null)
                        StopCoroutine(m_dialogSkip);
                    m_dialogSkip = StartCoroutine(DialogSkip(0.2f));
                    break;
                case SKIPTYPE.ST_SPEED2:
                    m_skipTxt.text = "Skipx2";
                    if (m_dialogSkip != null)
                        StopCoroutine(m_dialogSkip);
                    m_dialogSkip = StartCoroutine(DialogSkip(0.1f));
                    break;
            }
        }

        public void Reset_Skip()
        {
            m_skipType = SKIPTYPE.ST_NONE;

            m_skipTxt.text = "Skip";
            if (m_dialogSkip != null)
                StopCoroutine(m_dialogSkip);
        }

        private IEnumerator DialogSkip(float speed)
        {
            while (m_dialogIndex < m_dialogs.Count)
            {
                Update_Dialogs(false);
                yield return new WaitForSeconds(speed);
            }

            yield break;
        }
        #endregion

        #region Event
        private void Start_Event(DialogData data)
        {
            if (m_eventBeforeIndex == data.eventIndex)
                return;

            End_Event(data.eventIndex);

            m_eventBeforeIndex = data.eventIndex;
            switch (data.eventIndex)
            {
                case (int)EVENTTYPE.ET_DIALOGTEXT:
                    m_dialogTxt.rectTransform.anchoredPosition = new Vector2(0f, 61f);
                    break;

                case (int)EVENTTYPE.ET_BIGMINATS:
                    Transform child = transform.GetChild(2);
                    child.SetSiblingIndex(5);
                    RectTransform rt = child.GetChild(0).GetComponent<RectTransform>();
                    rt.anchoredPosition = new Vector2(0f, -1387f);
                    rt.localScale = new Vector3(2.239582f, 2.239582f, 2.239582f);
                    m_standingUpdate = false;
                    break;

                case (int)EVENTTYPE.ET_SWEATMINATS:
                    GameObject effect = GameManager.Ins.Resource.LoadCreate("5. Prefab/1. VisualNovel/Effect/Panel_Sweat", m_standingObj[0].transform);
                    effect.transform.SetSiblingIndex(1);
                    for(int i = 0; i < effect.transform.childCount; ++i)
                    {
                        Animator am = effect.transform.GetChild(i).GetComponent<Animator>();
                        if(am != null)
                        {
                            float randomStartTime = UnityEngine.Random.Range(0f, am.GetCurrentAnimatorStateInfo(0).length);
                            am.Play(am.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, randomStartTime);
                        }
                    }
                    break;

                case (int)EVENTTYPE.ET_SCALEMINATS:
                    m_eventCoroutines = StartCoroutine(Update_ScaleMinats());
                    break;

                case (int)EVENTTYPE.ET_HINALIKEONE:
                    GameManager.Ins.Novel.NpcHeart[(int)VisualNovelManager.OWNERTYPE.OT_YELLOW] += 1;
                    break;

                case (int)EVENTTYPE.ET_SCROLLTEXT:
                    if (m_dialogTextCoroutine != null)
                        StopCoroutine(m_dialogTextCoroutine);
                    m_isTyping = false;

                    string dialogText = data.dialogText.Replace("{{PLAYER_NAME}}", GameManager.Ins.PlayerName);
                    m_dialogTxt.text = dialogText;
                    m_dialogTxt.text += m_dialogTxt.text;
                    m_eventCoroutines = StartCoroutine(Update_ScrollText(dialogText));
                    break;

                case (int)EVENTTYPE.ET_AYAKAEYE:
                    m_eventCoroutines = StartCoroutine(Update_Background(data.backgroundSpr, "AyakaEye"));
                    break;

                case (int)EVENTTYPE.ET_AYAKHAND:
                    m_eventCoroutines = StartCoroutine(Update_Background(data.backgroundSpr, "TSAyaka01"));
                    break;

                case (int)EVENTTYPE.ET_AYAKSHADOW:
                    m_eventCoroutines = StartCoroutine(Update_AlphaBackground("BackGround_BandRoomAyaka"));
                    break;
            }
        }

        private void End_Event(int currentIndex)
        {
            if (m_eventBeforeIndex == -1)
                return;

            switch (m_eventBeforeIndex)
            {
                case (int)EVENTTYPE.ET_DIALOGTEXT:
                    m_dialogTxt.rectTransform.anchoredPosition = new Vector2(0f, -33f);
                    break;

                case (int)EVENTTYPE.ET_BIGMINATS:
                    Transform child = transform.GetChild(5);
                    child.SetSiblingIndex(2);
                    RectTransform rt = child.GetChild(0).GetComponent<RectTransform>();
                    rt.anchoredPosition = new Vector2(0f, -147f);
                    rt.localScale = new Vector3(0.61121f, 0.61121f, 0.61121f);
                    m_standingUpdate = true;
                    break;

                case (int)EVENTTYPE.ET_SCALEMINATS:
                    GameManager.Ins.Resource.Destroy(m_standingObj[0].transform.GetChild(0).gameObject);
                    if (m_eventCoroutines != null)
                        StopCoroutine(m_eventCoroutines);
                    m_standingObj[0].transform.localScale = new Vector3(0.61121f, 0.61121f, 0.61121f);
                    break;

                case (int)EVENTTYPE.ET_SCROLLTEXT:
                    if (m_eventCoroutines != null)
                        StopCoroutine(m_eventCoroutines);
                    break;

                case (int)EVENTTYPE.ET_AYAKAEYE:
                    if (m_eventCoroutines != null)
                        StopCoroutine(m_eventCoroutines);
                    break;

                case (int)EVENTTYPE.ET_AYAKHAND:
                    if (m_eventCoroutines != null)
                        StopCoroutine(m_eventCoroutines);
                    break;

                case (int)EVENTTYPE.ET_AYAKSHADOW:
                    if(m_eventCoroutines != null)
                        StopCoroutine(m_eventCoroutines);
                    GameManager.Ins.Resource.Destroy(m_backgroundImg.gameObject.transform.GetChild(0).gameObject);
                    break;
            }

            m_eventBeforeIndex = -1;
        }

        private IEnumerator Update_ScaleMinats()
        {
            Transform objTransform = m_standingObj[0].transform;

            float scaleDuration = 1f;  // ũ�� ��ȭ�� �ɸ��� �ð�
            Vector3 originalScale = objTransform.localScale;  // ���� ������ ����
            Vector3 targetScale = originalScale * 1.2f;  // 1.5�� Ȯ���� ������

            while (true)
            {
                // Ȯ��
                for (float t = 0; t < scaleDuration; t += Time.deltaTime)
                {
                    objTransform.localScale = Vector3.Lerp(originalScale, targetScale, t / scaleDuration);
                    yield return null;
                }

                // ���
                for (float t = 0; t < scaleDuration; t += Time.deltaTime)
                {
                    objTransform.localScale = Vector3.Lerp(targetScale, originalScale, t / scaleDuration);
                    yield return null;
                }
            }
        }

        private IEnumerator Update_ScrollText(string text)
        {
            int maxCount = text.Length * 10 - UnityEngine.Random.Range(0, 100);
            while (true)
            {
                m_dialogTxt.text += text;
                yield return new WaitForSeconds(0.2f);

                if (m_dialogTxt.text.Length > maxCount)
                {
                    m_dialogTxt.text = m_dialogTxt.text.Substring(m_dialogTxt.text.Length - maxCount);
                    maxCount = text.Length * 10 - UnityEngine.Random.Range(0, 100);
                    yield return null;
                }
            }
        }

        private IEnumerator Update_Background(string backgroundSpr, string changeImg)
        {
            yield return new WaitForSeconds(0.2f);

            m_backgroundImg.sprite = GameManager.Ins.Novel.BackgroundSpr[changeImg];
            yield return new WaitForSeconds(0.1f);

            if (!string.IsNullOrEmpty(backgroundSpr))
                m_backgroundImg.sprite = GameManager.Ins.Novel.BackgroundSpr[backgroundSpr];
            yield break;
        }

        private IEnumerator Update_AlphaBackground(string name)
        {
            m_isEvent = true;

            GameObject bg = GameManager.Ins.Resource.Create(m_backgroundImg.gameObject, m_backgroundImg.gameObject.transform);
            Image bgImage = bg.GetComponent<Image>();
            bgImage.sprite = GameManager.Ins.Novel.BackgroundSpr[name];
            bgImage.raycastTarget = false;

            Color color = bgImage.color;
            color.a = 0;
            bgImage.color = color;

            float duration = 0.6f;
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                if (m_skipType != SKIPTYPE.ST_NONE)
                    break;

                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(0, 1, elapsedTime / duration);
                bgImage.color = color;
                yield return null;
            }

            color.a = 1;
            bgImage.color = color;
            m_isEvent = false;
            yield break;
        }
        #endregion

        private IEnumerator Type_CutText(AnimationValue animationValue)
        {
            // �÷��̾� �̸��� ���� �� �Է� ���� �̸����� ����
            string dialogText = animationValue.dialogText.Replace("{{PLAYER_NAME}}", GameManager.Ins.PlayerName);

            m_isTyping = true;
            m_cancelTyping = false;

            m_dialogTxt.text = "";
            foreach (char letter in dialogText.ToCharArray())
            {
                if (m_cancelTyping)
                {
                    m_dialogTxt.text = dialogText;
                    break;
                }

                m_dialogTxt.text += letter;
                yield return new WaitForSeconds(m_typeSpeed);
            }

            m_isTyping = false;

            // �ڵ� ������Ʈ
            if (animationValue.nextIndex == true)
                Update_Dialogs();

            yield break;
        }

        public void Close_Background()
        {
            m_backgroundObj.SetActive(false);
        }


        #region Common
        public void Start_Dialog(List<DialogData_VN> dialogs = null)
        {
            // ���̾�α� ���� �ʱ�ȭ
            m_dialogs = dialogs;

            // ���� �ʱ�ȭ
            m_isTyping = false;
            m_cancelTyping = false;
            m_dialogIndex = 0;
            m_choiceIndex = 0;

            m_darkPanelObj.SetActive(false);
            m_standingObj[0].SetActive(false);
            m_standingObj[1].SetActive(false);
            m_standingObj[2].SetActive(false);

            for (int i = 0; i < m_choice_Button.Count; ++i)
                Destroy(m_choice_Button[i]);
            m_choice_Button.Clear();

            m_backgroundObj.SetActive(true);
            gameObject.SetActive(true);
            Update_Dialogs(false);
        }

        public void Start_Dialog(int sheetIndex)
        {
            Novel_Level level = GameManager.Ins.Novel.LevelController.Get_CurrentLevel<Novel_Level>();
            if (level == null)
                return;

            List<ExcelData> sheetList = level.Get_DialogData(sheetIndex);
            List<DialogData_VN> dialogs = new List<DialogData_VN>();
            for (int i = 0; i < sheetList.Count; ++i)
            {
                DialogData_VN data = new DialogData_VN();
                data.dialogType = (DialogData_VN.DIALOG_TYPE)sheetList[i].dialogType;

                switch (data.dialogType)
                {
                    case DialogData_VN.DIALOG_TYPE.DT_FADE:
                        FadeData fadeData = new FadeData();
                        fadeData.fadeType = (FadeData.FADETYPE)sheetList[i].fadeType;
                        fadeData.pathIndex = sheetList[i].pathIndex;

                        data.dialogSubData = fadeData;
                        break;

                    case DialogData_VN.DIALOG_TYPE.DT_DIALOG:
                        DialogData dialogData = new DialogData();
                        dialogData.owner = (VisualNovelManager.OWNERTYPE)sheetList[i].owner;
                        dialogData.dialogName = sheetList[i].dialogName;
                        dialogData.dialogText = sheetList[i].dialogText;
                        dialogData.backgroundSpr = sheetList[i].backgroundSpr;
                        dialogData.standingSpr = ExtractTextsInCurlyBrackets(sheetList[i].standingSpr);
                        dialogData.eventIndex = sheetList[i].pathIndex;
                        dialogData.addLike = sheetList[i].addLike;

                        ChoiceData choiceData = new ChoiceData();
                        choiceData.choiceLoop = sheetList[i].choiceLoop;
                        choiceData.choiceEventType = ExtractValuesInCurlyBrackets<ChoiceData.CHOICETYPE>(sheetList[i].choiceEventType);
                        choiceData.choiceText = ExtractTextsInCurlyBrackets(sheetList[i].choiceText);
                        choiceData.choiceDialog = ExtractIntegers(sheetList[i].choiceDialog);
                        choiceData.pathIndex = sheetList[i].pathIndex;
                        dialogData.choiceData = choiceData;

                        data.dialogSubData = dialogData;
                        break;

                    case DialogData_VN.DIALOG_TYPE.DT_GAMESTATE:
                        GameState gameState = new GameState();
                        gameState.gameType = (GameState.GAMETYPE)sheetList[i].gameType;
                        data.dialogSubData = gameState;
                        break;

                    case DialogData_VN.DIALOG_TYPE.DT_CUTSCENE:
                        CutScene cutScene = new CutScene();
                        cutScene.cutSceneEvents = ExtractValuesInCurlyBrackets<CutScene.CUTSCENETYPE>(sheetList[i].cutSceneEvents);

                        cutScene.eventValues = new List<CutSceneValue>();
                        for (int j = 0; j < cutScene.cutSceneEvents.Count; ++j)
                        {
                            CutSceneValue cutSceneValue;
                            switch (cutScene.cutSceneEvents[j])
                            {
                                case VisualNovel.CutScene.CUTSCENETYPE.CT_BLINK:
                                    BasicValue basicValue = new BasicValue();
                                    basicValue.nextIndex = sheetList[i].nextIndex;
                                    cutSceneValue = basicValue;
                                    cutScene.eventValues.Add(cutSceneValue);
                                    break;

                                case VisualNovel.CutScene.CUTSCENETYPE.CT_CAMERA:
                                    //CameraValue cameraValue = new CameraValue();
                                    //cameraValue.nextIndex;
                                    //cameraValue.usePosition;
                                    //cameraValue.targetPosition;
                                    //cameraValue.positionSpeed;
                                    //cameraValue.useRotation;
                                    //cameraValue.targetRotation;
                                    //cameraValue.rotationSpeed;
                                    BasicValue basicValue4 = new BasicValue();
                                    basicValue4.nextIndex = sheetList[i].nextIndex;
                                    cutSceneValue = basicValue4;
                                    cutScene.eventValues.Add(cutSceneValue);
                                    break;

                                case VisualNovel.CutScene.CUTSCENETYPE.CT_ANIMATION:
                                    //AnimationValue animationValue = new AnimationValue();
                                    //animationValue.nextIndex;
                                    //animationValue.owner;
                                    //animationValue.dialogName;
                                    //animationValue.dialogText;
                                    //animationValue.animatroTriger;
                                    BasicValue basicValue5 = new BasicValue();
                                    basicValue5.nextIndex = sheetList[i].nextIndex;
                                    cutSceneValue = basicValue5;
                                    cutScene.eventValues.Add(cutSceneValue);
                                    break;

                                case VisualNovel.CutScene.CUTSCENETYPE.CT_LIKEPANEL:
                                    BasicValue basicValue1 = new BasicValue();
                                    basicValue1.nextIndex = sheetList[i].nextIndex;
                                    cutSceneValue = basicValue1;
                                    cutScene.eventValues.Add(cutSceneValue);
                                    break;

                                case VisualNovel.CutScene.CUTSCENETYPE.CT_ACTIVE:
                                    //ActiveValue activeValue = new ActiveValue();
                                    //activeValue.nextIndex;
                                    //activeValue.objectType;
                                    //activeValue.active;
                                    BasicValue basicValue6 = new BasicValue();
                                    basicValue6.nextIndex = sheetList[i].nextIndex;
                                    cutSceneValue = basicValue6;
                                    cutScene.eventValues.Add(cutSceneValue);
                                    break;

                                case VisualNovel.CutScene.CUTSCENETYPE.CT_IMAGE:
                                    ImageValue imageValue = new ImageValue();
                                    imageValue.imageName = sheetList[i].imageName;
                                    cutSceneValue = imageValue;
                                    cutScene.eventValues.Add(cutSceneValue);
                                    break;

                                case VisualNovel.CutScene.CUTSCENETYPE.CT_SHAKE:
                                    BasicValue basicValue2 = new BasicValue();
                                    basicValue2.nextIndex = sheetList[i].nextIndex;
                                    cutSceneValue = basicValue2;
                                    cutScene.eventValues.Add(cutSceneValue);
                                    break;
                            }
                        }
                        data.dialogSubData = cutScene;
                        break;
                }
                dialogs.Add(data);
            }

            Start_Dialog(dialogs);
        }

        public static List<int> ExtractIntegers(string input)
        {
            List<int> result = new List<int>();
            Regex regex = new Regex(@"\{(.*?)\}");
            MatchCollection matches = regex.Matches(input);

            foreach (Match match in matches)
            {
                if (int.TryParse(match.Groups[1].Value, out int number))
                {
                    result.Add(number);
                }
            }

            return result;
        }

        public List<T> ExtractValuesInCurlyBrackets<T>(string input) where T : struct, Enum
        {
            List<T> result = new List<T>();

            MatchCollection matches = Regex.Matches(input, @"\{(.*?)\}");
            foreach (Match match in matches)
            {
                string value = match.Groups[1].Value;

                // Attempt to parse the string value into the enum type T
                if (Enum.TryParse(typeof(T), value, out var enumValue))
                {
                    result.Add((T)enumValue); // Add the parsed enum value to the list
                }
            }

            return result;
        }

        public List<string> ExtractTextsInCurlyBrackets(string input)
        {
            List<string> result = new List<string>();

            // Regular expression to capture content inside {}
            MatchCollection matches = Regex.Matches(input, @"\{(.*?)\}");

            foreach (Match match in matches)
            {
                result.Add(match.Groups[1].Value); // Add the content inside {} to the list
            }

            return result;
        }

        private void Close_Dialog()
        {
            gameObject.SetActive(false);
        }

        IEnumerator Type_Text(DialogData dialogData, bool nextUpdate)
        {
            // �÷��̾� �̸��� ���� �� �Է� ���� �̸����� ����
            string dialogText = dialogData.dialogText.Replace("{{PLAYER_NAME}}", GameManager.Ins.PlayerName);

            m_isTyping = true;
            m_cancelTyping = false;

            m_dialogTxt.text = "";
            foreach (char letter in dialogText.ToCharArray())
            {
                if (m_cancelTyping)
                {
                    m_dialogTxt.text = dialogText;
                    break;
                }

                m_dialogTxt.text += letter;
                yield return new WaitForSeconds(m_typeSpeed);
            }

            m_isTyping = false;

            // ������ ����
            if (dialogData.choiceData.choiceText != null)
            {
                if(dialogData.choiceData.choiceLoop == true)
                {
                    if (m_slectBool == null)
                    {
                        m_slectBool = new List<bool>();
                        for (int i = 0; i < dialogData.choiceData.choiceText.Count; ++i)
                            m_slectBool.Add(false);
                    }
                    else
                    {
                        // �ѹ��� �� Ŭ���ߴ°�?
                        bool allClick = true;
                        for (int i = 0; i < m_slectBool.Count; ++i)
                        {
                            if (m_slectBool[i] == false)
                            {
                                allClick = false;
                                break;
                            }
                        }

                        if(allClick == true)
                        {
                            m_slectBool = null;
                            Start_Dialog(dialogData.choiceData.pathIndex);
                            yield break;
                        }
                    }
                }

                if (0 < dialogData.choiceData.choiceText.Count)
                    Create_ChoiceButton(dialogData.choiceData);
            }

            // ȣ���� ����
            if (dialogData.addLike != 0)
            {
                GameManager.Ins.Novel.NpcHeart[(int)dialogData.owner] += dialogData.addLike;
                if (GameManager.Ins.Novel.NpcHeart[(int)dialogData.owner] < 0)
                    GameManager.Ins.Novel.NpcHeart[(int)dialogData.owner] = 0;
                m_heartScr.Set_Owner(dialogData.owner);
            }

            // �ڵ� ������Ʈ
            if (nextUpdate == true)
                Update_Dialogs();

            yield break;
        }
        #endregion
    }
}

