using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileIconSlots : WindowData
{
    private int m_slotCount = 72;
    private FileIconSlot[][] m_files;

    public FileIconSlots()
    {
        m_files = new FileIconSlot[6][]; // 6줄
        for (int i = 0; i < m_files.Length; ++i)
            m_files[i] = new FileIconSlot[12]; // 12칸

        int RowIndex = 0;
        int ColumnIndex = 0;
        for (int i = 0; i < m_slotCount; ++i)
        {
            m_files[RowIndex][ColumnIndex] = new FileIconSlot();
            ColumnIndex++;

            if (ColumnIndex >= 12)
            {
                RowIndex++;
                ColumnIndex = 0;
            }
        }
    }

    public override void Load_Scene()
    {
        int RowIndex = 0;
        int ColumnIndex = 0;
        for (int i = 0; i < m_slotCount; ++i)
        {
            m_files[RowIndex][ColumnIndex].Load_Scene();
            ColumnIndex++;

            if (ColumnIndex >= 12)
            {
                RowIndex++;
                ColumnIndex = 0;
            }
        }
    }

    public override void Update_Data()
    {
    }

    public override void Unload_Scene()
    {
        int RowIndex = 0;
        int ColumnIndex = 0;
        for (int i = 0; i < m_slotCount; ++i)
        {
            m_files[RowIndex][ColumnIndex].Unload_Scene();
            ColumnIndex++;

            if (ColumnIndex >= 12)
            {
                RowIndex++;
                ColumnIndex = 0;
            }
        }
    }

    public void Add_FileIcon(int rowIndex, int columnIndex, WindowManager.FILETYPE fileType, string fileName, Action action = null, WindowFileDataSubData subData = null, string prevfilePath = "") // 위치 지정 아이콘 추가
    {
        if (m_files[rowIndex][columnIndex].Empty == false)
            return;

        // 생성 또는 할당
        WindowFile file = GameManager.Ins.Window.Get_WindowFile(GameManager.Ins.Window.Get_FullFilePath(GameManager.Ins.Window.BackgroundPath, fileName), new WindowFileData(fileType, fileName, action, subData, prevfilePath));

        m_files[rowIndex][columnIndex].Add_FileIcon(file);
    }
    
    public bool Add_FileIcon(WindowManager.FILETYPE fileType, string fileName, Action action = null, WindowFileDataSubData subData = null, string prevfilePath = "") // 위치 미지정 아이콘 추가
    {
        // 경로 중복 검사
        string path = GameManager.Ins.Window.Get_FullFilePath(GameManager.Ins.Window.BackgroundPath, fileName);
        if (GameManager.Ins.Window.Check_File(path) == true)
            return false;

        int RowIndex = 0;
        int ColumnIndex = 0;
        for (int i = 0; i < m_slotCount; ++i)
        {
            FileIconSlot slot = m_files[RowIndex][ColumnIndex];
            if (slot.Empty == true)
            {
                Add_FileIcon(RowIndex, ColumnIndex, fileType, fileName, action, subData, prevfilePath);
                return true;
            }

            RowIndex++;
            if (RowIndex >= 6)
            {
                ColumnIndex++;
                RowIndex = 0;
            }
        }

        return false;
    }

    public List<WindowFileData> Get_WindowFileData() // 바탕화면에 존재하는 파일 세로로 정보 읽기
    {
        List<WindowFileData> windowFileData = new List<WindowFileData>();

        int RowIndex = 0;
        int ColumnIndex = 0;
        for (int i = 0; i < m_slotCount; ++i)
        {
            FileIconSlot slot = m_files[RowIndex][ColumnIndex];
            if (slot.Empty == false)
                windowFileData.Add(slot.File.FileData);

            RowIndex++; // ColumnIndex++;
            if (RowIndex >= 6) // if (ColumnIndex >= 12)
            {
                ColumnIndex++; // RowIndex++;
                RowIndex = 0; // ColumnIndex = 0;
            }
        }

        return windowFileData;
    }

    public void Remove_FileIcon(string path)
    {
        int RowIndex = 0;
        int ColumnIndex = 0;
        for (int i = 0; i < m_slotCount; ++i)
        {
            FileIconSlot slot = m_files[RowIndex][ColumnIndex];
            if (slot.Empty == false)
            {
                if(slot.File.FilePath == path)
                {
                    slot.Remove_FileIcon();
                    return;
                }
            }

            RowIndex++;
            if (RowIndex >= 6)
            {
                ColumnIndex++;
                RowIndex = 0;
            }
        }
    }
}
