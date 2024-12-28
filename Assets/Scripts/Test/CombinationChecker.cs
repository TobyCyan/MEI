using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationChecker : MonoBehaviour
{
    // Start is called before the first frame update
    int[] lockValue = new int[] { 0, 0, 0, 0 };
    [SerializeField] int[] correctCombination = new int[] { 0, 0, 0, 0 };

    void Start()
    {
        lockValue[0] = 0;
        lockValue[1] = 0;
        lockValue[2] = 0;
        lockValue[3] = 0;
        Padlock.displayed += checkResult;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void checkResult(string digitPlace, int number) 
    {
        switch (digitPlace)
        {
            case "combi0":
                lockValue[0] = number; 
                break;

            case "combi1":
                lockValue[1] = number;
                break;

            case "combi2":
                lockValue[2] = number;
                break;

            case "combi3":
                lockValue[3] = number;
                break;
                
            default:
                Debug.Log("digitPlace or number cannot be identified. " +
                    "Place value should be between 0 to 3 and number should be between 0 to 9.");
                break;
        }

        if (lockValue[0] == correctCombination[0] && lockValue[1] == correctCombination[1] 
            && lockValue[2] == correctCombination[2] && lockValue[3] == correctCombination[3]) 
        {
            Debug.Log("opnened");
        }

    }
}
