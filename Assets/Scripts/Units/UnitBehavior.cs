using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBehavior : MonoBehaviour
{
    protected UnitInfo Info
    {
        get
        {
            if(info == null)
                info = GetComponent<UnitInfo>();
            return info;
        }
    }
    protected UnitInfo info;
    protected Rigidbody2D body2D;

    protected Transform targetTF;

    [SerializeField] protected float detectRadius = 6f;

    protected virtual void Start()
    {
        info = GetComponent<UnitInfo>();
        body2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (Info.IsDead) return;
    }

    protected virtual void FindNearestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectRadius);

        Transform targetEnemy = null;
        if (colliders.Length > 0)
        {
            float minDst = float.MaxValue;
            foreach (var collider in colliders)
            {
                //if (!CheckUnblocked(collider.transform)) continue;
                if (!collider.TryGetComponent(out Enemy enemy) || enemy.info.IsDead)
                {
                    continue; // Skip dead enemies
                }

                float dst = Vector2.Distance(collider.transform.position, transform.position);
                if (dst < minDst)
                {
                    minDst = dst;
                    targetEnemy = collider.transform;
                }
            }
        }

        targetTF = targetEnemy;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (Info.IsDead) return;

        if (collision.transform.TryGetComponent(out Enemy enemy) && !enemy.info.IsDead)
        {
            DoDamage(enemy);

            Destroy(gameObject);
        }
    }

    protected abstract void DoDamage(Enemy enemy);
}
