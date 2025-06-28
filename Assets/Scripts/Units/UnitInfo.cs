using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfo : MonoBehaviour
{
    [SerializeField, Min(1)] int maxHealth = 10;
    private int Health { get => health;
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);
            if(healthbarComponent != null)
            {
                healthbarComponent.SetValue((float)health / maxHealth);
            }
        }
    }
    private int health = 0;

    [SerializeField, Min(1)] int maxPotentialHealth = 10;
    private int potentalHealth = 0;

    public bool IsDead => health <= 0;

    [SerializeField] CardFlip cardComponent;
    [SerializeField] Healthbar healthbarComponent;
    private void Start()
    {
        Health = maxHealth;
        potentalHealth = maxPotentialHealth;
    }

    public void Hurt(int amount = 1)
    {
        if (IsDead)
        {
            potentalHealth -= amount;
            if(potentalHealth <= 0)
            {
                Health = maxHealth;
                cardComponent.DoFlip();
            }
        }
        else
        {
            Health -= amount;
            if (Health <= 0)
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
