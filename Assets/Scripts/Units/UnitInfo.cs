using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfo : MonoBehaviour
{
    [SerializeField, Min(1)] int maxHealth = 10;
    private int health = 0;

    [SerializeField, Min(1)] int maxPotentialHealth = 10;
    private int potentalHealth = 0;

    public bool IsDead => health <= 0;

    [SerializeField] CardFlip cardComponent;
    private void Start()
    {
        health = maxHealth;
        potentalHealth = maxPotentialHealth;
    }

    public void Hurt(int amount = 1)
    {
        if (IsDead)
        {
            potentalHealth -= amount;
            if(potentalHealth <= 0)
            {
                health = maxHealth;
                cardComponent.DoFlip();
            }
        }
        else
        {
            health -= amount;
            if (health <= 0)
            {
                potentalHealth = maxPotentialHealth;
                cardComponent.DoFlip();
            }
        }
    }

    public void BackFlip()
    {
        cardComponent.ForceUp(_isUp: false);
        cardComponent.DoFlip();
    }
}
