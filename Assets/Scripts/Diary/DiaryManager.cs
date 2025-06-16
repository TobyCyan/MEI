using System.Collections.Generic;
using UnityEngine;

public class DiaryManager : MonoBehaviour
{
    [SerializeField] private DiaryEntryDataBase _diaryEntryDataBase;
    private DiaryUi _diaryUi;
    private int _currentPageIndex = 0;
    private bool _isDiaryOpened = false;

    private void Awake()
    {
        _diaryUi = GetComponentInChildren<DiaryUi>();
    }

    private void Start()
    {
        _diaryUi.gameObject.SetActive(false);
    }

    public bool AddEntry(int id)
    {
        return _diaryEntryDataBase.AddEntry(id);
    }

    public DiaryEntry GetDiaryEntryById(int id)
    {
        return _diaryEntryDataBase.GetDiaryEntryById(id);
    }

    public DiaryEntry GetDiaryEntryByIndex(int index)
    {
        return _diaryEntryDataBase.GetDiaryEntryByIndex(index);
    }

    public List<DiaryEntry> GetAllFoundEntries()
    {
        return _diaryEntryDataBase.GetAllFoundEntries();
    }

    public void NextPage()
    {
        int nextPageIndex = _currentPageIndex + 1;
        if (!_diaryEntryDataBase.IsIndexValid(nextPageIndex))
        {
            return;
        }

        bool isEntryFound = _diaryEntryDataBase.CheckIsEntryFoundByIndex(nextPageIndex);
        if (!isEntryFound)
        {
            return;
        }

        int entryId = _diaryEntryDataBase.GetDiaryEntryId(nextPageIndex);
        LoadPage(entryId);
        _currentPageIndex = nextPageIndex;
    }

    public void PrevPage()
    {
        int prevPageIndex = _currentPageIndex - 1;
        if (!_diaryEntryDataBase.IsIndexValid(prevPageIndex))
        {
            return;
        }
        
        bool isEntryFound = _diaryEntryDataBase.CheckIsEntryFoundByIndex(prevPageIndex);
        if (!isEntryFound)
        {
            return;
        }

        int entryId = _diaryEntryDataBase.GetDiaryEntryId(prevPageIndex);
        LoadPage(entryId);
        _currentPageIndex = prevPageIndex;
    }

    public void LoadPage(int id)
    {
        DiaryEntry diaryEntry = GetDiaryEntryById(id);
        _diaryUi.LoadPage(diaryEntry.Title, diaryEntry.Content);
    }

    public void OpenDiary()
    {
        if (_isDiaryOpened)
        {
            return;
        }
        _isDiaryOpened = true;
        _diaryUi.gameObject.SetActive(true);
        LoadPage(0);
    }

    public void CloseDiary()
    {
        if (!_isDiaryOpened)
        {
            return;
        }
        _isDiaryOpened = false;
        _diaryUi.gameObject.SetActive(false);
    }
}
