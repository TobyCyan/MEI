using UnityEngine;

/**
 * This is the enum class for dialogue sprite emotions.
 * Make sure the names here are same as the dialogue sprite names.
 * Otherwise it won't work as the dictionary reads the values of the sprites using their actual file names.
 */
public class EmotionEnum : MonoBehaviour
{
    public enum Emotion
    {
        Mei_Despair,
        Mei_Neutral,
        Mei_Smiling1,
        Mei_Smiling2,
        Mei_Worried,
    }
}
