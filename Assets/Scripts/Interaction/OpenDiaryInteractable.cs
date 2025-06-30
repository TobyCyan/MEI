using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class OpenDiaryInteractable : Interactable
{
    [SerializeField] private DiaryManager _diaryManager;
    [Header("Entry ID Should Match The Pick Up Diary Entry If Used With DiaryEntryPickUp")]
    [SerializeField] private int _openEntryId = 0;

    void Start()
    {
        Assert.IsNotNull(_diaryManager, "Diary Manager Not Attached To: " + name);
    }

    public override IEnumerator Interact()
    {
        _diaryManager.OpenDiary(_openEntryId);
        yield break;
    }
}
