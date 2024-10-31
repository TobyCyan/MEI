using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        sprites = Resources.LoadAll<Sprite>("Mei");
        LoadSpritesIntoSpriteDict(sprites);
    }

    public void ActivateDialogueSprite(EmotionEnum.Emotion emotion)
    {
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
