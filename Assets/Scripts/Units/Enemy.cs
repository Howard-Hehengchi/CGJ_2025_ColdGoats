using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHitable
{
    private int maxHealth = 25;
    private int health = 5;

    private bool IsDead { get { return health <= 0; } }
    private bool canMove = true;

    [SerializeField] float targetDetectDst = 5f;
    private Vector2 targetPoint;

    [SerializeField] float moveSpeed = 3f;
    private Rigidbody2D body2D;

    [Header("攻击参数")]
    [SerializeField] float attackDst = 3f;
    [SerializeField] float dashPower = 50f;
    [SerializeField] float attackPrepTime = 0.5f;
    [SerializeField] float attackInterval = 1f;

    [Header("视效表现")]
    [SerializeField] CardFlip cardComponent;

    private void Start()
    {
        body2D = GetComponent<Rigidbody2D>();

        health = maxHealth;
        targetPoint = Vector2.positiveInfinity;
        targetUpdateTimer = targetUpdateInterval;
        canMove = true;

        targetPoint = PlayerController.Instance.transform.position;
    }

    private float targetUpdateInterval = 0.5f;
    private float targetUpdateTimer = 0f;

    private void FixedUpdate()
    {
        if (IsDead) return;
        if (!canMove) return;

        Vector2 playerPos = PlayerController.Instance.transform.position;

        targetUpdateTimer += Time.fixedDeltaTime;
        if(targetUpdateTimer >= targetUpdateInterval)
        {
            targetUpdateTimer = 0f;

            Vector2 playerDir = playerPos - (Vector2)transform.position;
            if (ObservePlayer(playerDir))
            {
                targetPoint = playerPos;
            }
        }

        Vector2 targetOffset = targetPoint - (Vector2)transform.position;
        if (targetPoint == playerPos && targetOffset.magnitude <= attackDst)
        {
            StartCoroutine(Dash(targetOffset.normalized));
        }
        else
        {
            body2D.AddForce(targetOffset.normalized * moveSpeed * 10f);
        }
    }

    private bool ObservePlayer(Vector2 playerDir)
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, playerDir, targetDetectDst);
        if (hit2D && !hit2D.transform.CompareTag("Player"))
        {
            return false;
        }

        return true;
    }

    private IEnumerator Dash(Vector2 direction)
    {
        canMove = false;
        yield return new WaitForSeconds(attackPrepTime);

        body2D.AddForce(direction * dashPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(attackInterval);
        canMove = true;
        targetUpdateTimer = targetUpdateInterval; // 攻击之后立即寻路
    }

    public void OnHit(Vector2 direction)
    {
        if (IsDead) return;

        health--;
        if(health <= 0)
        {
            cardComponent.DoFlip(direction.x);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out PlayerInfo playerInfo))
        {
            playerInfo.OnHit();
        }
    }
}
