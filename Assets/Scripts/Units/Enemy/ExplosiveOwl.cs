using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosiveOwl : Enemy
{
    [Header("¹¥»÷²ÎÊý")]
    [SerializeField] float attackDst = 0.7f;

    protected override void Start()
    {
        base.Start();
        exploded = false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!canMove) return;
        if (Info.IsDead) return;

        Vector2 targetOffset = targetPoint - (Vector2)transform.position;
        if (TargetIsPlayer && targetOffset.magnitude <= attackDst)
        {
            Explode();
        }
        else
        {
            body2D.AddForce(14f * moveSpeed * targetOffset.normalized);
        }
    }

    private bool exploded = false;
    private void Explode()
    {
        if (exploded) return;
        exploded = true;

        VFXManager.Instance.GenerateExplosionVFX(transform.position, 0.2f);

        var colliders = Physics2D.OverlapCircleAll(transform.position, 2.5f);

        for (int i = colliders.Length - 1; i >= 0; i--)
        {
            if (colliders[i].TryGetComponent(out UnitBehavior unit))
            {
                Destroy(unit.gameObject);
            }

            else if (colliders[i].TryGetComponent(out PlayerInfo playerInfo))
            {
                playerInfo.OnHit(damage);   
            }
        }

        Destroy(gameObject);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (Info.IsDead) return;
        base.OnCollisionEnter2D(collision);

        if (collision.collider.TryGetComponent(out PlayerInfo _))
        {
            Explode();
        }
        else if (collision.collider.TryGetComponent(out UnitBehavior _) && !collision.collider.TryGetComponent(out FireLantern _))
        {
            Explode();
        }

    }
}
