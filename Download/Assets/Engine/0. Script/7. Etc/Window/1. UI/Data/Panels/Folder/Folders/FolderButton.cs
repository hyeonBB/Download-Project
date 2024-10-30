using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolderButton : MonoBehaviour
{
    #region Ű���� �Է� (�߶󳻱�/ �ٿ��ֱ�)
    public void Update()
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Folder.IsButtonClick == false || WM.Folder.SelectFolderBox == null)
            return;

        // ��Ʈ�� C/ V ����Ʈ �߶󳻱� �ٿ��ֱ� ���� ���� �߶󳻱� �ٿ��ֱ�� �Ҽ������� �ϱ� // ������1, 2 �̷��� ����
        if(Input.GetKeyDown(KeyCode.C))
        {

        }
        else if (Input.GetKeyDown(KeyCode.C))
        {

        }
    }
    #endregion

    #region ���ã��
    public void Active_Favorite() // ���ã�� ��/Ȱ��ȭ ��ư
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Folder.IsButtonClick == false)
            return;

        // ���ã�� ��� ��/ Ȱ��ȭ
        Transform favoriteTr = WM.Folder.FavoriteTransform;
        if (favoriteTr.gameObject.activeSelf == false)
            Update_Favorite();
        favoriteTr.gameObject.SetActive(!favoriteTr.gameObject.activeSelf);
    }

    public void Favorite_Folder() // ���� ���ã��
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Folder.IsButtonClick == false || WM.Folder.SelectFolderBox == null)
            return;

        // ���� ���ã��
        FolderBox selectBox = WM.Folder.SelectFolderBox;
        selectBox.FileData.Favorite = !selectBox.FileData.Favorite;
        selectBox.Set_Favorite();

        // ���ã�� �г� ������Ʈ
        Update_Favorite();
    }

    private void Update_Favorite()
    {
        Transform favoriteTr = GameManager.Ins.Window.Folder.FavoriteTransform;

        // 0�� �ڽ� ������ �ڽ� ����
        for (int i = favoriteTr.childCount - 1; i > 0; i--)
            Destroy(favoriteTr.GetChild(i).gameObject);

        GameObject prefab = GameManager.Ins.Resource.Load<GameObject>("5. Prefab/0. Window/UI/Folder/Folders/Button_Popup_Bookmark");
        Dictionary<string, WindowFile> filedatas = GameManager.Ins.Window.FileData;
        foreach (var file in filedatas)
        {
            if (file.Value.Favorite == true)
            {
                GameObject newFavorite = GameManager.Ins.Resource.Create(prefab, favoriteTr);
                if (newFavorite != null)
                {
                    FolderBookmark bookmark = newFavorite.GetComponent<FolderBookmark>();
                    if (bookmark != null)
                        bookmark.Set_Bookmark(file.Value);
                }
            }
        }
    }
    #endregion

    #region ���� ����/ ����
    public void Create_Folder() // ���� ��ο� ���� �����ϱ�
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Folder.IsButtonClick == false)
            return;

        Panel_Folder folder = WM.Folder;

        #region ���� �̸� ���� : ���� ��� + �̸� ��� ���� ���� ���� �˻�
        string fileName = "�� ����";
        int fileCount = 1;
        string fullPath = WM.Get_FullFilePath(folder.Path, fileName);
        while (WM.Check_File(fullPath) == true) // ���� �̸� �ߺ��� ��
        {
            fileCount++;

            fileName = $"�� ���� ({fileCount})"; // �� ���� (2), �� ���� (3) ...
            fullPath = WM.Get_FullFilePath(folder.Path, fileName);
        }
        #endregion

        #region ���� ����ü ����
        WindowFileData windowFileData = new WindowFileData();
        windowFileData.fileType = WindowManager.FILETYPE.TYPE_FOLDER;
        windowFileData.fileName = fileName;
        #endregion

        #region ����
        if (folder.Path == WM.BackgroundPath) // ���� ��ΰ� ����ȭ���� ��
        {
            // ����ȭ�� ������ �߰�
            WM.FileIconSlots.Add_FileIcon(windowFileData.fileType, windowFileData.fileName);

        }
        else
        {
            // ���� ��� ���� �θ� ������ �ڽ� ����Ʈ�� �߰�
            WindowFile parentfile = WM.Get_WindowFile(folder.Path, windowFileData);
            parentfile.Add_ChildFile(windowFileData);
        }
        // ������ ���� �߰�
        folder.Create_File(folder.Path, windowFileData);
        // �ش� ���� �׼� �߰�
        WindowFile file = WM.Get_WindowFile(fullPath, windowFileData);
        file.Set_FileAction(() => WM.Folder.Active_Popup(true));
        #endregion

        // ���� Ŭ�� �� �̸� ���氡��
        //*
    }

    public void Delete_Folder() 
    {
        // ����(����) �����ϱ�
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Folder.IsButtonClick == false || WM.Folder.SelectFolderBox == null)
            return;

        // ������ �� �ִ� �������� �˻�
        //*

        GameManager.Ins.Window.FolderDelete.Set_FileDelete(WM.Folder.SelectFolderBox.FileData);
        GameManager.Ins.Window.FolderDelete.Active_ChildPopup(true);
    }
    #endregion

    #region �ڷΰ���/ ������ ����
    public void Button_Back() // �ڷΰ���
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Folder.IsButtonClick == false)
            return;


    }

    public void Button_Again() // �����ΰ���
    {
        WindowManager WM = GameManager.Ins.Window;
        if (WM.Folder.IsButtonClick == false)
            return;


    }
    #endregion

    #region �ּ�

    #endregion
}
