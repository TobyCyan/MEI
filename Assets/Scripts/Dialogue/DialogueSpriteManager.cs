using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Attach this script to the dialogue canvas.
 * Make sure to attach the Avatar image too.
 */
public class DialogueSpriteManager : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Dictionary<string, Sprite> _spriteDict = new();
    private readonly Dictionary<CharacterEnum.Character, Sprite[]> _characterNameToSpritesDict = new();

    void Awake()
    {
        _characterNameToSpritesDict[CharacterEnum.Character.Mei] = Resources.LoadAll<Sprite>("Avatars/Mei");
        _characterNameToSpritesDict[CharacterEnum.Character.DarkMei] = Resources.LoadAll<Sprite>("Avatars/DarkMei");
        LoadSpritesIntoSpriteDict(_characterNameToSpritesDict);
    }

    /**
     * Replaces the dialogue character sprite with the one passed in.
     */
    public void ActivateDialogueSprite(EmotionEnum.Emotion emotion)
    {
        if (emotion == EmotionEnum.Emotion.None)
        {
            _image.enabled = false;
            return;
        }
        _image.enabled = true;
        _image.sprite = _spriteDict[emotion.ToString()];
    }

    private void LoadSpritesIntoSpriteDict(Dictionary<CharacterEnum.Character, Sprite[]> characterToSprites)
    {
        foreach (Sprite[] sprites in characterToSprites.Values)
        {
            foreach (Sprite sprite in sprites)
            {
                if (sprite != null)
                {
                    _spriteDict[sprite.name] = sprite;
                }
            }
        }
    }
}
