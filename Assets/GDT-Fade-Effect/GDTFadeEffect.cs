using UnityEngine;
using UnityEngine.UI;

/**
 * Script and package by GDT Solution ES.
 * https://www.youtube.com/watch?v=77YBCXTfM0o
 */
public class GDTFadeEffect : MonoBehaviour
{
    public bool playOnAwake = true;
    public Color firstColor;
    public Color lastColor;
    public float timeEffect;
    public float initialDelay;
    public bool firstToLast=true;
    public bool pingPong;
    public float pingPongDelay;
    public bool disableWhenFinish=true;
    public float disableDelay;
    private float speed;
    private Image blackImage;
    private float currentValue;
    private bool performEffect=false;
    private bool finished = false;
    private bool halfCycle;
    private bool goingToLast;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        halfCycle = false;
        speed = 1 / timeEffect;
        goingToLast = firstToLast;
        if (blackImage == null)
        {
            blackImage = GetComponent<Image>();
        }
        if (firstToLast)
        {
            currentValue = 0f;
            blackImage.color = firstColor;
        }
        else
        {
            currentValue = 1f;
            blackImage.color = lastColor;
        }
        if (playOnAwake)
        {
            if (!performEffect)
            {
                Invoke("StartEffect", initialDelay);
            }
        }
        
    }

    void FixedUpdate()
    {
        if (performEffect)
        {
            if (pingPong)
            {
                if (!halfCycle)
                {
                    if (goingToLast)
                    {
                        if (PerformFadeIn())
                        {
                            Invoke("HalfCycleDelay", pingPongDelay);
                        }
                    }
                    else
                    {
                        if (PerformFadeOut())
                        {
                            Invoke("HalfCycleDelay", pingPongDelay);
                        }
                    }
                }
                else
                {
                    if (goingToLast)
                    {
                        if (PerformFadeIn())
                        {
                            performEffect = false;
                            finished = true;
                        }
                    }
                    else
                    {
                        if (PerformFadeOut())
                        {
                            finished = true;
                        }
                    }
                }
            }
            else
            {
                if (goingToLast)
                {
                    if (PerformFadeIn())
                    {
                        finished = true;
                    }

                }
                else
                {
                    if (PerformFadeOut())
                    {
                        finished = true;
                    }
                }
            }

            blackImage.color = Color.Lerp(firstColor, lastColor, currentValue);
            if (finished)
            {
                performEffect = false;
                if (disableWhenFinish)
                {
                    Invoke("Disable", disableDelay);
                }
            }
        }

    }

    private bool PerformFadeIn()
    {
        if (currentValue != 1f)
        {
            currentValue += speed * Time.deltaTime;
            if (currentValue > 1f)
            {
                currentValue = 1f;
                return true;
            }
        }
        return false;
    }

    private bool PerformFadeOut()
    {
        if (currentValue != 0f)
        {
            currentValue -= speed * Time.deltaTime;
            if (currentValue < 0f)
            {
                currentValue = 0f;
                return true;
            }
        }
        return false;
    }

    private void Disable()
    {
        Debug.Log("Fade Disabled");
        gameObject.SetActive(false);
    }

    private void HalfCycleDelay()
    {
        halfCycle = true;
        goingToLast = !goingToLast;
    }

    public bool HasFinished()
    {
        return performEffect;
    }

    public bool IsPingPongInHalfCycle()
    {
        return halfCycle;
    }

    public void StartEffect()
    {
        performEffect = true;
        finished = false;
    }

    /** <summary>
        Calculates the total duration of the current fade effect.
        This takes into account whether there is a pingpong or not.
        <summary>
    */
    public float CalculateFadeDuration()
    {
        /** To calculate the total duration, we only care about:
          * Whether it pingpongs,
          * the initial delay, the time effect, and the pingpong delay duration.
          */
        float totalDuration = initialDelay + timeEffect;
        if (pingPong)
        {
            totalDuration += pingPongDelay + timeEffect;
        }
        return totalDuration;
    }

}
