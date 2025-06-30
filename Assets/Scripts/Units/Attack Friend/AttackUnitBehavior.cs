using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackUnitBehavior : UnitBehavior
{
    protected Enemy targetEnemy;

    protected virtual void FindNearestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectRadius);

        Enemy chosenEnemy = null;
        if (colliders.Length > 0)
        {
            float minDst = float.MaxValue;
            foreach (var collider in colliders)
            {
                //if (!CheckUnblocked(collider.transform)) continue;
                if (!collider.TryGetComponent(out Enemy enemy) || enemy.Info.IsDead)
                {
                    continue; // Skip dead enemies
                }

                float dst = Vector2.Distance(collider.transform.position, transform.position);
                if (dst < minDst)
                {
                    minDst = dst;
                    chosenEnemy = enemy;
                }
            }
        }

        targetEnemy = chosenEnemy;
        targetTF = chosenEnemy != null ? chosenEnemy.transform : null;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.transform.TryGetComponent(out Enemy enemy) && !enemy.Info.IsDead)
        {
            DoDamage(enemy);
        }
    }

    protected abstract void DoDamage(Enemy enemy);
}
