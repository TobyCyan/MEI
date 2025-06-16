using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(menuName = "Diary/Diary Entry Database")]
public class DiaryEntryDataBase : ScriptableObject
{
    [SerializeField] private List<DiaryEntry> _diaryEntries = new();
    private List<DiaryEntry> _foundDiaryEntries = new();

    public List<DiaryEntry> GetAllFoundEntries()
    {
        return _diaryEntries.FindAll(entry => entry.IsFound);
    }

    public bool AddEntry(int id)
    {
        Assert.IsTrue(CheckIsEntryExistById(id), 
            "Diary Entry With ID " + id + " Doesn't Exist In The DataBase!");
        bool isEntryFound = CheckIsEntryFoundById(id);
        if (!isEntryFound)
        {
            GetDiaryEntryById(id).IsFound = true;
            UpdateFoundEntries();
            return true;
        }
        return false;
    }

    public bool CheckIsEntryExistById(int id)
    {
        return GetDiaryEntryById(id) != null;
    }

    public bool CheckIsEntryFoundById(int id)
    {
        return GetDiaryEntryById(id).IsFound;
    }

    public bool CheckIsEntryFoundByIndex(int index)
    {
        return GetDiaryEntryByIndex(index).IsFound;
    }

    public DiaryEntry GetDiaryEntryById(int id)
    {
        return _diaryEntries.Find(entry => entry.Id == id);
    }

    public DiaryEntry GetDiaryEntryByIndex(int index)
    {
        return _diaryEntries[index];
    }

    public int GetDiaryEntryId(int index)
    {
        return GetDiaryEntryByIndex(index).Id;
    }

    public bool IsFoundEntryIndexValid(int index)
    {
        return index >= 0 && index < _foundDiaryEntries.Count;
    }

    public void UpdateFoundEntries()
    {
        _foundDiaryEntries = GetAllFoundEntries();
    }
}
