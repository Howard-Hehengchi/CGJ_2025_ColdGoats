using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendUnitBehavior : UnitBehavior
{
    protected Transform playerTF;

    protected override void Start()
    {
        base.Start();
        playerTF = PlayerController.Instance.transform;
    }
}
