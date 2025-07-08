using UnityEngine;
using UnityEngine.Assertions;

public class DiaryManager : MonoBehaviour
{
    [SerializeField] private DiaryEntryDataBase _diaryEntryDataBase;
    // The diary entry database used for the current session.
    private static DiaryEntryDataBase _diaryEntryDataBaseCopy;
    private DiaryUi _diaryUi;
    private int _currentPageIndex = 0;
    private bool _isDiaryOpened = false;

    private void Awake()
    {
        if (_diaryEntryDataBaseCopy == null)
        {
            Assert.IsNotNull(_diaryEntryDataBase, "Diary Entry DataBase Is Not Assigned To " + name + "!");
            _diaryEntryDataBaseCopy = Instantiate(_diaryEntryDataBase);
        }
        
        _diaryUi = GetComponentInChildren<DiaryUi>();
        _diaryEntryDataBaseCopy.UpdateFoundEntries();
    }

    private void Start()
    {
        _diaryUi.gameObject.SetActive(false);
    }

    public bool AddEntry(int id)
    {
        return _diaryEntryDataBaseCopy.AddEntry(id);
    }

    public bool IsEntryFound(int id)
    {
        return _diaryEntryDataBaseCopy.CheckIsEntryFoundById(id);
    }

    public DiaryEntry GetDiaryEntryById(int id)
    {
        return _diaryEntryDataBaseCopy.GetDiaryEntryById(id);
    }

    public void NextPage()
    {
        int nextPageIndex = _currentPageIndex + 1;
        if (!_diaryEntryDataBaseCopy.IsFoundEntryIndexValid(nextPageIndex))
        {
            return;
        }

        int entryId = _diaryEntryDataBaseCopy.GetFoundDiaryEntryId(nextPageIndex);
        LoadPage(entryId);
        _currentPageIndex = nextPageIndex;
    }

    public void PrevPage()
    {
        int prevPageIndex = _currentPageIndex - 1;
        if (!_diaryEntryDataBaseCopy.IsFoundEntryIndexValid(prevPageIndex))
        {
            return;
        }

        int entryId = _diaryEntryDataBaseCopy.GetFoundDiaryEntryId(prevPageIndex);
        LoadPage(entryId);
        _currentPageIndex = prevPageIndex;
    }

    public void LoadPage(int id)
    {
        DiaryEntry diaryEntry = GetDiaryEntryById(id);
        _diaryUi.LoadPage(diaryEntry.Title, diaryEntry.Content, diaryEntry.Doodle);
    }

    public void OpenDiary(int entryId)
    {
        if (_isDiaryOpened)
        {
            return;
        }
        _isDiaryOpened = true;
        _diaryUi.gameObject.SetActive(true);
        _currentPageIndex = _diaryEntryDataBaseCopy.GetFoundDiaryEntryIndexById(entryId);
        LoadPage(entryId);
    }

    public void OpenDiary()
    {
        if (_isDiaryOpened)
        {
            return;
        }
        _isDiaryOpened = true;
        _diaryUi.gameObject.SetActive(true);
        _currentPageIndex = 0;
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

    public bool IsDiaryOpened() => _isDiaryOpened;
}
