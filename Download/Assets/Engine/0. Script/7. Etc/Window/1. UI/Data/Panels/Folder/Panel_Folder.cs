using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Folder : Panel_Popup
{
    public enum TYPE { TYPE_NONE, TYPE_GAMEZIP, TYPE_FILESAVE, TYPE_END } // �⺻ ���, ���� Zip, ���� ����
    public enum EVENT { EVENT_GAMEZIP, EVENT_END } // ���� Zip

    private string m_path;
    private List<FoldersData> m_foldersData;
    private List<bool> m_eventBool;

    private TMP_InputField m_pathText;     // ���� ���
    private ScrollRect m_scrollRect;       // ���� ��ũ��
    private FileInput m_fileInput;         // ���� �г� ��ũ��Ʈ
    private FolderBox m_folderBox;         // ������ ����
    private Transform m_folderTransform;   // ���� �г� Ʈ������
    private Transform m_favoriteTransform; // ���ã�� �г� Ʈ������
    private RectTransform m_lastInterval;  // ���� �г� �ϴ� ���� ��ƮƮ������

    public string Path => m_path;

    public FolderBox SelectFolderBox => m_folderBox; 
    public Transform FavoriteTransform => m_favoriteTransform;

    public Panel_Folder() : base()
    {
        m_fileType = WindowManager.FILETYPE.TYPE_FOLDER;

        m_eventBool = new List<bool>();
        for (int i = 0; i < (int)EVENT.EVENT_END; ++i)
            m_eventBool.Add(false);
    }

    protected override void Active_Event(bool active)
    {
        if(active == true)
        {
            // ���� â �ʱ�ȭ
            m_scrollRect.verticalNormalizedPosition = 1f;
            m_favoriteTransform.gameObject.SetActive(false);
            Reset_SelectBox();

            switch (m_activeType)
            {
                case (int)TYPE.TYPE_NONE: // �⺻ ��� ���� �б�
                    Set_WindowBackgroundData();
                    break;

                case (int)TYPE.TYPE_GAMEZIP: // zip ���� �ڽ� ���� �б�
                    m_isButtonClick = false;
                    m_pathText.enabled = false;

                    WindowFile file = GameManager.Ins.Window.Get_WindowFile(GameManager.Ins.Window.Get_FullFilePath(GameManager.Ins.Window.BackgroundPath, "Zip"), new WindowFileData(WindowManager.FILETYPE.TYPE_ZIP, "Zip"));
                    FolderData folderData = (FolderData)file.FileData.windowSubData;
                    List<FoldersData> foldersDatas = new List<FoldersData>();
                    foldersDatas.Add(new FoldersData(folderData.childFolders));
                    Set_FolderData("C:\\Users\\user\\Desktop\\Zip", foldersDatas);

                    // ���� �̺�Ʈ ����
                    if (m_eventBool[(int)EVENT.EVENT_GAMEZIP] == false)
                        Start_Event(EVENT.EVENT_GAMEZIP);
                    break;

                case (int)TYPE.TYPE_FILESAVE:
                    Set_WindowBackgroundData();                                               // ��� ���� �б�
                    m_fileInput.gameObject.SetActive(true);                                   // ���� ��ǲ �г� Ȱ��ȭ
                    m_lastInterval.sizeDelta = new Vector2(m_lastInterval.sizeDelta.x, 170f); // ���� ����

                    // ��ǲ �ʱ�ȭ
                    m_fileInput.NameField.text = "";
                    m_fileInput.PathField.text = GameManager.Ins.Window.BackgroundPath;
                    m_fileInput.PathField.enabled = false;
                    break;
            }
        }
        else
        {
            switch (m_activeType)
            {
                case (int)TYPE.TYPE_GAMEZIP:
                    m_isButtonClick = true;
                    m_pathText.enabled = true;

                    GameManager.Ins.Window.FileIconSlots.Add_FileIcon(WindowManager.FILETYPE.TYPE_NOVEL, "���Ͽ��� ����", () => GameManager.Ins.Window.WindowButton.Button_VisualNovel());
                    GameManager.Ins.Window.FileIconSlots.Add_FileIcon(WindowManager.FILETYPE.TYPE_WESTERN, "THE LEGEND COWBOY", () => GameManager.Ins.Window.WindowButton.Button_Western());
                    GameManager.Ins.Window.FileIconSlots.Add_FileIcon(WindowManager.FILETYPE.TYPE_HORROR, "THE HOSPITAL", () => GameManager.Ins.Window.WindowButton.Button_Horror());
                    break;

                case (int)TYPE.TYPE_FILESAVE:
                    m_fileInput.gameObject.SetActive(false);                                   // ���� �г� ��Ȱ��ȭ
                    m_lastInterval.sizeDelta = new Vector2(m_lastInterval.sizeDelta.x, 1.62f); // ���� ����
                    break;
            }
        }
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Folder/Panel_Folder", canvas.GetChild(3));
        m_object.SetActive(m_select);

        // ��ư �̺�Ʈ �߰�
        m_object.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => Putdown_Popup());
        m_object.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Active_Popup(false));

        #region �⺻ ����
        m_childPopup = new List<Panel_Popup>();
        m_childPopup.Add(GameManager.Ins.Window.FolderDelete);

        m_folderTransform = m_object.transform.GetChild(3).GetChild(0).GetChild(0);
        m_favoriteTransform = m_object.transform.GetChild(2).GetChild(0).GetChild(1);
        m_lastInterval = m_folderTransform.GetChild(m_folderTransform.childCount - 1).GetComponent<RectTransform>();
        m_pathText = m_object.transform.GetChild(2).GetChild(2).GetChild(1).GetComponent<TMP_InputField>();
        m_scrollRect = m_object.transform.GetChild(3).GetComponent<ScrollRect>();
        m_fileInput = m_object.transform.GetChild(4).GetComponent<FileInput>();
        m_fileInput.Start_FileInput();

        if (m_foldersData != null)
            Set_FolderData(m_path, m_foldersData);
        #endregion
    }

    #region
    #region ���� �̺�Ʈ
    public void Start_Event(EVENT type)
    {
        if (m_eventBool[(int)type] == true)
            return;

        m_eventBool[(int)type] = true;
        switch (type)
        {
            case EVENT.EVENT_GAMEZIP:
                GameManager.Ins.StartCoroutine(Destroy_GameIcon());
                break;
        }
    }

    private IEnumerator Destroy_GameIcon()
    {
        while (true)
        {
            m_inputPopupButton = false;
            yield return new WaitForSeconds(1.0f);

            m_scrollRect.verticalNormalizedPosition = 1f;
            m_scrollRect.gameObject.transform.GetChild(1).gameObject.GetComponent<Scrollbar>().enabled = false;

            int currentIndex = 0;
            int childCount = m_folderTransform.childCount;
            for (int i = 0; i < childCount; ++i)
            {
                GameObject obj = m_folderTransform.GetChild(currentIndex).gameObject;
                FolderBox folderBox = obj.GetComponent<FolderBox>();
                if (folderBox != null)
                {
                    int fileIndex = (int)folderBox.FileData.FileData.fileType;
                    if (fileIndex >= (int)WindowManager.FILETYPE.TYPE_BLACKOUT && fileIndex <= (int)WindowManager.FILETYPE.TYPE_THET || fileIndex == 18)
                    {
                        // ���� �ڽ� Ȱ��ȭ
                        folderBox.Set_ClickImage(FolderBox.BOXIMAGE.BT_DESTROY);
                        yield return new WaitForSeconds(0.5f);

                        // �Ͼ� ��ĭ���� ����
                        folderBox.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        folderBox.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        folderBox.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                        yield return new WaitForSeconds(0.25f);

                        // �׸� ����
                        m_foldersData[0].childFolders.RemoveAt(currentIndex - 1);
                        GameManager.Ins.Resource.Destroy(folderBox.gameObject);
                        yield return new WaitForSeconds(0.3f);
                    }
                    else
                    {
                        currentIndex++;
                    }
                }
                else
                {
                    currentIndex++;
                }
            }

            m_inputPopupButton = true;
            m_scrollRect.gameObject.transform.GetChild(1).gameObject.GetComponent<Scrollbar>().enabled = true;
            break;
        }
    }
    #endregion

    #region ����
    private void Set_FolderPath(string path) // ���� ��� ����
    {
        m_path = path;
        m_pathText.text = m_path;
    }

    private void Remove_FolderData() // ���� ������ ����
    {
        int childCount = m_folderTransform.childCount - 1;
        for (int i = 1; i < childCount; i++)
            GameManager.Ins.Resource.Destroy(m_folderTransform.GetChild(i).gameObject);
    }

    public void Set_FolderData(string path, List<FoldersData> foldersDatas) // ���� ���� ����
    {
        // ��� ����
        Set_FolderPath(path);

        // �ڽ� ����
        Remove_FolderData();

        // �г� ����
        m_foldersData = foldersDatas;
        Create_FileList(path);
    }

    private void Set_WindowBackgroundData() // ��� ���� ���� ����
    {
        // ��� ����
        string path = GameManager.Ins.Window.BackgroundPath;
        Set_FolderPath(path);

        // �ڽ� ����
        Remove_FolderData();

        // �г� ����
        m_foldersData = new List<FoldersData>();
        m_foldersData.Add(new FoldersData(GameManager.Ins.Window.FileIconSlots.Get_WindowFileData()));
        Create_FileList(path);
    }

    private void Create_FileList(string path) // ���� �����Ϳ� ���� ���� ����
    {
        int count = m_foldersData[0].childFolders.Count;
        for (int i = 0; i < count; ++i)
            Create_File(path, m_foldersData[0].childFolders[i]);
    }

    public void Create_File(string path, WindowFileData windowFileData) // ���� ���� ����
    {
        GameObject obj = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Folder/Folders/Folder_FileList", m_folderTransform);
        if (obj != null)
        {
            WindowFile file = GameManager.Ins.Window.Get_WindowFile(GameManager.Ins.Window.Get_FullFilePath(path, windowFileData.fileName), windowFileData);

            obj.transform.SetSiblingIndex(m_folderTransform.childCount - 2);
            FolderBox folderbox = obj.GetComponent<FolderBox>();
            if (folderbox != null)
                folderbox.Set_FolderBox(file);
        }
    }
    #endregion

    #region ���� ����
    public void Set_SelectBox(FolderBox box)
    {
        if (box == null || m_folderBox == box)
            return;

        Reset_SelectBox();

        m_folderBox = box;
        m_folderBox.Set_ClickImage(FolderBox.BOXIMAGE.BT_BASIC);
    }

    public void Reset_SelectBox()
    {
        if (m_folderBox == null)
            return;

        m_folderBox.Set_ClickImage(FolderBox.BOXIMAGE.BI_NONE);
        m_folderBox = null;
    }
    #endregion
    #endregion
}
