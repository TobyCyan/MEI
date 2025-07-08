using System.Collections;
using UnityEngine;

public class OpenDiaryInteractable : Interactable
{
    [SerializeField] private DiaryManager _diaryManager;
    [Header("Entry ID Should Match The Pick Up Diary Entry If Used With DiaryEntryPickUp")]
    [SerializeField] private int _openEntryId = 0;

    void Start()
    {
        Debug.Assert(_diaryManager != null, "Diary Manager Not Attached To: " + name);
    }

    public override IEnumerator Interact()
    {
        _diaryManager.OpenDiary(_openEntryId);
        yield return new WaitUntil(() => !_diaryManager.IsDiaryOpened());
    }
}
