using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private int maxHealth = 30;
    private int Health
    {
        get { return health; }
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);

            if (health <= 0)
            {
                GameManager.Instance.PlayerDie();
            }

            UpdateHealthbar(health / (float)maxHealth);
        }
    }
    private int health = 25;

    [SerializeField] GameObject hitMask;

    [SerializeField] SpriteRenderer gunForegroundSpRenderer;
    private Vector2 defaultSize = new Vector2(2f, 2f);
    private Vector2 defaultPos = new Vector2(0f, 0f);

    private Vector2 lowSize = new Vector2(2f, 0.96f);
    private Vector2 lowPos = new Vector2(0f, -0.52f);

    private void Start()
    {
        health = maxHealth;
        invincibleTimer = invincibleTime;
    }

    private float invincibleTime = 0.6f;
    private float invincibleTimer = 0f;

    private void Update()
    {
        invincibleTimer += Time.deltaTime;
    }

    private void UpdateHealthbar(float value)
    {
        gunForegroundSpRenderer.size = Vector2.Lerp(lowSize, defaultSize, value);
        gunForegroundSpRenderer.transform.localPosition = Vector2.Lerp(lowPos, defaultPos, value);
    }

    public void OnHit(int amount = 1, bool ignoreInvincible = false)
    {
        if(amount > 0)
        {
            if (!ignoreInvincible && invincibleTimer <= invincibleTime) return;

            Health -= amount;

            invincibleTimer = 0f;
            if(flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }
            flashCoroutine = StartCoroutine(HitFlash());
            if (Health <= 0)
            {
                GameManager.Instance.PlayerDie();
            }
        }
        else
        {
            Health -= amount; // Negative amount heals the player
        }
    }

    private Coroutine flashCoroutine = null;
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
