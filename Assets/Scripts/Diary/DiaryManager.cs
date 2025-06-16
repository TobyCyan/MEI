using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DiaryManager : MonoBehaviour
{
    [SerializeField] private DiaryEntryDataBase _diaryEntryDataBase;
    private DiaryUi _diaryUi;
    private int _currentPageIndex = 0;
    private bool _isDiaryOpened = false;

    private void Awake()
    {
        Assert.IsNotNull(_diaryEntryDataBase, "Diary Entry DataBase Is Not Assigned To " + name + "!");
        _diaryEntryDataBase = Instantiate(_diaryEntryDataBase);
        _diaryUi = GetComponentInChildren<DiaryUi>();
        _diaryEntryDataBase.UpdateFoundEntries();
    }

    private void Start()
    {
        _diaryUi.gameObject.SetActive(false);
    }

    public bool AddEntry(int id)
    {
        return _diaryEntryDataBase.AddEntry(id);
    }

    public bool IsEntryFound(int id)
    {
        return _diaryEntryDataBase.CheckIsEntryFoundById(id);
    }

    public DiaryEntry GetDiaryEntryById(int id)
    {
        return _diaryEntryDataBase.GetDiaryEntryById(id);
    }

    public void NextPage()
    {
        int nextPageIndex = _currentPageIndex + 1;
        if (!_diaryEntryDataBase.IsFoundEntryIndexValid(nextPageIndex))
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
        if (!_diaryEntryDataBase.IsFoundEntryIndexValid(prevPageIndex))
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
