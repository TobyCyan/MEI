using UnityEngine;

[System.Serializable]
public class DiaryEntry
{
    public int Id = -1;
    public string Title;
    public string Content;
    public Sprite Doodle;
    public bool IsFound = false;
}
