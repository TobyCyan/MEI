using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Diary/Diary Entry Database")]
public class DiaryEntryDataBase : ScriptableObject
{
    [SerializeField] private List<DiaryEntry> _diaryEntries = new();

    public List<DiaryEntry> GetAllFoundEntries()
    {
        return _diaryEntries.FindAll(entry => entry.IsFound);
    }

    public bool AddEntry(int id)
    {
        bool isContainEntry = IsContainEntry(id);
        bool isEntryFound = CheckIsEntryFoundById(id);
        bool CanBeAdded = isContainEntry && !isEntryFound;
        if (CanBeAdded)
        {
            GetDiaryEntryById(id).IsFound = true;
        }
        return CanBeAdded;
    }

    private bool IsContainEntry(int id)
    {
        return GetDiaryEntryById(id) != null;
    }

    private bool CheckIsEntryFoundById(int id)
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
}
