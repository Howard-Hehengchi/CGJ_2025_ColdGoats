using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitInfo : MonoBehaviour
{
    [SerializeField, Min(1)] int maxHealth = 10;
    private int Health { get => health;
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);
            CheckHealthbar();
            if(NeedHealthbar && healthbarComponent != null)
            {
                healthbarComponent.SetValue((float)health / maxHealth);
            }
        }
    }
    private int health = 0;

    [SerializeField, Min(1)] int maxPotentialHealth = 10;
    private int potentalHealth = 0;

    public bool IsDead => health <= 0;

    private Action OnAlive;
    public void AddOnAlive(Action _OnAlive)
    {
        OnAlive -= _OnAlive;
        OnAlive += _OnAlive;
    }

    [SerializeField] CardFlip cardComponent;

    [SerializeField] Healthbar healthbarPrefab;
    public bool NeedHealthbar = false;
    private Healthbar healthbarComponent;

    private void Awake()
    {
        AddOnAlive(CheckHealthbar);

        healthbarComponent = GetComponentInChildren<Healthbar>();

        Health = maxHealth;
        potentalHealth = maxPotentialHealth;
    }

    private void CheckHealthbar()
    {
        if(NeedHealthbar && healthbarComponent == null)
        {
            healthbarComponent = Instantiate(healthbarPrefab, transform.position, Quaternion.identity, transform);
            healthbarComponent.GetComponent<HealthbarUnit>().EntityInfo = this;
        }
    }

    public void SetInitState(int _health)
    {
        health = _health;
    }

    public void Hurt(int amount = 1)
    {
        if (IsDead)
        {
            potentalHealth -= amount;
            if(potentalHealth <= 0)
            {
                OnAlive?.Invoke();
                Health = maxHealth;

                if (NeedHealthbar)
                {
                    cardComponent.DoFlip();
                }
            }
        }
        else
        {
            Health -= amount;
            if (Health <= 0)
            {
                if (!NeedHealthbar)
                {
                    Destroy(gameObject);
                    return;
                }

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
