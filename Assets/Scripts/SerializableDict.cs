using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * A custom serializable dictionary for storing the dialogue text and its corresponding dialogue sprite emotion as the value.
 */
[Serializable]
public class SerializableDialogueDict
{
    [SerializeField] public SerializableDialogueDictItem[] dialogueItems;

    public Dictionary<string, EmotionEnum.Emotion> ToDict()
    {
        // Loads all the pairs from the inspector into a dictionary and returns it.
        Dictionary<string, EmotionEnum.Emotion> newDict = new Dictionary<string, EmotionEnum.Emotion>();
        foreach (var item in dialogueItems)
        {
            newDict[item.text] = item.emotion;
        }
        return newDict;
    }
}

/**
 * A serializable dialogue dictionary item with string as the key and EmotionEnum.Emotion as the value.
 */
[Serializable]
public class SerializableDialogueDictItem
{
    [SerializeField] public string text;
    [SerializeField] public EmotionEnum.Emotion emotion;
}