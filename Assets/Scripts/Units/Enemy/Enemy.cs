using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHitable
{
    [HideInInspector]
    public UnitInfo Info;

    protected bool canMove = true;

    [SerializeField] protected float targetDetectDst = 5f;
    [SerializeField] protected LayerMask detectLayer;

    protected Transform playerTF;
    protected Vector2 targetPoint;

    protected bool TargetIsPlayer 
    { 
        get 
        {
            Vector2 playerPos = playerTF.position;
            return targetPoint == playerPos;
        }
    }

    [SerializeField] protected float moveSpeed = 3f;
    protected Rigidbody2D body2D;

    protected virtual void Start()
    {
        Info = GetComponent<UnitInfo>();
        body2D = GetComponent<Rigidbody2D>();

        playerTF = PlayerController.Instance.transform;
        targetPoint = playerTF.position;

        targetUpdateTimer = targetUpdateInterval;
        canMove = true;
    }

    protected float targetUpdateInterval = 0.5f;
    protected float targetUpdateTimer = 0f;

    protected virtual void FixedUpdate()
    {
        if (Info.IsDead) return;
        if (!canMove) return;

        Vector2 playerPos = playerTF.position;

        targetUpdateTimer += Time.fixedDeltaTime;
        if(targetUpdateTimer >= targetUpdateInterval)
        {
            targetUpdateTimer = 0f;

            if (PlayerObservable(playerPos))
            {
                targetPoint = playerPos;
            }
        }
    }

    private bool PlayerObservable(Vector2 playerPosition)
    {
        Vector2 offset = playerPosition - (Vector2)transform.position;
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, offset.normalized, offset.magnitude, detectLayer);
        if (hit2D && !hit2D.transform.CompareTag("Player"))
        {
            return false;
        }

        return true;
    }

    public void OnHit(Vector2 position, int hitAmount = 1)
    {
        Info.Hurt(hitAmount);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Info.IsDead) return;

        if (collision.collider.TryGetComponent(out PlayerInfo playerInfo))
        {
            playerInfo.OnHit();
        }
    }
}
