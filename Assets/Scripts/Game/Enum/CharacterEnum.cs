using System.Collections.Generic;

/**
 * This is the enum class for dialogue character name.
 * Make sure the names here are as how you would want the character's name to be shown in the dialogue box.
 */
public class CharacterEnum
{
    public enum Character
    {
        None,
        Mei,
        Students,
        Unknown,
        Chihaya,
        Haru,
        DarkMei,
        StudentA,
        StudentB,
    }

    /**
     * The dialogue system refers to this dictionary for the displayed character names.
     * Make sure to update this alongside the enum.
     */
    public static readonly Dictionary<Character, string> CHARACTER_TO_STRING = new()
    {
        { Character.None, "" },
        { Character.Mei, "Mei" },
        { Character.Students, "Students" },
        { Character.Unknown, "???" },
        { Character.Chihaya, "Chihaya" },
        { Character.Haru, "Haru" },
        { Character.DarkMei, "Dark Mei" },
        { Character.StudentA, "Student A" },
        { Character.StudentB, "Student B" },
    };
}

