using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IHitable
{
    [SerializeField] CardFlip cardComponent;

    public void OnHit(Vector2 direction)
    {
        cardComponent.DoFlip(direction.x);
    }
}
