using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DialogueInfoStruct
{
    [SerializeField] public string text;
    [SerializeField] public EmotionEnum.Emotion emotion;
    [SerializeField] public CharacterEnum.Character character;
}
