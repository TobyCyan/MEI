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
        None,
        Mei_Despair,
        Mei_Neutral,
        Mei_Smiling_1,
        Mei_Smiling_2,
        Mei_Worried,
        Mei_Crying,
        Mei_Surprised_1,
        Mei_Surprised_2,
    }
}
