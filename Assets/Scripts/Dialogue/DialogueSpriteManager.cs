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
    [SerializeField] private Dictionary<string, Sprite> _spriteDict = new Dictionary<string, Sprite>();
    private Sprite[] sprites;

    void Awake()
    {
        sprites = Resources.LoadAll<Sprite>("Avatars/Mei");
        LoadSpritesIntoSpriteDict(sprites);
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

    private void LoadSpritesIntoSpriteDict(Sprite[] sprites)
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
