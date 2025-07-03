using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingEnemy : Enemy
{
    public bool IsWalkingSoundHeard { get; set; }

    public void SetWalkingSoundHeardStatus(bool isWalkingSoundHeard)
    {
        IsWalkingSoundHeard = isWalkingSoundHeard;
    }
}
