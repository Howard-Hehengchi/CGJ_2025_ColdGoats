using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHitable
{
    [HideInInspector]
    public UnitInfo info;

    private bool canMove = true;

    [SerializeField] float targetDetectDst = 5f;
    [SerializeField] LayerMask detectLayer;
    private Vector2 targetPoint;

    [SerializeField] float moveSpeed = 3f;
    private Rigidbody2D body2D;

    [Header("攻击参数")]
    [SerializeField] float attackDst = 3f;
    [SerializeField] float dashPower = 50f;
    [SerializeField] float attackPrepTime = 0.5f;
    [SerializeField] float attackInterval = 1f;

    private void Start()
    {
        info = GetComponent<UnitInfo>();
        body2D = GetComponent<Rigidbody2D>();

        targetPoint = Vector2.positiveInfinity;
        targetUpdateTimer = targetUpdateInterval;
        canMove = true;

        targetPoint = PlayerController.Instance.transform.position;
    }

    private float targetUpdateInterval = 0.5f;
    private float targetUpdateTimer = 0f;

    private void FixedUpdate()
    {
        if (info.IsDead) return;
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
            body2D.AddForce(14f * moveSpeed * targetOffset.normalized);
        }
    }

    private bool ObservePlayer(Vector2 playerDir)
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, playerDir, targetDetectDst, detectLayer);
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

        if (info.IsDead) yield break; // 如果在攻击准备期间死亡，则不执行冲刺

        body2D.AddForce(direction * dashPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(attackInterval);
        canMove = true;
        targetUpdateTimer = targetUpdateInterval; // 攻击之后立即寻路
    }

    public void OnHit(Vector2 position, int hitAmount = 1)
    {
        info.Hurt(hitAmount);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (info.IsDead) return;

        if (collision.collider.TryGetComponent(out PlayerInfo playerInfo))
        {
            playerInfo.OnHit();
        }
    }
}
