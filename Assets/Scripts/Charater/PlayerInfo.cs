using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private int maxHealth = 30;
    private int health = 30;

    [SerializeField] GameObject hitMask;

    private void Start()
    {
        health = maxHealth;
        invincibleTimer = invincibleTime;
    }

    private float invincibleTime = 1f;
    private float invincibleTimer = 0f;

    private void Update()
    {
        invincibleTimer += Time.deltaTime;
    }

    public void OnHit(int amount = 1)
    {
        if(amount > 0)
        {
            if (invincibleTimer <= invincibleTime) return;

            health -= amount;

            invincibleTimer = 0f;
            StartCoroutine(HitFlash());
            if (health <= 0)
            {
                Debug.Log("Player Dead");
            }
        }
        else
        {
            health -= amount; // Negative amount heals the player
        }
    }

    private IEnumerator HitFlash()
    {
        int flashCycles = 4;
        float interval = invincibleTime / (2f * flashCycles - 1f);
        for (int i = 0; i < flashCycles - 1; i++)
        {
            hitMask.SetActive(true);
            yield return new WaitForSeconds(interval);
            hitMask.SetActive(false);
            yield return new WaitForSeconds(interval);
        }

        hitMask.SetActive(true);
        yield return new WaitForSeconds(interval);
        hitMask.SetActive(false);
    }
}
