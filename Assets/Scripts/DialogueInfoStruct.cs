using System;
using UnityEngine;

/**
 * This is the struct to store dialogue information.
 * It contains the dialogue text, the character emotion sprite name, and the character name.
 * These information will be read by the dialogue activation scripts to assign the correct values to their respective game objects.
 * Make sure that the values serialized here are according to their requirements from their enums.
 */
[Serializable]
public struct DialogueInfoStruct
{
    [SerializeField] public string text;
    [SerializeField] public EmotionEnum.Emotion emotion;
    [SerializeField] public CharacterEnum.Character character;
}
