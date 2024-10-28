using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolderButton : MonoBehaviour
{
    public void Update()
    {
        if (GameManager.Ins == null || GameManager.Ins.Window.FOLDER.SelectFolderBox == null)
            return;

        // ��Ʈ�� C/ V ����Ʈ �߶󳻱� �ٿ��ֱ� ���� ���� �߶󳻱� �ٿ��ֱ�� �Ҽ������� �ϱ� // ������1, 2 �̷��� ����
        if(Input.GetKeyDown(KeyCode.C))
        {

        }
        else if (Input.GetKeyDown(KeyCode.C))
        {

        }
    }

    public void Active_Favorite()
    {
        // ���ã�� ��� Ȱ��ȭ
        Transform favoriteTr = GameManager.Ins.Window.FOLDER.FavoriteTransform;
        if (favoriteTr.gameObject.activeSelf == true) // ���ã�� ��� ��Ȱ��ȭ
        {
            favoriteTr.gameObject.SetActive(false);
            return;
        }

        Update_Favorite();
        favoriteTr.gameObject.SetActive(true);
    }


    public void Create_Folder() // ���� �����ϱ�
    {
        // �����ϱ� ������ ���� ������ �ȿ� ���� �ϴܿ� �� ������ ����鼭 �̸� �Է¹ޱ�
        // ���� ��ο� ���� �߰�
           // ����ȭ���̸� ������ �߰�
    }

    public void Delete_Folder() // ����(����) �����ϱ�
    {
        // ���� ���� �� �����ϱ� ������ �˾�â Ȱ��ȭ
    }

    public void Favorite_Folder() 
    {
        if (GameManager.Ins == null || GameManager.Ins.Window.FOLDER.SelectFolderBox == null)
            return;

        // ���� ���ã��
        FolderBox selectBox = GameManager.Ins.Window.FOLDER.SelectFolderBox;
        selectBox.FileData.Favorite = !selectBox.FileData.Favorite;
        selectBox.Set_Favorite();

        Update_Favorite();
    }


    public void Button_Back() // �ڷΰ���
    {

    }

    public void Button_Again() // �����ΰ���
    {

    }

    private void Update_Favorite()
    {
        Transform favoriteTr = GameManager.Ins.Window.FOLDER.FavoriteTransform;

        // 0�� �ڽ� ������ �ڽ� ����
        for (int i = favoriteTr.childCount - 1; i > 0; i--)
            Destroy(favoriteTr.GetChild(i).gameObject);

        GameObject prefab = GameManager.Ins.Resource.Load<GameObject>("5. Prefab/0. Window/UI/Folder/Button_Popup_Bookmark");
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
                        bookmark.Set_Bookmark(file.Value.FileData);
                }
            }
        }
    }
}
